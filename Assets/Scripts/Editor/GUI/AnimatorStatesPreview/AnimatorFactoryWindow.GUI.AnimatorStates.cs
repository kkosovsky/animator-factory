using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace AnimatorFactory
{
    public partial class AnimatorFactoryWindow
    {
         void DrawAnimatorUI(PrefabHierarchyListItem item)
        {
            if (!item.gameObject.HasComponent<Animator>())
            {
                Debug.LogWarning($"Item {item.name} doesn't have an animator!");
                return;
            }

            Animator animator = item.gameObject.GetComponent<Animator>();
            AnimatorController controller = animator.runtimeAnimatorController as AnimatorController;

            if (controller == null)
            {
                Debug.LogWarning($"Item {item.name} doesn't have an AnimatorController!");
                EditorGUILayout.HelpBox(message: "No Animator Controller found.", type: MessageType.Warning);
                return;
            }
            List<AnimatorState> allStates = GetAllAnimatorStates(controller: controller);
            
            if (allStates.Count == 0)
            {
                Debug.LogWarning($"{item.name}'s AnimatorController doesn't have any states!");
                EditorGUILayout.HelpBox(message: "No states found in the Animator Controller.", type: MessageType.Info);
                return;
            }

            string message = $"Animator States ({allStates.Count})";
            EditorGUILayout.LabelField(message, EditorStyles.boldLabel);
            using (EditorGUILayout.ScrollViewScope _ = new(Vector2.zero, GUILayout.Height(60)))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    foreach (AnimatorState state in allStates)
                    {
                        if (GUILayout.Button(state.name, GUILayout.Width(100), GUILayout.Height(40)))
                        {
                            OnAnimatorStateSelected(state, animator);
                        }
                    }
                }
            }
            Debug.Log(message);
            // TODO: Get all states of the animator. Show them as a horizontal, scrollable(if needed) list of buttons.
            
        }
        
         void OnAnimatorStateSelected(AnimatorState state, Animator animator)
        {
            Debug.Log($"Selected animator state: {state.name}");
            
            // Here you can add logic for what happens when a state is selected
            // For example:
            // - Show state properties
            // - Preview the animation
            // - Modify state settings
            // etc.
        }

         List<AnimatorState> GetAllAnimatorStates(AnimatorController controller)
        {
            var allStates = new List<AnimatorState>();

            foreach (AnimatorControllerLayer layer in controller.layers)
            {
                CollectStatesRecursive(stateMachine: layer.stateMachine, states: allStates);
            }

            return allStates;
        }

         void CollectStatesRecursive(AnimatorStateMachine stateMachine, List<AnimatorState> states)
        {
            states.AddRange(collection: stateMachine.states.Select(selector: childState => childState.state));

            foreach (ChildAnimatorStateMachine childStateMachine in stateMachine.stateMachines)
            {
                CollectStatesRecursive(stateMachine: childStateMachine.stateMachine, states: states);
            }
        }
    }
}
