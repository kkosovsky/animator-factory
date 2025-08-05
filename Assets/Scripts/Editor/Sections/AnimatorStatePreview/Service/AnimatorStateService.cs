using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace AnimatorFactory.AnimatorStatePreview
{
    /// <summary>
    /// Service responsible for animator state operations.
    /// Contains stateless logic for creating and managing animator states.
    /// </summary>
    public static class AnimatorStateService
    {
        /// <summary>
        /// Creates a new state in the animator controller and attaches the animation clip.
        /// </summary>
        /// <param name="animatorController">The animator controller to add the state to</param>
        /// <param name="stateName">The name of the new state</param>
        /// <param name="animationClip">The animation clip to attach</param>
        /// <returns>The created animator state, or null if creation failed</returns>
        public static AnimatorState CreateNewStateWithClip(AnimatorController animatorController, string stateName, AnimationClip animationClip)
        {
            Debug.Log(message: $"AnimatorStateService.CreateNewStateWithClip called with stateName: '{stateName}', animationClip: '{animationClip?.name}'");
            
            if (animatorController == null)
            {
                Debug.LogError(message: "Animator controller is null, cannot create new state");
                return null;
            }

            if (animationClip == null)
            {
                Debug.LogError(message: "Animation clip is null, cannot create state");
                return null;
            }

            if (string.IsNullOrEmpty(stateName))
            {
                Debug.LogError(message: "State name is null or empty, cannot create state");
                return null;
            }

            try
            {
                Debug.Log(message: $"Current animator controller: {animatorController.name}");
                Debug.Log(message: $"Number of layers: {animatorController.layers.Length}");
                
                // Create new state in the first layer's state machine
                AnimatorStateMachine rootStateMachine = animatorController.layers[0].stateMachine;
                Debug.Log(message: $"Root state machine: {rootStateMachine.name}");
                
                AnimatorState newState = rootStateMachine.AddState(name: stateName);
                Debug.Log(message: $"Added new state: {newState.name}");
                
                // Attach the animation clip to the state
                newState.motion = animationClip;
                Debug.Log(message: $"Attached animation clip '{animationClip.name}' to state '{newState.name}'");
                
                // Mark the animator controller as dirty so Unity saves the changes
                EditorUtility.SetDirty(target: animatorController);
                AssetDatabase.SaveAssets();
                
                Debug.Log(message: $"Created new animator state '{stateName}' with animation clip '{animationClip.name}'");
                
                return newState;
            }
            catch (System.Exception ex)
            {
                Debug.LogError(message: $"Error creating new animator state: {ex.Message}");
                Debug.LogException(exception: ex);
                return null;
            }
        }

        /// <summary>
        /// Gets all animator states from an animator controller, including nested state machines.
        /// This is a wrapper around StateAnalysisService for consistency.
        /// </summary>
        /// <param name="controller">The animator controller to analyze</param>
        /// <returns>List of all animator states</returns>
        public static List<AnimatorState> GetAllAnimatorStates(AnimatorController controller)
        {
            return StateAnalysisService.GetAllAnimatorStates(controller);
        }
    }
}