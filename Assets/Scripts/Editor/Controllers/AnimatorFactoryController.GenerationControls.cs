using UnityEngine;

namespace AnimatorFactory.Editor
{
    public partial class AnimatorFactoryController
    {
        void BindGenerationControlsEvents()
        {
            _generationControlsView.GenerateButtonClicked += OnGenerateButtonClicked;
            _generationControlsViewModel.StartedGeneration +=
                _generationControlsView.ShowIsGeneratingDialogue;
            _generationControlsViewModel.UpdatedGenerationProgress +=
                _generationControlsView.UpdateGenerationProgressDialogue;
            _generationControlsViewModel.FinishedGeneration +=
                _generationControlsView.HideGeneratingDialogue;
            _generationControlsViewModel.AnimationClipGenerated += OnAnimationClipGenerated;
        }

        void UnbindGenerationControlsEvents()
        {
            _generationControlsView.GenerateButtonClicked -= OnGenerateButtonClicked;
            _generationControlsViewModel.StartedGeneration -=
                _generationControlsView.ShowIsGeneratingDialogue;
            _generationControlsViewModel.UpdatedGenerationProgress -=
                _generationControlsView.UpdateGenerationProgressDialogue;
            _generationControlsViewModel.FinishedGeneration -=
                _generationControlsView.HideGeneratingDialogue;
            _generationControlsViewModel.AnimationClipGenerated -= OnAnimationClipGenerated;
        }

        void OnGenerateButtonClicked()
        {
            _generationControlsView.ShowIsGeneratingDialogue();
            _generationControlsViewModel.GenerateAnimationClips(animationInfo: _spriteKeyframeViewModel.AnimationInfo);
        }

        void OnAnimationClipGenerated(AnimationClip animationClip, string stateName)
        {
            Debug.Log(
                message:
                $"OnAnimationClipGenerated called - Animation clip: {animationClip?.name}, State name: {stateName}"
            );

            if (animationClip == null)
            {
                Debug.LogError(message: "Animation clip is null in OnAnimationClipGenerated");
                return;
            }

            if (string.IsNullOrEmpty(value: stateName))
            {
                Debug.LogError(message: "State name is null or empty in OnAnimationClipGenerated");
                return;
            }

            Debug.Log(
                message: $"About to call CreateNewStateWithClip stateName: '{stateName}', clip: '{animationClip.name}'"
            );

            _animatorStatesViewModel.CreateNewStateWithClip(stateName: stateName, animationClip: animationClip);
        }
    }
}
