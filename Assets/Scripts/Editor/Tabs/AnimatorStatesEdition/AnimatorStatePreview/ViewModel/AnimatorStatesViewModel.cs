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

                List<AnimatorState> allStates = AnimatorStateService.GetAllAnimatorStates(controller: controller);

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
            if (_currentAnimatorController == null)
            {
                Debug.LogError(message: "No animator controller available to create new state");
                ShowStatus(message: "No animator controller available to create new state", isError: true);
                return;
            }

            AnimatorState newState = AnimatorStateService.CreateNewStateWithClip(
                animatorController: _currentAnimatorController,
                stateName: stateName,
                animationClip: animationClip
            );

            if (newState != null)
            {
                Debug.Log(message: $"Successfully created new animator state: {newState.name}");
                LoadAnimatorStatesFromCurrentController();
            }
            else
            {
                Debug.LogError(message: "Failed to create new animator state");
                ShowStatus(message: "Failed to create new animator state", isError: true);
            }
        }

        /// <summary>
        /// Reloads animator states from the current controller.
        /// </summary>
        void LoadAnimatorStatesFromCurrentController()
        {
            if (_currentAnimatorController == null)
            {
                Debug.LogWarning(message: "Current animator controller is null, cannot reload states");
                return;
            }

            List<AnimatorState> allStates =
                AnimatorStateService.GetAllAnimatorStates(controller: _currentAnimatorController);

            _currentStates = allStates;

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
