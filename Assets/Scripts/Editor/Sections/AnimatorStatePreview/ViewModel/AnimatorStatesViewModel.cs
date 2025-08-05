using System;
using System.Collections.Generic;
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
                // TODO: Add current animator data

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
        /// Clears all animator state data.
        /// </summary>
        public void Clear()
        {
            ClearStates();
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
