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
            
        }

        void OnGenerateButtonClicked()
        {
            _generationControlsView.ShowIsGeneratingDialogue();
            _generationControlsViewModel.GenerateAnimationClips(animationInfo: _spriteKeyframeViewModel.AnimationInfo);
        }
    }
}
