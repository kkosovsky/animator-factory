using AnimatorFactory.PrefabVariants;
using UnityEngine;
using UnityEngine.UIElements;

namespace AnimatorFactory.Editor
{
    public class AnimatorFactoryController
    {
        readonly AnimatorStateEditionTabController _animatorStateEditionTabController = new();
        readonly SpriteEditionTabController _spriteEditionTabController = new();
        readonly PrefabVariantsEditionTabController _prefabVariantsEditionTabController = new();

        public void Dispose()
        {
            _animatorStateEditionTabController?.Dispose();
            _spriteEditionTabController?.Dispose();
            _prefabVariantsEditionTabController?.Dispose();
        }

        public VisualElement GetAnimatorStatesContent() => _animatorStateEditionTabController.GetContent();

        public VisualElement GetSpriteEditionContent() => _spriteEditionTabController.GetContent();
        public VisualElement GetPrefabVariantsContent() => _prefabVariantsEditionTabController.GetContent();

        public void OnTextureSelectionChanged(Texture2D texture)
        {
            _spriteEditionTabController.OnTextureSelectionChanged(texture: texture);
        }
    }
}
