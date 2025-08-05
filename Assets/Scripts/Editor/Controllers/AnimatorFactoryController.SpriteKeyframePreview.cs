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
            _spriteKeyframesView.AnimationNameChanged += OnAnimationNameChanged;
            _spriteKeyframesView.DestinationFolderChanged += OnDestinationFolderChanged;
        }

        void UnbindSpriteKeyFramePreviewEvents()
        {
            _spriteKeyframeViewModel.StatusChanged -= _spriteKeyframesView.OnStatusChanged;
            _spriteKeyframeViewModel.DataChanged -= OnKeyframeDataChanged;
            
            _spriteKeyframesView.FrameRateChanged -= OnFrameRateChanged;
            _spriteKeyframesView.TotalFramesChanged -= OnTotalFramesChanged;
            _spriteKeyframesView.SpritesSelected -= OnSpritesSelected;
            _spriteKeyframesView.AnimationNameChanged -= OnAnimationNameChanged;
            _spriteKeyframesView.DestinationFolderChanged -= OnDestinationFolderChanged;
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

        void OnAnimationNameChanged(string newAnimationName)
        {
            _spriteKeyframeViewModel.UpdateAnimationName(newAnimationName);
        }

        void OnDestinationFolderChanged(string newDestinationFolder)
        {
            Debug.Log($"Destination folder changed to: {newDestinationFolder}");
            // TODO: Update destination folder in the view model or data structure
            // _spriteKeyframeViewModel.UpdateDestinationFolder(newDestinationFolder);
        }
    }
}
