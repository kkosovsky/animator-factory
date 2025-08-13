using System.Collections.Generic;
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
    }
}
