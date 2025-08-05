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
        public static AnimatorState CreateNewStateWithClip(
            AnimatorController animatorController,
            string stateName,
            AnimationClip animationClip
        )
        {
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
                AnimatorStateMachine rootStateMachine = animatorController.layers[0].stateMachine;
                AnimatorState newState = rootStateMachine.AddState(name: stateName);
                newState.motion = animationClip;
                EditorUtility.SetDirty(target: animatorController);
                AssetDatabase.SaveAssets();
                return newState;
            }
            catch (System.Exception ex)
            {
                Debug.LogError(message: $"Error creating new animator state: {ex.Message}");
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
