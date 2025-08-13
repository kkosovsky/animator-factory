using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public event Action<SpriteAnimationInfo> DataChanged;

        /// <summary>
        /// Fired when status message changes.
        /// </summary>
        public event Action<string, bool> StatusChanged; // message, isError

        public SpriteAnimationInfo SpriteAnimationInfo => _currentSpriteAnimationInfo;

        SpriteAnimationInfo _currentSpriteAnimationInfo;
        List<SpriteAnimationKeyframe> _originalKeyframes;
        bool _hasData;

        /// <summary>
        /// Creates a new empty animation state for editing.
        /// </summary>
        /// <param name="stateName">The name of the new state</param>
        public void CreateNewAnimationState(string stateName)
        {
            float defaultFrameRate = 12.0f;
            int defaultTotalFrames = 12;

            List<SpriteAnimationKeyframe> defaultKeyframes = new List<SpriteAnimationKeyframe>();
            for (int i = 0; i < defaultTotalFrames; i++)
            {
                float time = i / defaultFrameRate;
                defaultKeyframes.Add(item: new SpriteAnimationKeyframe(index: i, time: time, sprite: null));
            }

            _currentSpriteAnimationInfo = new SpriteAnimationInfo(
                animationName: stateName,
                duration: defaultTotalFrames / defaultFrameRate,
                frameRate: defaultFrameRate,
                totalFrames: defaultTotalFrames,
                keyframes: defaultKeyframes,
                destinationFolderPath: $"Assets{Path.DirectorySeparatorChar}"
            );

            _originalKeyframes = new List<SpriteAnimationKeyframe>(collection: defaultKeyframes);
            _hasData = true;

            DataChanged?.Invoke(obj: _currentSpriteAnimationInfo);
        }

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

                SpriteAnimationInfo info = SpriteInfoExtractionService.ExtractSpriteKeyframes(clip: clip);

                if (info.keyframes.Count == 0)
                {
                    ShowStatus(message: "No sprite keyframes found in this animation.", isError: false);
                    ClearData();
                    return;
                }

                _originalKeyframes = new List<SpriteAnimationKeyframe>(collection: info.keyframes);
                _currentSpriteAnimationInfo = info;
                _hasData = true;
                DataChanged?.Invoke(obj: info);
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

            SpriteAnimationInfo modifiedInfo = CreateModifiedSpriteInfo(
                original: _currentSpriteAnimationInfo,
                newFrameRate: newFrameRate,
                newTotalFrames: _currentSpriteAnimationInfo.totalFrames,
                newKeyframes: _currentSpriteAnimationInfo.keyframes
            );

            _currentSpriteAnimationInfo = modifiedInfo;
            DataChanged?.Invoke(obj: modifiedInfo);
        }

        /// <summary>
        /// Updates the total frames count and adjusts the keyframes list accordingly.
        /// </summary>
        /// <param name="newTotalFrames">The new total frames value</param>
        public void UpdateTotalFrames(int newTotalFrames)
        {
            if (!_hasData || newTotalFrames <= 0 || _originalKeyframes == null) return;

            List<SpriteAnimationKeyframe> adjustedKeyframes = AdjustKeyframesForTotalFrames(
                originalKeyframes: _originalKeyframes,
                newTotalFrames: newTotalFrames,
                frameRate: _currentSpriteAnimationInfo.frameRate
            );

            SpriteAnimationInfo modifiedInfo = CreateModifiedSpriteInfo(
                original: _currentSpriteAnimationInfo,
                newFrameRate: _currentSpriteAnimationInfo.frameRate,
                newTotalFrames: newTotalFrames,
                newKeyframes: adjustedKeyframes
            );

            _currentSpriteAnimationInfo = modifiedInfo;
            DataChanged?.Invoke(obj: modifiedInfo);
        }

        /// <summary>
        /// Clears all sprite data.
        /// </summary>
        public void Clear()
        {
            ClearData();
        }

        public void SelectedSpritesChanged(Sprite[] sprites)
        {
            SpriteAnimationInfo currentInfo = _currentSpriteAnimationInfo;
            List<Sprite> spritesList = sprites.ToList();
            spritesList
                .Sort(
                    comparison: (a, b) => string.Compare(
                        strA: a.name,
                        strB: b.name,
                        comparisonType: StringComparison.OrdinalIgnoreCase
                    )
                );

            List<SpriteAnimationKeyframe> keyframeData = spritesList
                .Select(
                    selector: (sprite, index) => new SpriteAnimationKeyframe(
                        index: index,
                        time: index / currentInfo.frameRate,
                        sprite: sprite
                    )
                )
                .ToList();

            _currentSpriteAnimationInfo = new SpriteAnimationInfo(
                animationName: currentInfo.animationName,
                duration: keyframeData.Count / currentInfo.frameRate,
                frameRate: currentInfo.frameRate,
                totalFrames: sprites.Length,
                keyframes: keyframeData,
                destinationFolderPath: currentInfo.destinationFolderPath
            );

            _originalKeyframes = keyframeData;
            DataChanged?.Invoke(obj: _currentSpriteAnimationInfo);
        }

        public void UpdateAnimationName(string name)
        {
            _currentSpriteAnimationInfo = _currentSpriteAnimationInfo.WithName(name: name);
        }

        public void UpdateDestinationFolder(string destinationFolderPath)
        {
            _currentSpriteAnimationInfo =
                _currentSpriteAnimationInfo.WithDestinationFolderPath(destinationFolderPath: destinationFolderPath);
        }

        void ClearData()
        {
            _hasData = false;
            _currentSpriteAnimationInfo = default;
            _originalKeyframes = null;
        }

        /// <summary>
        /// Creates a modified version of the sprite info with new frame rate, total frames, and keyframes.
        /// </summary>
        SpriteAnimationInfo CreateModifiedSpriteInfo(
            SpriteAnimationInfo original,
            float newFrameRate,
            int newTotalFrames,
            List<SpriteAnimationKeyframe> newKeyframes
        )
        {
            return new SpriteAnimationInfo(
                animationName: original.animationName,
                duration: newTotalFrames / newFrameRate,
                frameRate: newFrameRate,
                totalFrames: newTotalFrames,
                keyframes: newKeyframes,
                destinationFolderPath: original.destinationFolderPath
            );
        }

        /// <summary>
        /// Adjusts the keyframes list to match the new total frames count.
        /// Uses the original cached keyframes to preserve sprite data when increasing frames.
        /// </summary>
        List<SpriteAnimationKeyframe> AdjustKeyframesForTotalFrames(
            List<SpriteAnimationKeyframe> originalKeyframes,
            int newTotalFrames,
            float frameRate
        )
        {
            List<SpriteAnimationKeyframe> adjustedKeyframes = new List<SpriteAnimationKeyframe>();

            int framesToCopy = Mathf.Min(a: originalKeyframes.Count, b: newTotalFrames);
            for (int i = 0; i < framesToCopy; i++)
            {
                adjustedKeyframes.Add(item: originalKeyframes[index: i]);
            }

            if (newTotalFrames > originalKeyframes.Count)
            {
                for (int i = originalKeyframes.Count; i < newTotalFrames; i++)
                {
                    float time = i / frameRate;
                    adjustedKeyframes.Add(item: new SpriteAnimationKeyframe(index: i, time: time, sprite: null));
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
