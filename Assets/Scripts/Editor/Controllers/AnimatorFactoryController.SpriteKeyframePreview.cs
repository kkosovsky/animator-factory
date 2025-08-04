using UnityEngine;

namespace AnimatorFactory.Editor
{
    public partial class AnimatorFactoryController
    {
        void BindSpriteKeyFramePreviewEvents()
        {
            _spriteKeyframeViewModel.StatusChanged += _spriteKeyframesView.OnStatusChanged;
            _spriteKeyframeViewModel.DataChanged += _spriteKeyframesView.OnDataChanged;
            
            _spriteKeyframesView.FrameRateChanged += OnFrameRateChanged;
            _spriteKeyframesView.TotalFramesChanged += OnTotalFramesChanged;
            _spriteKeyframesView.SpritesSelected += OnSpritesSelected;
        }

        void UnbindSpriteKeyFramePreviewEvents()
        {
            _spriteKeyframeViewModel.StatusChanged -= _spriteKeyframesView.OnStatusChanged;
            _spriteKeyframeViewModel.DataChanged -= _spriteKeyframesView.OnDataChanged;
            
            _spriteKeyframesView.FrameRateChanged -= OnFrameRateChanged;
            _spriteKeyframesView.TotalFramesChanged -= OnTotalFramesChanged;
            _spriteKeyframesView.SpritesSelected -= OnSpritesSelected;
        }

        void OnFrameRateChanged(float newFrameRate)
        {
            _spriteKeyframeViewModel.UpdateFrameRate(newFrameRate);
        }

        void OnTotalFramesChanged(int newTotalFrames)
        {
            _spriteKeyframeViewModel.UpdateTotalFrames(newTotalFrames);
        }

        void OnSpritesSelected(Sprite[] sprites)
        {
            // _spriteKeyframeViewModel.ReplaceKeyframesWithSprites(sprites);
        }
    }
}
