using System;
using System.Collections.Generic;
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
        List<SpriteKeyframeData> _originalKeyframes;
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

                // Cache the original keyframes for later restoration
                _originalKeyframes = new List<SpriteKeyframeData>(spriteInfo.keyframes);
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
        /// Updates the frame rate and refreshes the display.
        /// </summary>
        /// <param name="newFrameRate">The new frame rate value</param>
        public void UpdateFrameRate(float newFrameRate)
        {
            if (!_hasData || newFrameRate <= 0) return;

            // Create a modified version of the current sprite info with new frame rate
            AnimationSpriteInfo modifiedInfo = CreateModifiedSpriteInfo(
                original: _currentSpriteInfo,
                newFrameRate: newFrameRate,
                newTotalFrames: _currentSpriteInfo.totalFrames,
                newKeyframes: _currentSpriteInfo.keyframes
            );

            _currentSpriteInfo = modifiedInfo;
            DataChanged?.Invoke(obj: modifiedInfo);
        }

        /// <summary>
        /// Updates the total frames count and adjusts the keyframes list accordingly.
        /// </summary>
        /// <param name="newTotalFrames">The new total frames value</param>
        public void UpdateTotalFrames(int newTotalFrames)
        {
            if (!_hasData || newTotalFrames <= 0 || _originalKeyframes == null) return;

            // Adjust the keyframes list based on the new total frames count using original keyframes
            List<SpriteKeyframeData> adjustedKeyframes = AdjustKeyframesForTotalFrames(
                _originalKeyframes, 
                newTotalFrames, 
                _currentSpriteInfo.frameRate
            );

            // Create a modified version of the current sprite info with new total frames and adjusted keyframes
            AnimationSpriteInfo modifiedInfo = CreateModifiedSpriteInfo(
                original: _currentSpriteInfo,
                newFrameRate: _currentSpriteInfo.frameRate,
                newTotalFrames: newTotalFrames,
                newKeyframes: adjustedKeyframes
            );

            _currentSpriteInfo = modifiedInfo;
            DataChanged?.Invoke(obj: modifiedInfo);
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
            _originalKeyframes = null;
        }

        /// <summary>
        /// Creates a modified version of the sprite info with new frame rate, total frames, and keyframes.
        /// </summary>
        AnimationSpriteInfo CreateModifiedSpriteInfo(
            AnimationSpriteInfo original,
            float newFrameRate,
            int newTotalFrames,
            List<SpriteKeyframeData> newKeyframes
        )
        {
            return new AnimationSpriteInfo(
                animationName: original.animationName,
                duration: original.duration,
                frameRate: newFrameRate,
                totalFrames: newTotalFrames,
                keyframes: newKeyframes
            );
        }

        /// <summary>
        /// Adjusts the keyframes list to match the new total frames count.
        /// Uses the original cached keyframes to preserve sprite data when increasing frames.
        /// </summary>
        List<SpriteKeyframeData> AdjustKeyframesForTotalFrames(
            List<SpriteKeyframeData> originalKeyframes, 
            int newTotalFrames, 
            float frameRate
        )
        {
            List<SpriteKeyframeData> adjustedKeyframes = new List<SpriteKeyframeData>();

            int framesToCopy = Mathf.Min(originalKeyframes.Count, newTotalFrames);
            for (int i = 0; i < framesToCopy; i++)
            {
                adjustedKeyframes.Add(originalKeyframes[i]);
            }

            if (newTotalFrames > originalKeyframes.Count)
            {
                for (int i = originalKeyframes.Count; i < newTotalFrames; i++)
                {
                    float time = i / frameRate;
                    adjustedKeyframes.Add(new SpriteKeyframeData(index: i, time, null));
                }
            }

            return adjustedKeyframes;
        }

        void ShowStatus(string message, bool isError)
        {
            StatusChanged?.Invoke(arg1: message, arg2: isError);
        }
    }
}
