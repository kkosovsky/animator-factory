using System;
using UnityEngine.UIElements;

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
            // TODO: Show dialogue saying "Animation data generation in progres..."
        }

        void MakeButton()
        {
            _generateButton = new Button { text = "Generate" };
            Add(child: _generateButton);
        }
    }
}
