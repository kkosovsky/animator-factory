namespace AnimatorFactory.Editor
{
    public partial class AnimatorFactoryController
    {
        void BindGenerationControlsEvents()
        {
            _generationControlsView.GenerateButtonClicked += OnGenerateButtonClicked;
        }

        void UnbindGenerationControlsEvents()
        {
            _generationControlsView.GenerateButtonClicked -= OnGenerateButtonClicked;
        }

        void OnGenerateButtonClicked()
        {
            _generationControlsView.ShowIsGeneratingDialogue();
            _generationControlsViewModel.GenerateAnimationClips();
        }
    }
}
