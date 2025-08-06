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

        public void OnSpriteSelectionChanged(Sprite sprite)
        {
            _spriteEditionViewModel.LoadSprite(sprite: sprite);
        }

        public void Dispose() => UnbindEvents();

        void BindEvents()
        {
            _spriteEditionViewModel.SpriteChanged += _spriteEditionView.OnSpriteChanged;
            _spriteEditionViewModel.StatusChanged += _spriteEditionView.OnStatusChanged;
            _spriteEditionView.SpriteSelectionChanged += OnSpriteSelectionChanged;
        }

        void UnbindEvents()
        {
            _spriteEditionViewModel.SpriteChanged -= _spriteEditionView.OnSpriteChanged;
            _spriteEditionViewModel.StatusChanged -= _spriteEditionView.OnStatusChanged;
            _spriteEditionView.SpriteSelectionChanged -= OnSpriteSelectionChanged;
        }
    }
}
