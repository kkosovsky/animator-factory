namespace AnimatorFactory.Editor
{
    public partial class AnimatorFactoryController
    {
        void BindSpriteKeyFramePreviewEvents()
        {
            _spriteKeyframeViewModel.StatusChanged += _spriteKeyframeView.OnStatusChanged;
            _spriteKeyframeViewModel.DataChanged += _spriteKeyframeView.OnDataChanged;
        }

        void UnbindSpriteKeyFramePreviewEvents()
        {
            _spriteKeyframeViewModel.StatusChanged -= _spriteKeyframeView.OnStatusChanged;
            _spriteKeyframeViewModel.DataChanged -= _spriteKeyframeView.OnDataChanged;
        }
    }
}
