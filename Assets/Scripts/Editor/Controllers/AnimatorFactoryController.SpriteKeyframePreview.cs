using AnimatorFactory.SpriteKeyframePreview;
using UnityEngine;

namespace AnimatorFactory.Editor
{
    public partial class AnimatorFactoryController
    {
        void BindSpriteKeyFramePreviewEvents()
        {
            _spriteKeyframeViewModel.StatusChanged += _spriteKeyframesView.OnStatusChanged;
            _spriteKeyframeViewModel.DataChanged += OnKeyframeDataChanged;
            
            _spriteKeyframesView.FrameRateChanged += OnFrameRateChanged;
            _spriteKeyframesView.TotalFramesChanged += OnTotalFramesChanged;
            _spriteKeyframesView.SpritesSelected += OnSpritesSelected;
        }

        void UnbindSpriteKeyFramePreviewEvents()
        {
            _spriteKeyframeViewModel.StatusChanged -= _spriteKeyframesView.OnStatusChanged;
            _spriteKeyframeViewModel.DataChanged -= OnKeyframeDataChanged;
            
            _spriteKeyframesView.FrameRateChanged -= OnFrameRateChanged;
            _spriteKeyframesView.TotalFramesChanged -= OnTotalFramesChanged;
            _spriteKeyframesView.SpritesSelected -= OnSpritesSelected;
        }

        void OnKeyframeDataChanged(AnimationSpriteInfo spriteInfo)
        {
            _spriteKeyframesView.OnDataChanged(spriteInfo: spriteInfo);
        }

        void OnFrameRateChanged(float newFrameRate)
        {
            _spriteKeyframeViewModel.UpdateFrameRate(newFrameRate: newFrameRate);
        }

        void OnTotalFramesChanged(int newTotalFrames)
        {
            _spriteKeyframeViewModel.UpdateTotalFrames(newTotalFrames: newTotalFrames);
        }

        void OnSpritesSelected(Sprite[] sprites)
        {
            _spriteKeyframeViewModel.SelectedSpritesChanged(sprites: sprites);
            _generationControlsView.ShowButton();
        }
    }
}
