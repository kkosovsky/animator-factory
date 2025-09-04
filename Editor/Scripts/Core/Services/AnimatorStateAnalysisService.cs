using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;

namespace AnimatorFactory
{
    /// <summary>
    /// Service responsible for analyzing animator states and controllers.
    /// Contains stateless logic for state analysis.
    /// </summary>
    public static class AnimatorStateAnalysisService
    {
        /// <summary>
        /// Gets all animator states from an animator controller, including nested state machines.
        /// </summary>
        /// <param name="controller">The animator controller to analyze</param>
        /// <returns>List of all animator states</returns>
        public static List<AnimatorState> GetAllAnimatorStates(RuntimeAnimatorController controller)
        {
            var allStates = new List<AnimatorState>();
            AnimatorControllerLayer[] layers = Array.Empty<AnimatorControllerLayer>();
            
            if (controller is AnimatorController animatorController)
            {
                layers = animatorController.layers;
            }
            
            if (controller is AnimatorOverrideController overrideController)
            {
                layers = (overrideController.runtimeAnimatorController as AnimatorController).layers;
            }
            
            foreach (AnimatorControllerLayer layer in layers)
            {
                CollectStatesRecursive(stateMachine: layer.stateMachine, states: allStates);
            }

            return allStates;
        }

        public static void GetAllObjectsWithAnimator(GameObject gameObject, in List<GameObject> objects)
        {
            if (gameObject.HasComponent<Animator>())
            {
                objects.Add(item: gameObject);
            }

            Transform transform = gameObject.transform;
            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                GetAllObjectsWithAnimator(gameObject: transform.GetChild(index: i).gameObject, objects: objects);
            }
        }

        /// <summary>
        /// Recursively collects all states from a state machine and its sub-state machines.
        /// </summary>
        /// <param name="stateMachine">The state machine to analyze</param>
        /// <param name="states">The list to add states to</param>
        static void CollectStatesRecursive(AnimatorStateMachine stateMachine, List<AnimatorState> states)
        {
            // Add all states from current state machine
            states.AddRange(collection: stateMachine.states.Select(selector: childState => childState.state));

            // Recursively collect from sub-state machines
            foreach (ChildAnimatorStateMachine childStateMachine in stateMachine.stateMachines)
            {
                CollectStatesRecursive(stateMachine: childStateMachine.stateMachine, states: states);
            }
        }
    }
}
