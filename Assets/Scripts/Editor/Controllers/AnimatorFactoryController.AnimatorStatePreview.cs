using UnityEditor.Animations;
using UnityEngine;

namespace AnimatorFactory.Editor
{
    public partial class AnimatorFactoryController
    {
        void BindAnimatorStatesPreviewEvents()
        {
            _animatorStatesViewModel.StatesChanged += _animatorStatesView.OnStatesChanged;
            _animatorStatesViewModel.StatusChanged += _animatorStatesView.OnStatusChanged;

            _animatorStatesView.SelectedState += OnAnimatorStateSelected;
            _animatorStatesView.AddStateRequested += OnAddStateRequested;
        }

        void UnbindAnimatorStatesPreviewEvents()
        {
            _animatorStatesViewModel.StatesChanged -= _animatorStatesView.OnStatesChanged;
            _animatorStatesViewModel.StatusChanged -= _animatorStatesView.OnStatusChanged;
            
            _animatorStatesView.SelectedState -= OnAnimatorStateSelected;
            _animatorStatesView.AddStateRequested -= OnAddStateRequested;
        }

        void OnAnimatorStateSelected(AnimatorState state)
        {
            _spriteKeyframeViewModel.LoadSpriteKeyframes(state: state);
        }

        void OnAddStateRequested()
        {
            Debug.Log("Add State Clicked");
            // TODO: Implement add state functionality
        }
    }
}
