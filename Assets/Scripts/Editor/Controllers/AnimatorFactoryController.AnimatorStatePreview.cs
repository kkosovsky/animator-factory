using UnityEditor.Animations;

namespace AnimatorFactory.Editor
{
    public partial class AnimatorFactoryController
    {
        void BindAnimatorStatesPreviewEvents()
        {
            _animatorStatesViewModel.StatesChanged += _animatorStatesView.OnStatesChanged;
            _animatorStatesViewModel.StatusChanged += _animatorStatesView.OnStatusChanged;

            _animatorStatesView.SelectedState += OnAnimatorStateSelected;
        }

        void UnbindAnimatorStatesPreviewEvents()
        {
            _animatorStatesViewModel.StatesChanged -= _animatorStatesView.OnStatesChanged;
            _animatorStatesViewModel.StatusChanged -= _animatorStatesView.OnStatusChanged;
            
            _animatorStatesView.SelectedState -= OnAnimatorStateSelected;
        }

        void OnAnimatorStateSelected(AnimatorState state)
        {
            _spriteKeyframeViewModel.LoadSpriteKeyframes(state: state);
        }
    }
}
