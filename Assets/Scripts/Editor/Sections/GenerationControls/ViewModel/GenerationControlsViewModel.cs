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

        bool _isGenerating;
        float _generationStartTime;
        float _generationDuration = 1.0f;

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

            EditorApplication.update += UpdateGenerationProgress;

            StartedGeneration?.Invoke();
            AnimationClipGenerationService.CreateAnimationClip(
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

            FinishedGeneration?.Invoke();
            Debug.Log(message: "...:: Generation Completed ::...");
        }
    }
}
