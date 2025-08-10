using AnimatorFactory.PrefabHierarchy;
using AnimatorFactory.SpriteKeyframePreview;
using AnimatorFactory.AnimatorStatePreview;
using AnimatorFactory.GenerationControls;
using AnimatorFactory.PrefabVariants;
using AnimatorFactory.SpriteEdition;
using UnityEngine;

namespace AnimatorFactory.Editor
{
    public class AnimatorFactoryController
    {
        readonly AnimatorStateEditionTabController _animatorStateEditionTabController;
        readonly SpriteEditionTabController _spriteEditionTabController;
        readonly PrefabVariantsEditionTabController _prefabVariantsEditionTabController;

        public AnimatorFactoryController(
            PrefabHierarchyView prefabHierarchyView,
            AnimatorStatesView animatorStatesView,
            SpriteKeyframesView spriteKeyframesView,
            GenerationControlsView generationControlsView,
            SpriteEditionView spriteEditionView,
            PrefabVariantsEditionView prefabEditionView
        )
        {
            PrefabHierarchyViewModel prefabHierarchyViewModel = new();
            AnimatorStatesViewModel animatorStatesViewModel = new();
            SpriteKeyframeViewModel spriteKeyframeViewModel = new();
            GenerationControlsViewModel generationControlsViewModel = new();
            SpriteEditionViewModel spriteEditionViewModel = new();

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

            _spriteEditionTabController = new SpriteEditionTabController(
                spriteEditionViewModel: spriteEditionViewModel,
                spriteEditionView: spriteEditionView
            );

            _prefabVariantsEditionTabController = new PrefabVariantsEditionTabController(
                view: prefabEditionView,
                viewModel: new PrefabVariantsEditionViewModel()
            );
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
            _prefabVariantsEditionTabController?.Dispose();
        }
    }
}
