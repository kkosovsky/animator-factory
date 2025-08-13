using System;
using System.Collections.Generic;
using System.Linq;
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
        /// <param name="replacementSpritesPath">The path to look for replacement animation sprites</param>
        public static void CreateAnimatorOverrideControllerAsSubAsset(
            GameObject prefabVariant,
            string replacementSpritesPath
        )
        {
            if (!IsValidVariant(variant: prefabVariant, path: replacementSpritesPath))
            {
                return;
            }

            Animator[] animators = prefabVariant.GetComponentsInChildren<Animator>(includeInactive: true);

            foreach (Animator animator in animators)
            {
                if (IsOriginalAnimatorValid(animator: animator))
                {
                    CreateOverrideControllerForAnimator(
                        prefabVariant: prefabVariant,
                        originalAnimator: animator,
                        replacementSpritesPath: replacementSpritesPath
                    );
                }
            }

            // TODO: Uncomment - Save all changes
            //PrefabUtility.SavePrefabAsset(asset: prefabVariant);
            //AssetDatabase.SaveAssets();
        }

        static bool IsValidVariant(GameObject variant, string path)
        {
            if (variant == null)
            {
                Debug.LogError(message: "PrefabVariant is null");
                return false;
            }

            if (string.IsNullOrEmpty(value: path))
            {
                Debug.LogError(message: "Sprite source path is null");
                return false;
            }

            return true;
        }

        static void CreateOverrideControllerForAnimator(
            GameObject prefabVariant,
            Animator originalAnimator,
            string replacementSpritesPath
        )
        {
            AnimatorController originalController = originalAnimator.runtimeAnimatorController as AnimatorController;
            AnimatorOverrideController overrideController = new(controller: originalController);
            List<KeyValuePair<AnimationClip, AnimationClip>> overrides = new List<
                KeyValuePair<AnimationClip, AnimationClip>
            >();

            overrideController.GetOverrides(overrides: overrides);

            List<KeyValuePair<AnimationClip, AnimationClip>> newAnimationClips = GetNewAnimationClips(
                originalStates: AnimatorStateAnalysisService.GetAllAnimatorStates(controller: originalController),
                overrides: overrides,
                replacementSpritesPath: replacementSpritesPath
            );

            overrideController.ApplyOverrides(overrides: newAnimationClips);
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
            List<AnimatorState> originalStates,
            List<KeyValuePair<AnimationClip, AnimationClip>> overrides,
            string replacementSpritesPath
        )
        {
            Dictionary<string, List<Sprite>> sprites = LoadAllSpritesRecursively(
                states: originalStates,
                rootPath: replacementSpritesPath
            );

            for (int i = 0; i < overrides.Count; i++)
            {
                KeyValuePair<AnimationClip, AnimationClip> @override = overrides[index: i];
                overrides[index: i] = new KeyValuePair<AnimationClip, AnimationClip>(
                    key: @override.Key,
                    value: MakeAnimationClip(original: @override.Key)
                );
            }

            return overrides;
        }

        static AnimationClip MakeAnimationClip(AnimationClip original)
        {
            // TODO: Implement
            return original;
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
                        continue;
                    }

                    string spriteName = sprite.name.ToLower();
                    bool matchingStateExists =
                        states.Any(predicate: state => spriteName.Contains(value: state.name.ToLower()));
                    if (!matchingStateExists)
                    {
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
                        newKeyframes.Add(sprite);
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
    }
}
