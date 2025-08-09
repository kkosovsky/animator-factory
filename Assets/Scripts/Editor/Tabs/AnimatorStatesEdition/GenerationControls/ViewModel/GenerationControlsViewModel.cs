using System;
using System.Linq;
using AnimatorFactory.SpriteKeyframePreview;
using UnityEngine;
using UnityEditor;

namespace AnimatorFactory.GenerationControls
{
    public class GenerationControlsViewModel
    {
        public event Action StartedGeneration;
        public event Action<float> UpdatedGenerationProgress;
        public event Action FinishedGeneration;
        public event Action<AnimationClip, string> AnimationClipGenerated; // clip, stateName

        bool _isGenerating;
        float _generationStartTime;
        float _generationDuration = 1.0f;
        AnimationClip _generatedClip;
        string _generatedStateName;

        public void GenerateAnimationClips(AnimationSpriteInfo animationInfo)
        {
            if (_isGenerating)
            {
                return;
            }

            StartGeneration(animationInfo: animationInfo);
        }

        void StartGeneration(AnimationSpriteInfo animationInfo)
        {
            _isGenerating = true;
            _generationStartTime = (float)EditorApplication.timeSinceStartup;
            _generatedStateName = animationInfo.animationName;

            EditorApplication.update += UpdateGenerationProgress;

            StartedGeneration?.Invoke();
            _generatedClip = AnimationClipGenerationService.CreateAnimationClip(
                sprites: animationInfo.keyframes.Select(selector: data => data.sprite).ToArray(),
                keyframeCount: animationInfo.totalFrames,
                frameRate: animationInfo.frameRate,
                hasLoopTime: false,
                wrapMode: WrapMode.Clamp,
                animationName: animationInfo.animationName,
                destinationFolderPath: animationInfo.destinationFolderPath
            );
        }

        void UpdateGenerationProgress()
        {
            if (!_isGenerating) return;

            float elapsedTime = (float)EditorApplication.timeSinceStartup - _generationStartTime;
            float progress = Mathf.Clamp01(value: elapsedTime / _generationDuration);

            UpdatedGenerationProgress?.Invoke(obj: progress);

            if (progress >= 1.0f)
            {
                CompleteGeneration();
            }
        }

        void CompleteGeneration()
        {
            _isGenerating = false;

            EditorApplication.update -= UpdateGenerationProgress;

            if (_generatedClip != null && !string.IsNullOrEmpty(value: _generatedStateName))
            {
                AnimationClipGenerated?.Invoke(arg1: _generatedClip, arg2: _generatedStateName);
            }
            else
            {
                Debug.LogWarning(
                    message:
                    $"Not firing AnimationClipGenerated event - clip is null: {_generatedClip == null}, state name is empty: {string.IsNullOrEmpty(value: _generatedStateName)}"
                );
            }

            FinishedGeneration?.Invoke();
            
            Debug.Log(message: "...:: Generation Completed ::...");
            _generatedClip = null;
            _generatedStateName = null;
        }
    }
}
