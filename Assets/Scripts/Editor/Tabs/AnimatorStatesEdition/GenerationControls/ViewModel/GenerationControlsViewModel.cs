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
        const float maxGenerationDuration = 0.5f;
        AnimationClip _generatedClip;
        string _generatedStateName;

        public void GenerateAnimationClips(SpriteAnimationInfo spriteAnimationInfo)
        {
            if (_isGenerating)
            {
                return;
            }

            StartGeneration(spriteAnimationInfo: spriteAnimationInfo);
        }

        void StartGeneration(SpriteAnimationInfo spriteAnimationInfo)
        {
            _isGenerating = true;
            _generationStartTime = (float)EditorApplication.timeSinceStartup;
            _generatedStateName = spriteAnimationInfo.animationName;

            EditorApplication.update += UpdateGenerationProgress;

            StartedGeneration?.Invoke();
            _generatedClip = AnimationClipGenerationService.CreateAnimationClip(
                sprites: spriteAnimationInfo.keyframes.Select(selector: data => data.sprite).ToArray(),
                keyframeCount: spriteAnimationInfo.totalFrames,
                frameRate: spriteAnimationInfo.frameRate,
                hasLoopTime: false,
                wrapMode: WrapMode.Clamp,
                animationName: spriteAnimationInfo.animationName,
                destinationFolderPath: spriteAnimationInfo.destinationFolderPath
            );
        }

        void UpdateGenerationProgress()
        {
            if (!_isGenerating) return;

            float elapsedTime = (float)EditorApplication.timeSinceStartup - _generationStartTime;
            float progress = Mathf.Clamp01(value: elapsedTime / maxGenerationDuration);

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
