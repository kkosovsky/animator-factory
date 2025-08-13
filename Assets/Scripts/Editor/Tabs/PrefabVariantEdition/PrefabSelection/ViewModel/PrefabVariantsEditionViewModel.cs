using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;

namespace AnimatorFactory.PrefabVariants
{
    public class PrefabVariantsEditionViewModel
    {
        string spriteSourcePath = $"Assets{Path.DirectorySeparatorChar}";
        GameObject _rootPrefab;
        GameObject[] _variants;

        public void SourceFolderChanged(string path) => spriteSourcePath = path;

        public void PrefabSelected(GameObject prefab) => _rootPrefab = prefab;

        public void VariantsSelected(GameObject[] variants) => _variants = variants;

        public void OnGenerateClicked()
        {
            List<AnimatorState> validAnimatorStates = GetAllAnimatorStatesInParentHierarchy();
            foreach (GameObject variant in _variants)
            {
                CreateOverrideAnimator(variant: variant, spritesSourcePath: spriteSourcePath);
            }
        }

        void CreateOverrideAnimator(GameObject variant, string spritesSourcePath)
        {
            PrefabVariantsEditionService.CreateAnimatorOverrideControllerAsSubAsset(
                prefabVariant: variant,
                replacementSpritesPath: spritesSourcePath
            );
        }

        List<SpriteAnimationInfo> GetSpriteAnimationInfos(
            AnimatorController animatorController,
            List<AnimatorState> validAnimatorStates
        ) => animatorController
            .animationClips
            .Select(selector: SpriteInfoExtractionService.ExtractSpriteKeyframes)
            .ToList();

        List<AnimatorState> GetAllAnimatorStatesInParentHierarchy()
        {
            List<GameObject> parentPrefabObjectsWithAnimator = GetAllObjectsWithAnimator(rootObject: _rootPrefab);

            IEnumerable<AnimatorState> allAnimatorStatesInParentHierarchy = parentPrefabObjectsWithAnimator.SelectMany(
                selector: gameObject =>
                {
                    AnimatorController parentObjectAnimator =
                        gameObject.GetComponent<Animator>().runtimeAnimatorController as AnimatorController;
                    List<AnimatorState> parentObjectAnimatorStates =
                        AnimatorStateAnalysisService.GetAllAnimatorStates(controller: parentObjectAnimator);
                    return parentObjectAnimatorStates;
                }
            );

            return allAnimatorStatesInParentHierarchy.ToList();
        }

        List<GameObject> GetAllObjectsWithAnimator(GameObject rootObject)
        {
            List<GameObject> parentPrefabObjectsWithAnimator = new List<GameObject>();
            AnimatorStateAnalysisService.GetAllObjectsWithAnimator(
                gameObject: rootObject,
                objects: parentPrefabObjectsWithAnimator
            );

            return parentPrefabObjectsWithAnimator;
        }
    }
}
