using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace AnimatorFactory.AnimatorStatePreview
{
    /// <summary>
    /// ViewModel for animator state preview.
    /// Handles presentation logic and state management for animator states.
    /// </summary>
    public class AnimatorStatesViewModel
    {
        /// <summary>
        /// Fired when the list of available states changes.
        /// </summary>
        public event Action<List<AnimatorState>> StatesChanged;

        /// <summary>
        /// Fired when status message changes.
        /// </summary>
        public event Action<string, bool> StatusChanged; // message, isError

        List<AnimatorState> _currentStates = new();
        Animator _currentAnimator;
        AnimatorController _currentAnimatorController;
        
        /// <summary>
        /// Gets the currently selected animator.
        /// </summary>
        public Animator CurrentAnimator => _currentAnimator;
        
        /// <summary>
        /// Gets the currently selected animator controller.
        /// </summary>
        public AnimatorController CurrentAnimatorController => _currentAnimatorController;

        /// <summary>
        /// Loads animator states from the given hierarchy item.
        /// </summary>
        /// <param name="item">The hierarchy item to analyze</param>
        public void LoadAnimatorStates(PrefabHierarchyListItem item)
        {
            try
            {
                if (!item.gameObject.HasComponent<Animator>())
                {
                    ShowStatus(message: "No Animator component found.", isError: true);
                    ClearStates();
                    return;
                }

                Animator animator = item.gameObject.GetComponent<Animator>();
                AnimatorController controller = animator.runtimeAnimatorController as AnimatorController;
                
                // Store current animator and controller
                _currentAnimator = animator;
                _currentAnimatorController = controller;

                if (controller == null)
                {
                    ShowStatus(message: "No Animator Controller found.", isError: true);
                    ClearStates();
                    return;
                }

                List<AnimatorState> allStates = StateAnalysisService.GetAllAnimatorStates(controller: controller);

                if (allStates.Count == 0)
                {
                    ShowStatus(message: "No states found in the Animator Controller.", isError: false);
                    ClearStates();
                    return;
                }

                _currentStates = allStates;
                StatesChanged?.Invoke(obj: allStates);
            }
            catch (Exception ex)
            {
                ShowStatus(message: $"Error loading animator states: {ex.Message}", isError: true);
                ClearStates();
            }
        }

        /// <summary>
        /// Creates a new state in the current animator controller and attaches the animation clip.
        /// </summary>
        /// <param name="stateName">The name of the new state</param>
        /// <param name="animationClip">The animation clip to attach</param>
        public void CreateNewStateWithClip(string stateName, AnimationClip animationClip)
        {
            Debug.Log(message: $"CreateNewStateWithClip called with stateName: '{stateName}', animationClip: '{animationClip?.name}'");
            
            if (_currentAnimatorController == null)
            {
                Debug.LogError(message: "No animator controller available to create new state");
                return;
            }

            if (animationClip == null)
            {
                Debug.LogError(message: "Animation clip is null, cannot create state");
                return;
            }

            try
            {
                Debug.Log(message: $"Current animator controller: {_currentAnimatorController.name}");
                Debug.Log(message: $"Number of layers: {_currentAnimatorController.layers.Length}");
                
                // Create new state in the first layer's state machine
                AnimatorStateMachine rootStateMachine = _currentAnimatorController.layers[0].stateMachine;
                Debug.Log(message: $"Root state machine: {rootStateMachine.name}");
                
                AnimatorState newState = rootStateMachine.AddState(name: stateName);
                Debug.Log(message: $"Added new state: {newState.name}");
                
                // Attach the animation clip to the state
                newState.motion = animationClip;
                Debug.Log(message: $"Attached animation clip '{animationClip.name}' to state '{newState.name}'");
                
                // Mark the animator controller as dirty so Unity saves the changes
                EditorUtility.SetDirty(target: _currentAnimatorController);
                AssetDatabase.SaveAssets();
                
                Debug.Log(message: $"Created new animator state '{stateName}' with animation clip '{animationClip.name}'");
                
                // Refresh the states list
                LoadAnimatorStatesFromCurrentController();
            }
            catch (System.Exception ex)
            {
                Debug.LogError(message: $"Error creating new animator state: {ex.Message}");
                Debug.LogException(exception: ex);
            }
        }

        /// <summary>
        /// Reloads animator states from the current controller.
        /// </summary>
        void LoadAnimatorStatesFromCurrentController()
        {
            Debug.Log(message: "LoadAnimatorStatesFromCurrentController called");
            
            if (_currentAnimatorController == null) 
            {
                Debug.LogWarning(message: "Current animator controller is null, cannot reload states");
                return;
            }

            Debug.Log(message: $"Reloading states from controller: {_currentAnimatorController.name}");
            List<AnimatorState> allStates = StateAnalysisService.GetAllAnimatorStates(controller: _currentAnimatorController);
            Debug.Log(message: $"Found {allStates.Count} states after reload");
            
            _currentStates = allStates;
            
            Debug.Log(message: $"StatesChanged event has {StatesChanged?.GetInvocationList()?.Length ?? 0} subscribers");
            StatesChanged?.Invoke(obj: allStates);
            Debug.Log(message: "StatesChanged event fired");
        }

        /// <summary>
        /// Clears all animator state data.
        /// </summary>
        public void Clear()
        {
            ClearStates();
            _currentAnimator = null;
            _currentAnimatorController = null;
        }

        void ClearStates()
        {
            _currentStates.Clear();
        }

        void ShowStatus(string message, bool isError)
        {
            StatusChanged?.Invoke(arg1: message, arg2: isError);
        }
    }
}
