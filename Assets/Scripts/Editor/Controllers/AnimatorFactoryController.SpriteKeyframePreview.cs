using UnityEngine;

namespace AnimatorFactory.Editor
{
    public partial class AnimatorFactoryController
    {
        void BindSpriteKeyFramePreviewEvents()
        {
            _spriteKeyframeViewModel.StatusChanged += _spriteKeyframeView.OnStatusChanged;
            _spriteKeyframeViewModel.DataChanged += _spriteKeyframeView.OnDataChanged;
            
            _spriteKeyframeView.FrameRateChanged += OnFrameRateChanged;
            _spriteKeyframeView.TotalFramesChanged += OnTotalFramesChanged;
            _spriteKeyframeView.SpritesSelected += OnSpritesSelected;
        }

        void UnbindSpriteKeyFramePreviewEvents()
        {
            _spriteKeyframeViewModel.StatusChanged -= _spriteKeyframeView.OnStatusChanged;
            _spriteKeyframeViewModel.DataChanged -= _spriteKeyframeView.OnDataChanged;
            
            _spriteKeyframeView.FrameRateChanged -= OnFrameRateChanged;
            _spriteKeyframeView.TotalFramesChanged -= OnTotalFramesChanged;
            _spriteKeyframeView.SpritesSelected -= OnSpritesSelected;
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
