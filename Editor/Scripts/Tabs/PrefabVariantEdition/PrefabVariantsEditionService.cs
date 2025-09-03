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
        /// <param name="originalAnimatorGameObject">The original animator we want to override</param>
        /// <param name="prefabVariant">The prefab variant to modify</param>
        public static void CreateAnimatorOverrideControllerAsSubAsset(
            PrefabHierarchyListItem originalAnimatorGameObject,
            PrefabVariant prefabVariant
        )
        {
            Animator originalAnimator = originalAnimatorGameObject.gameObject.GetComponent<Animator>();

            if (!IsValidVariant(originalAnimator: originalAnimator, variant: prefabVariant))
            {
                return;
            }

            if (!IsOriginalAnimatorValid(animator: originalAnimator))
            {
                return;
            }

            CreateOverrideControllerForAnimator(
                originalAnimator: originalAnimator,
                prefabVariant: prefabVariant
            );

            PrefabUtility.SavePrefabAsset(asset: prefabVariant.gameObject);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        static bool IsValidVariant(Animator originalAnimator, PrefabVariant variant)
        {
            if (variant == null)
            {
                Debug.LogError(message: "Variant is null");
                return false;
            }

            if (variant.gameObject == null)
            {
                Debug.LogError(
                    message: "Variant has no game object!"
                );

                return false;
            }

            List<Animator> animators = variant
                .gameObject.GetComponentsInChildren<Animator>(includeInactive: true)
                .Where(predicate: animator => animator.gameObject.name.Equals(value: originalAnimator.gameObject.name))
                .ToList();

            if (!animators.Any())
            {
                Debug.LogError(
                    message:
                    $"Variant doesn't not have a matching game object in its hierarchy! Variant game object: {variant.gameObject.name}, required game object: {originalAnimator.gameObject.name}"
                );

                return false;
            }

            return true;
        }

        static void CreateOverrideControllerForAnimator(Animator originalAnimator, PrefabVariant prefabVariant)
        {
            AnimatorController originalController = originalAnimator.runtimeAnimatorController as AnimatorController;
            Animator variantAnimator = prefabVariant
                .gameObject
                .GetComponentsInChildren<Animator>()
                .First(predicate: animator => animator.gameObject.name.Equals(value: originalAnimator.gameObject.name));

            if (variantAnimator == null)
            {
                Debug.LogError(
                    message:
                    $"Didn't find matching child object with animator where object name is {originalAnimator.gameObject.name}"
                );
                return;
            }

            AnimatorOverrideController overrideController = new(controller: originalController);
            List<KeyValuePair<AnimationClip, AnimationClip>> overrides = new List<
                KeyValuePair<AnimationClip, AnimationClip>
            >();

            overrideController.GetOverrides(overrides: overrides);

            List<KeyValuePair<AnimationClip, AnimationClip>> newAnimationClips = GetNewAnimationClips(
                variant: prefabVariant,
                originalStates: AnimatorStateAnalysisService.GetAllAnimatorStates(controller: originalController),
                overrides: overrides,
                variantSpritesPath: prefabVariant.fullSpritesSourcePath
            );

            overrideController.ApplyOverrides(overrides: newAnimationClips);
            string overrideControllerName = $"{prefabVariant.name}_AnimatorOverride";
            overrideController.name = overrideControllerName;

            AssetDatabase.AddObjectToAsset(objectToAdd: overrideController, assetObject: prefabVariant.gameObject);
            variantAnimator.runtimeAnimatorController = overrideController;
            PrefabUtility.RecordPrefabInstancePropertyModifications(targetObject: originalAnimator);
        }

        static bool IsOriginalAnimatorValid(Animator animator)
        {
            AnimatorController originalController = animator.runtimeAnimatorController as AnimatorController;
            if (originalController == null)
            {
                Debug.LogError(message: $"Animator on {animator.gameObject.name} doesn't have an AnimatorController");
                return false;
            }

            if (animator.runtimeAnimatorController is AnimatorOverrideController)
            {
                Debug.LogError(message: $"Animator on {animator.gameObject.name} already has an override controller");
                return false;
            }

            return true;
        }

        static List<KeyValuePair<AnimationClip, AnimationClip>> GetNewAnimationClips(
            PrefabVariant variant,
            List<AnimatorState> originalStates,
            List<KeyValuePair<AnimationClip, AnimationClip>> overrides,
            string variantSpritesPath
        )
        {
            Dictionary<string, List<Sprite>> sprites = LoadAllSpritesRecursively(
                states: originalStates,
                rootPath: variantSpritesPath
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
            string newClipName = $"{variant.name}_{originalClip.name.Split(separator: '_').Last()}";
            return AnimationClipGenerationService.CreateAnimationClip(
                sprites: newKeyframes,
                keyframeCount: newKeyframes.Length,
                frameRate: originalClip.frameRate,
                hasLoopTime: originalClip.isLooping,
                wrapMode: WrapMode.Clamp,
                animationName: newClipName,
                destinationFolderPath: variant.fullClipsDestinationPath
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
