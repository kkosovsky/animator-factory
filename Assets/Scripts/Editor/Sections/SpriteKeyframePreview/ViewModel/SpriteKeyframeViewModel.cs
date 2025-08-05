using System;
using System.Collections.Generic;
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
        public event Action<AnimationSpriteInfo> DataChanged;

        /// <summary>
        /// Fired when status message changes.
        /// </summary>
        public event Action<string, bool> StatusChanged; // message, isError

        public AnimationSpriteInfo AnimationInfo => _currentAnimationInfo;

        AnimationSpriteInfo _currentAnimationInfo;
        List<SpriteKeyframeData> _originalKeyframes;
        bool _hasData;

        /// <summary>
        /// Creates a new empty animation state for editing.
        /// </summary>
        /// <param name="stateName">The name of the new state</param>
        public void CreateNewAnimationState(string stateName)
        {
            // Create a new animation with default empty keyframes
            float defaultFrameRate = 12.0f;
            int defaultTotalFrames = 12;

            List<SpriteKeyframeData> defaultKeyframes = new List<SpriteKeyframeData>();
            for (int i = 0; i < defaultTotalFrames; i++)
            {
                float time = i / defaultFrameRate;
                defaultKeyframes.Add(item: new SpriteKeyframeData(index: i, time: time, sprite: null));
            }

            _currentAnimationInfo = new AnimationSpriteInfo(
                animationName: stateName,
                duration: defaultTotalFrames / defaultFrameRate,
                frameRate: defaultFrameRate,
                totalFrames: defaultTotalFrames,
                keyframes: defaultKeyframes,
                destinationFolderPath: "Assets/Animations"
            );

            _originalKeyframes = new List<SpriteKeyframeData>(collection: defaultKeyframes);
            _hasData = true;

            DataChanged?.Invoke(obj: _currentAnimationInfo);
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

                AnimationSpriteInfo spriteInfo = SpriteKeyframeService.ExtractSpriteKeyframes(clip: clip);

                if (spriteInfo.keyframes.Count == 0)
                {
                    ShowStatus(message: "No sprite keyframes found in this animation.", isError: false);
                    ClearData();
                    return;
                }

                _originalKeyframes = new List<SpriteKeyframeData>(collection: spriteInfo.keyframes);
                _currentAnimationInfo = spriteInfo;
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

            AnimationSpriteInfo modifiedInfo = CreateModifiedSpriteInfo(
                original: _currentAnimationInfo,
                newFrameRate: newFrameRate,
                newTotalFrames: _currentAnimationInfo.totalFrames,
                newKeyframes: _currentAnimationInfo.keyframes
            );

            _currentAnimationInfo = modifiedInfo;
            DataChanged?.Invoke(obj: modifiedInfo);
        }

        /// <summary>
        /// Updates the total frames count and adjusts the keyframes list accordingly.
        /// </summary>
        /// <param name="newTotalFrames">The new total frames value</param>
        public void UpdateTotalFrames(int newTotalFrames)
        {
            if (!_hasData || newTotalFrames <= 0 || _originalKeyframes == null) return;

            List<SpriteKeyframeData> adjustedKeyframes = AdjustKeyframesForTotalFrames(
                originalKeyframes: _originalKeyframes,
                newTotalFrames: newTotalFrames,
                frameRate: _currentAnimationInfo.frameRate
            );

            AnimationSpriteInfo modifiedInfo = CreateModifiedSpriteInfo(
                original: _currentAnimationInfo,
                newFrameRate: _currentAnimationInfo.frameRate,
                newTotalFrames: newTotalFrames,
                newKeyframes: adjustedKeyframes
            );

            _currentAnimationInfo = modifiedInfo;
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
            AnimationSpriteInfo currentInfo = _currentAnimationInfo;
            List<Sprite> spritesList = sprites.ToList();
            spritesList
                .Sort(
                    comparison: (a, b) => string.Compare(
                        strA: a.name,
                        strB: b.name,
                        comparisonType: StringComparison.OrdinalIgnoreCase
                    )
                );

            List<SpriteKeyframeData> keyframeData = spritesList
                .Select(
                    selector: (sprite, index) => new SpriteKeyframeData(
                        index: index,
                        time: index / currentInfo.frameRate,
                        sprite: sprite
                    )
                )
                .ToList();

            _currentAnimationInfo = new AnimationSpriteInfo(
                animationName: currentInfo.animationName,
                duration: keyframeData.Count / currentInfo.frameRate,
                frameRate: currentInfo.frameRate,
                totalFrames: sprites.Length,
                keyframes: keyframeData,
                destinationFolderPath: currentInfo.destinationFolderPath
            );

            _originalKeyframes = keyframeData;
            DataChanged?.Invoke(obj: _currentAnimationInfo);
        }

        public void UpdateAnimationName(string name)
        {
            _currentAnimationInfo = _currentAnimationInfo.WithName(name: name);
            Debug.Log(message: _currentAnimationInfo);
        }

        public void UpdateDestinationFolder(string destinationFolderPath)
        {
            _currentAnimationInfo =
                _currentAnimationInfo.WithDestinationFolderPath(destinationFolderPath: destinationFolderPath);
        }

        void ClearData()
        {
            _hasData = false;
            _currentAnimationInfo = default;
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
        List<SpriteKeyframeData> AdjustKeyframesForTotalFrames(
            List<SpriteKeyframeData> originalKeyframes,
            int newTotalFrames,
            float frameRate
        )
        {
            List<SpriteKeyframeData> adjustedKeyframes = new List<SpriteKeyframeData>();

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
                    adjustedKeyframes.Add(item: new SpriteKeyframeData(index: i, time: time, sprite: null));
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
