using System;
using System.Collections.Generic;
using System.Linq;
using AnimatorFactory.GenerationControls;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using Object = UnityEngine.Object;

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
            RuntimeAnimatorController originalController = originalAnimator.runtimeAnimatorController;
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
                variantSpritesPath: prefabVariant.fullSpritesSourcePath,
                fallbackSpritePath: prefabVariant.fallbackSpritePath
            );

            overrideController.ApplyOverrides(overrides: newAnimationClips);
            string overrideControllerName = $"{prefabVariant.name}_AnimatorOverride";
            overrideController.name = overrideControllerName;

            RemoveExistingSubAssetByName(
                mainAsset: prefabVariant.gameObject,
                assetName: overrideControllerName,
                assetType: typeof(AnimatorOverrideController)
            );

            AssetDatabase.AddObjectToAsset(objectToAdd: overrideController, assetObject: prefabVariant.gameObject);
            variantAnimator.runtimeAnimatorController = overrideController;
            PrefabUtility.RecordPrefabInstancePropertyModifications(targetObject: originalAnimator);
        }

        static void RemoveExistingSubAssetByName(GameObject mainAsset, string assetName, Type assetType)
        {
            Object[] subAssets =
                AssetDatabase.LoadAllAssetsAtPath(assetPath: AssetDatabase.GetAssetPath(assetObject: mainAsset));

            foreach (Object subAsset in subAssets)
            {
                if (subAsset == null)
                {
                    continue;
                }

                if (subAsset == mainAsset)
                {
                    continue;
                }

                bool nameMatches = subAsset.name == assetName;
                bool typeMatches = assetType.IsInstanceOfType(o: subAsset);

                if (!nameMatches || !typeMatches)
                {
                    continue;
                }

                Debug.Log(message: $"Removing existing {assetType.Name}: {assetName}");
                AssetDatabase.RemoveObjectFromAsset(objectToRemove: subAsset);
                Object.DestroyImmediate(obj: subAsset, allowDestroyingAssets: true);
                break;
            }
        }

        static List<KeyValuePair<AnimationClip, AnimationClip>> GetNewAnimationClips(
            PrefabVariant variant,
            List<AnimatorState> originalStates,
            List<KeyValuePair<AnimationClip, AnimationClip>> overrides,
            string variantSpritesPath,
            string fallbackSpritePath
        )
        {
            Dictionary<string, List<Sprite>> sprites = LoadAllSpritesRecursively(
                states: originalStates,
                rootPath: variantSpritesPath,
                fallbackSpritePath: fallbackSpritePath
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

        static Dictionary<string, List<Sprite>> LoadAllSpritesRecursively(
            List<AnimatorState> states,
            string rootPath,
            string fallbackSpritePath
        )
        {
            Dictionary<string, List<Sprite>> spriteDict = new Dictionary<string, List<Sprite>>();

            if (!AssetDatabase.IsValidFolder(path: rootPath))
            {
                LoadFallbackSpriteForEveryState(
                    states: states,
                    spriteDict: spriteDict,
                    fallbackSpritePath: fallbackSpritePath
                );
            }
            else
            {
                string[] guids = AssetDatabase.FindAssets(filter: "t:Sprite", searchInFolders: new[] { rootPath });
                if (!guids.Any())
                {
                    LoadFallbackSpriteForEveryState(
                        states: states,
                        spriteDict: spriteDict,
                        fallbackSpritePath: fallbackSpritePath
                    );
                }
                else
                {
                    LoadActualAnimSprites(
                        states: states,
                        guids: guids,
                        spriteDict: spriteDict,
                        fallbackSpritePath: fallbackSpritePath
                    );
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

        static void LoadFallbackSpriteForEveryState(
            List<AnimatorState> states,
            Dictionary<string, List<Sprite>> spriteDict,
            string fallbackSpritePath
        )
        {
            Sprite fallbackSprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath: fallbackSpritePath);
            if (fallbackSprite == null)
            {
                Debug.LogError(message: $"Could not find fallback sprite at path: {fallbackSpritePath}");
                return;
            }

            foreach (AnimatorState state in states)
            {
                spriteDict[key: state.name] = new List<Sprite> { fallbackSprite };
            }
        }

        static void LoadActualAnimSprites(
            List<AnimatorState> states,
            string[] guids,
            Dictionary<string, List<Sprite>> spriteDict,
            string fallbackSpritePath
        )
        {
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid: guid);

                if (Helpers.IsFromUnityPackage(assetPath: path))
                {
                    continue;
                }

                List<Sprite> allAssets = AssetDatabase
                    .LoadAllAssetsAtPath(assetPath: path)
                    .OfType<Sprite>()
                    .ToList();

                foreach (AnimatorState state in states)
                {
                    string stateName = state.name;
                    if (!allAssets.Any(predicate: sprite => sprite.name.Contains(value: stateName)))
                    {
                        continue;
                    }

                    {
                        List<Sprite> stateSprites =
                            allAssets.Where(predicate: sprite => sprite.name.Contains(value: stateName)).ToList();
                        List<Sprite> newKeyframes = new List<Sprite>(collection: stateSprites);
                        if (!spriteDict.ContainsKey(stateName))
                        {
                            spriteDict[stateName] = newKeyframes;
                            continue;
                        }

                        spriteDict[key: stateName].AddRange(newKeyframes);
                    }
                }
            }
            
            Sprite fallbackSprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath: fallbackSpritePath);
            foreach (AnimatorState state in states)
            {
                if (!spriteDict.ContainsKey(state.name))
                {
                    spriteDict[key: state.name] = new List<Sprite> { fallbackSprite };
                    continue;
                }

                if (!spriteDict[state.name].Any())
                {
                    spriteDict[key: state.name] = new List<Sprite> { fallbackSprite };
                }
            }
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
