using System;
using UnityEditor.Animations;
using UnityEngine;

namespace AnimatorFactory.SpriteKeyframePreview
{
    /// <summary>
    /// ViewModel for sprite keyframe analysis.
    /// Handles presentation logic and state management for sprite keyframes.
    /// </summary>
    public class SpriteKeyframeViewModel
    {
        /// <summary>
        /// Fired when sprite keyframe data changes.
        /// </summary>
        public event Action<AnimationSpriteInfo> DataChanged;
        
        /// <summary>
        /// Fired when status message changes.
        /// </summary>
        public event Action<string, bool> StatusChanged; // message, isError

        AnimationSpriteInfo _currentSpriteInfo;
        bool _hasData;

        /// <summary>
        /// Gets whether the ViewModel currently has sprite data.
        /// </summary>
        public bool HasData => _hasData;

        /// <summary>
        /// Gets the current sprite information.
        /// </summary>
        public AnimationSpriteInfo CurrentSpriteInfo => _currentSpriteInfo;

        /// <summary>
        /// Loads sprite keyframes from the given animator state.
        /// </summary>
        /// <param name="state">The animator state to analyze</param>
        public void LoadSpriteKeyframes(AnimatorState state)
        {
            try
            {
                if (state.motion is not AnimationClip clip)
                {
                    ShowStatus(message: "Selected state does not contain an Animation Clip.", isError: false);
                    ClearData();
                    return;
                }

                AnimationSpriteInfo spriteInfo = SpriteKeyframeService.ExtractSpriteKeyframes(clip: clip);
                
                if (spriteInfo.keyframes.Count == 0)
                {
                    ShowStatus(message: "No sprite keyframes found in this animation.", isError: false);
                    ClearData();
                    return;
                }

                _currentSpriteInfo = spriteInfo;
                _hasData = true;
                DataChanged?.Invoke(obj: spriteInfo);
            }
            catch (Exception ex)
            {
                ShowStatus(message: $"Error loading sprite keyframes: {ex.Message}", isError: true);
                ClearData();
            }
        }

        /// <summary>
        /// Clears all sprite data.
        /// </summary>
        public void Clear()
        {
            ClearData();
        }

        void ClearData()
        {
            _hasData = false;
            _currentSpriteInfo = default;
        }

        void ShowStatus(string message, bool isError)
        {
            StatusChanged?.Invoke(arg1: message, arg2: isError);
        }
    }
}
