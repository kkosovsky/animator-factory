using System;
using System.Collections.Generic;
using System.Linq;
using AnimatorFactory.GenerationControls;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace AnimatorFactory.PrefabVariants
{
    public static class PrefabVariantsEditionService
    {
        /// <summary>
        /// Replaces the AnimatorController with AnimatorOverrideController as a sub-asset
        /// </summary>
        /// <param name="prefabVariant">The prefab variant to modify</param>
        public static void CreateAnimatorOverrideControllerAsSubAsset(PrefabVariant prefabVariant)
        {
            if (!IsValidVariant(variant: prefabVariant.gameObject))
            {
                return;
            }

            Animator[] animators = prefabVariant.gameObject.GetComponentsInChildren<Animator>(includeInactive: true);

            foreach (Animator animator in animators)
            {
                if (IsOriginalAnimatorValid(animator: animator))
                {
                    CreateOverrideControllerForAnimator(
                        prefabVariant: prefabVariant,
                        originalAnimator: animator
                    );
                }
            }

            PrefabUtility.SavePrefabAsset(asset: prefabVariant.gameObject);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        static bool IsValidVariant(GameObject variant)
        {
            if (variant == null)
            {
                Debug.LogError(message: "PrefabVariant is null");
                return false;
            }

            return true;
        }

        static void CreateOverrideControllerForAnimator(
            PrefabVariant prefabVariant,
            Animator originalAnimator
        )
        {
            AnimatorController originalController = originalAnimator.runtimeAnimatorController as AnimatorController;
            AnimatorOverrideController overrideController = new(controller: originalController);
            List<KeyValuePair<AnimationClip, AnimationClip>> overrides = new List<
                KeyValuePair<AnimationClip, AnimationClip>
            >();

            overrideController.GetOverrides(overrides: overrides);

            List<KeyValuePair<AnimationClip, AnimationClip>> newAnimationClips = GetNewAnimationClips(
                variant: prefabVariant,
                originalStates: AnimatorStateAnalysisService.GetAllAnimatorStates(controller: originalController),
                overrides: overrides,
                replacementSpritesPath: prefabVariant.spriteSourcesDirPath
            );

            overrideController.ApplyOverrides(overrides: newAnimationClips);
            string overrideControllerName = $"{originalAnimator.gameObject.name}_AnimatorOverride";
            overrideController.name = overrideControllerName;

            AssetDatabase.AddObjectToAsset(objectToAdd: overrideController, assetObject: prefabVariant.gameObject);
            originalAnimator.runtimeAnimatorController = overrideController;
            PrefabUtility.RecordPrefabInstancePropertyModifications(targetObject: originalAnimator);
        }

        static bool IsOriginalAnimatorValid(Animator animator)
        {
            AnimatorController originalController = animator.runtimeAnimatorController as AnimatorController;
            if (originalController == null)
            {
                Debug.LogWarning(message: $"Animator on {animator.gameObject.name} doesn't have an AnimatorController");
                return false;
            }

            if (animator.runtimeAnimatorController is AnimatorOverrideController)
            {
                Debug.Log(message: $"Animator on {animator.gameObject.name} already has an override controller");
                return false;
            }

            return true;
        }

        static List<KeyValuePair<AnimationClip, AnimationClip>> GetNewAnimationClips(
            PrefabVariant variant,
            List<AnimatorState> originalStates,
            List<KeyValuePair<AnimationClip, AnimationClip>> overrides,
            string replacementSpritesPath
        )
        {
            Dictionary<string, List<Sprite>> sprites = LoadAllSpritesRecursively(
                states: originalStates,
                rootPath: replacementSpritesPath
            );

            Dictionary<AnimationClip, string> clipToStateName = CreateClipToStateMapping(states: originalStates);

            for (int i = 0; i < overrides.Count; i++)
            {
                KeyValuePair<AnimationClip, AnimationClip> @override = overrides[index: i];
                if
                (
                    clipToStateName.TryGetValue(key: @override.Key, value: out string stateName)
                    && sprites.TryGetValue(key: stateName, value: out List<Sprite> keyframes)
                )
                {
                    overrides[index: i] = new KeyValuePair<AnimationClip, AnimationClip>(
                        key: @override.Key,
                        value: MakeAnimationClip(
                            originalClip: @override.Key,
                            newKeyframes: keyframes.ToArray(),
                            variant: variant
                        )
                    );
                }
                else
                {
                    Debug.Log(message: $"{@override.Key.name} is not assigned to any of the original animator states!");
                }
            }

            return overrides;
        }


        static AnimationClip MakeAnimationClip(AnimationClip originalClip, Sprite[] newKeyframes, PrefabVariant variant)
        {
            return AnimationClipGenerationService.CreateAnimationClip(
                sprites: newKeyframes,
                keyframeCount: newKeyframes.Length,
                frameRate: originalClip.frameRate,
                hasLoopTime: false,
                wrapMode: WrapMode.Clamp,
                animationName: $"{variant.name}_{originalClip.name}",
                destinationFolderPath: variant.generatedClipsPath
            );
        }

        static Dictionary<string, List<Sprite>> LoadAllSpritesRecursively(List<AnimatorState> states, string rootPath)
        {
            Dictionary<string, List<Sprite>> spriteDict = new Dictionary<string, List<Sprite>>();

            if (!AssetDatabase.IsValidFolder(path: rootPath))
            {
                return spriteDict;
            }

            string[] guids = AssetDatabase.FindAssets(filter: "t:Sprite", searchInFolders: new[] { rootPath });

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid: guid);

                if (Helpers.IsFromUnityPackage(assetPath: path))
                {
                    continue;
                }

                UnityEngine.Object[] allAssets = AssetDatabase.LoadAllAssetsAtPath(assetPath: path);

                foreach (UnityEngine.Object asset in allAssets)
                {
                    if (asset is not Sprite sprite)
                    {
                        Debug.Log(message: $"Asset is not a sprite: {asset.name}");
                        continue;
                    }

                    string spriteName = sprite.name.ToLower();
                    bool matchingStateExists =
                        states.Any(predicate: state => spriteName.Contains(value: state.name.ToLower()));
                    if (!matchingStateExists)
                    {
                        Debug.Log(message: $"{spriteName} doesn't match any existing state");
                        continue;
                    }

                    string stateName = states
                        .First(predicate: state => spriteName.Contains(value: state.name.ToLower()))
                        .name;
                    if (spriteDict.ContainsKey(key: stateName))
                    {
                        spriteDict[key: stateName].Add(item: sprite);
                    }
                    else
                    {
                        List<Sprite> newKeyframes = new List<Sprite>();
                        newKeyframes.Add(item: sprite);
                        spriteDict[key: stateName] = newKeyframes;
                    }
                }
            }

            foreach (List<Sprite> sprites in spriteDict.Values)
            {
                sprites.Sort(
                    comparison: (a, b) => string.Compare(
                        strA: a.name,
                        strB: b.name,
                        comparisonType: StringComparison.OrdinalIgnoreCase
                    )
                );
            }

            return spriteDict;
        }

        static Dictionary<AnimationClip, string> CreateClipToStateMapping(List<AnimatorState> states)
        {
            Dictionary<AnimationClip, string> clipToStateName = new Dictionary<AnimationClip, string>();

            foreach (AnimatorState state in states)
            {
                if (state.motion is AnimationClip clip)
                {
                    clipToStateName[key: clip] = state.name;
                }
                else if (state.motion is BlendTree blendTree)
                {
                    AddBlendTreeClipsToMapping(blendTree: blendTree, stateName: state.name, mapping: clipToStateName);
                }
            }

            return clipToStateName;
        }

        static void AddBlendTreeClipsToMapping(
            BlendTree blendTree,
            string stateName,
            Dictionary<AnimationClip, string> mapping
        )
        {
            foreach (ChildMotion childMotion in blendTree.children)
            {
                if (childMotion.motion is AnimationClip clip)
                {
                    mapping[key: clip] = stateName;
                }
                else if (childMotion.motion is BlendTree nestedBlendTree)
                {
                    AddBlendTreeClipsToMapping(blendTree: nestedBlendTree, stateName: stateName, mapping: mapping);
                }
            }
        }
    }
}
