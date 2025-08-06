using AnimatorFactory.SpriteEdition;
using UnityEngine;

namespace AnimatorFactory.Editor
{
    public class SpriteEditionTabController
    {
        readonly SpriteEditionViewModel _spriteEditionViewModel;
        readonly SpriteEditionView _spriteEditionView;

        public SpriteEditionTabController(
            SpriteEditionViewModel spriteEditionViewModel,
            SpriteEditionView spriteEditionView
        )
        {
            _spriteEditionViewModel = spriteEditionViewModel;
            _spriteEditionView = spriteEditionView;

            BindEvents();
        }

        public void OnTextureSelectionChanged(Texture2D texture)
        {
            _spriteEditionViewModel.LoadTexture(texture: texture);
        }

        public void Dispose() => UnbindEvents();

        void BindEvents()
        {
            _spriteEditionViewModel.TextureChanged += _spriteEditionView.OnTextureChanged;
            _spriteEditionViewModel.StatusChanged += _spriteEditionView.OnStatusChanged;
            _spriteEditionView.TextureSelectionChanged += OnTextureSelectionChanged;
        }

        void UnbindEvents()
        {
            _spriteEditionViewModel.TextureChanged -= _spriteEditionView.OnTextureChanged;
            _spriteEditionViewModel.StatusChanged -= _spriteEditionView.OnStatusChanged;
            _spriteEditionView.TextureSelectionChanged -= OnTextureSelectionChanged;
        }
    }
}
