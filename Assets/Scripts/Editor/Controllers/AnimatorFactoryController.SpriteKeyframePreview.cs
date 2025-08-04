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
        }

        void UnbindSpriteKeyFramePreviewEvents()
        {
            _spriteKeyframeViewModel.StatusChanged -= _spriteKeyframeView.OnStatusChanged;
            _spriteKeyframeViewModel.DataChanged -= _spriteKeyframeView.OnDataChanged;
            
            _spriteKeyframeView.FrameRateChanged -= OnFrameRateChanged;
            _spriteKeyframeView.TotalFramesChanged -= OnTotalFramesChanged;
        }

        void OnFrameRateChanged(float newFrameRate)
        {
            _spriteKeyframeViewModel.UpdateFrameRate(newFrameRate);
        }

        void OnTotalFramesChanged(int newTotalFrames)
        {
            _spriteKeyframeViewModel.UpdateTotalFrames(newTotalFrames);
        }
    }
}
