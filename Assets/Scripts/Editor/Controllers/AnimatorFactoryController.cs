using AnimatorFactory.PrefabHierarchy;
using AnimatorFactory.SpriteKeyframePreview;
using AnimatorFactory.AnimatorStatePreview;
using AnimatorFactory.GenerationControls;
using UnityEngine;

namespace AnimatorFactory.Editor
{
    public class AnimatorFactoryController
    {
        readonly AnimatorStateEditionTabController _animatorStateEditionTabController;
        readonly SpriteEditionTabController _spriteEditionTabController;

        public AnimatorFactoryController(
            PrefabHierarchyView prefabHierarchyView,
            AnimatorStatesView animatorStatesView,
            SpriteKeyframesView spriteKeyframesView,
            GenerationControlsView generationControlsView
        )
        {
            PrefabHierarchyViewModel prefabHierarchyViewModel = new();
            AnimatorStatesViewModel animatorStatesViewModel = new();
            SpriteKeyframeViewModel spriteKeyframeViewModel = new();
            GenerationControlsViewModel generationControlsViewModel = new();

            _animatorStateEditionTabController = new AnimatorStateEditionTabController(
                prefabHierarchyViewModel: prefabHierarchyViewModel,
                animatorStatesViewModel: animatorStatesViewModel,
                spriteKeyframeViewModel: spriteKeyframeViewModel,
                generationControlsViewModel: generationControlsViewModel,
                prefabHierarchyView: prefabHierarchyView,
                animatorStatesView: animatorStatesView,
                spriteKeyframesView: spriteKeyframesView,
                generationControlsView: generationControlsView
            );

            _spriteEditionTabController = new SpriteEditionTabController();
        }

        public void OnPrefabSelectionChanged(GameObject prefab)
        {
            _animatorStateEditionTabController.OnPrefabSelectionChanged(prefab: prefab);
        }

        public void OnTextureSelectionChanged(Texture2D texture)
        {
            _spriteEditionTabController.OnTextureSelectionChanged(texture: texture);
        }

        public void Dispose()
        {
            _animatorStateEditionTabController?.Dispose();
            _spriteEditionTabController?.Dispose();
        }
    }
}
