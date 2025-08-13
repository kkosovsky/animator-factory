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
                CreateOverrideAnimator(variant: variant, validAnimatorStates: validAnimatorStates);
            }
        }

        void CreateOverrideAnimator(GameObject variant, List<AnimatorState> validAnimatorStates)
        {
            List<AnimatorController> allAnimatorControllers = GetAllObjectsWithAnimator(rootObject: variant)
                .Select(
                    selector: gameObject =>
                        gameObject.GetComponent<Animator>().runtimeAnimatorController as AnimatorController
                )
                .ToList();
            
            foreach (AnimatorController animatorController in allAnimatorControllers)
            {
                List<SpriteAnimationInfo> animationInfos = GetSpriteAnimationInfos(
                    animatorController: animatorController,
                    validAnimatorStates: validAnimatorStates
                );
                
                Debug.Log(animationInfos);
            }
        }

        List<SpriteAnimationInfo> GetSpriteAnimationInfos(
            AnimatorController animatorController,
            List<AnimatorState> validAnimatorStates
        ) => animatorController.animationClips
            .Select(SpriteInfoExtractionService.ExtractSpriteKeyframes)
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
