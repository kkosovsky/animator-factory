using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UIElements;

namespace AnimatorFactory
{
    public class AnimatorStatesView : VisualElement
    {
        readonly VisualElement _statesContainer;
        readonly ScrollView _statesScrollView;
        readonly HelpBox _helpBox;
        readonly Label _titleLabel;

        public AnimatorStatesView()
        {
            style.paddingTop = 10;
            _titleLabel = new Label(text: "Animator States") 
            { 
                style = { 
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
                style = { 
                    height = 60,
                    marginTop = 5
                }
            };
            
            _statesContainer = new VisualElement
            {
                style = { 
                    flexDirection = FlexDirection.Row,
                    flexWrap = Wrap.NoWrap
                }
            };
            _statesScrollView.Add(child: _statesContainer);
            Add(child: _statesScrollView);
        }

        public void ShowAnimatorStates(PrefabHierarchyListItem item)
        {
            ClearStates();

            if (!item.gameObject.HasComponent<Animator>())
            {
                ShowMessage(message: "No Animator component found.", type: HelpBoxMessageType.Warning);
                return;
            }

            Animator animator = item.gameObject.GetComponent<Animator>();
            AnimatorController controller = animator.runtimeAnimatorController as AnimatorController;

            if (controller == null)
            {
                ShowMessage(message: "No Animator Controller found.", type: HelpBoxMessageType.Warning);
                return;
            }

            List<AnimatorState> allStates = GetAllAnimatorStates(controller: controller);
            
            if (allStates.Count == 0)
            {
                ShowMessage(message: "No states found in the Animator Controller.", type: HelpBoxMessageType.Info);
                return;
            }

            HideMessage();
            _titleLabel.text = $"Animator States ({allStates.Count})";

            foreach (AnimatorState state in allStates)
            {
                Button stateButton = new Button(clickEvent: () => OnAnimatorStateSelected(state: state, animator: animator))
                {
                    text = state.name,
                    style = { 
                        width = 100, 
                        height = 40,
                        marginRight = 5
                    }
                };
                _statesContainer.Add(child: stateButton);
            }

            style.display = DisplayStyle.Flex;
        }

        public void Hide()
        {
            style.display = DisplayStyle.None;
            ClearStates();
        }

        void ShowMessage(string message, HelpBoxMessageType type)
        {
            _helpBox.text = message;
            _helpBox.messageType = type;
            _helpBox.style.display = DisplayStyle.Flex;
            _statesScrollView.style.display = DisplayStyle.None;
        }

        void HideMessage()
        {
            _helpBox.style.display = DisplayStyle.None;
            _statesScrollView.style.display = DisplayStyle.Flex;
        }

        void ClearStates()
        {
            _statesContainer.Clear();
        }

        void OnAnimatorStateSelected(AnimatorState state, Animator animator)
        {
            
        }

        static List<AnimatorState> GetAllAnimatorStates(AnimatorController controller)
        {
            var allStates = new List<AnimatorState>();
            foreach (AnimatorControllerLayer layer in controller.layers)
            {
                CollectStatesRecursive(stateMachine: layer.stateMachine, states: allStates);
            }
            return allStates;
        }

        static void CollectStatesRecursive(AnimatorStateMachine stateMachine, List<AnimatorState> states)
        {
            states.AddRange(collection: stateMachine.states.Select(selector: childState => childState.state));
            foreach (ChildAnimatorStateMachine childStateMachine in stateMachine.stateMachines)
            {
                CollectStatesRecursive(stateMachine: childStateMachine.stateMachine, states: states);
            }
        }
    }
} 
