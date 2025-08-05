using System;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UIElements;

namespace AnimatorFactory.AnimatorStatePreview
{
    public class AnimatorStatesView : VisualElement
    {
        public event Action<AnimatorState> SelectedState;

        VisualElement _statesContainer;
        ScrollView _statesScrollView;
        HelpBox _helpBox;
        Label _titleLabel;

        public AnimatorStatesView() => CreateUI();

        /// <summary>
        /// Hides the view and clears its content.
        /// </summary>
        public void Hide()
        {
            style.display = DisplayStyle.None;
            ClearStates();
        }

        public void OnStatesChanged(List<AnimatorState> states)
        {
            HideStatus();
            DisplayStates(states: states);
            style.display = DisplayStyle.Flex;
        }

        public void OnStatusChanged(string message, bool isError)
        {
            ShowStatus(message: message, type: isError ? HelpBoxMessageType.Error : HelpBoxMessageType.Warning);
            style.display = DisplayStyle.Flex;
        }

        void CreateUI()
        {
            style.paddingTop = 10;

            _titleLabel = new Label(text: "Animator States")
            {
                style =
                {
                    unityFontStyleAndWeight = FontStyle.Bold,
                    marginBottom = 5
                }
            };
            Add(child: _titleLabel);

            _helpBox = new HelpBox(text: "", messageType: HelpBoxMessageType.Warning)
            {
                style = { display = DisplayStyle.None }
            };
            Add(child: _helpBox);

            _statesScrollView = new ScrollView(scrollViewMode: ScrollViewMode.Horizontal)
            {
                style =
                {
                    height = 60,
                    marginTop = 5
                }
            };

            _statesContainer = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    flexWrap = Wrap.NoWrap
                }
            };
            _statesScrollView.Add(child: _statesContainer);
            Add(child: _statesScrollView);
        }

        void DisplayStates(List<AnimatorState> states)
        {
            ClearStates();
            _titleLabel.text = $"Animator States (Count: {states.Count})";

            foreach (AnimatorState state in states)
            {
                Button stateButton = new(clickEvent: () => OnStateButtonClicked(state: state))
                {
                    text = state.name,
                    style =
                    {
                        width = 100,
                        height = 40,
                        marginRight = 5
                    }
                };
                _statesContainer.Add(child: stateButton);
            }
        }

        void ShowStatus(string message, HelpBoxMessageType type)
        {
            _helpBox.text = message;
            _helpBox.messageType = type;
            _helpBox.style.display = DisplayStyle.Flex;
            _statesScrollView.style.display = DisplayStyle.None;
        }

        void HideStatus()
        {
            _helpBox.style.display = DisplayStyle.None;
            _statesScrollView.style.display = DisplayStyle.Flex;
        }

        void ClearStates()
        {
            _statesContainer.Clear();
        }

        void OnStateButtonClicked(AnimatorState state)
        {
            SelectedState?.Invoke(state);
        }
    }
}
