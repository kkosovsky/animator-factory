using System;
using UnityEngine.UIElements;
using UnityEditor;

namespace AnimatorFactory.GenerationControls
{
    public class GenerationControlsView : VisualElement
    {
        public event Action GenerateButtonClicked
        {
            add => _generateButton.clicked += value;
            remove => _generateButton.clicked -= value;
        }

        Button _generateButton;

        public GenerationControlsView()
        {
            MakeButton();
        }

        public void ShowIsGeneratingDialogue()
        {
            EditorUtility.DisplayProgressBar(
                title: "Animator Factory",
                info: "Animation data generation in progress...",
                progress: 0.1f
            );
            _generateButton.SetEnabled(value: false);
        }

        public void UpdateGenerationProgressDialogue(float progress)
        {
            EditorUtility.DisplayProgressBar(
                title: "Animator Factory",
                info: "Animation data generation in progress...",
                progress: progress
            );
        }

        public void HideGeneratingDialogue()
        {
            EditorUtility.ClearProgressBar();
            _generateButton.SetEnabled(value: true);
        }

        void MakeButton()
        {
            _generateButton = new Button { text = "Generate" };
            Add(child: _generateButton);
        }
    }
}
