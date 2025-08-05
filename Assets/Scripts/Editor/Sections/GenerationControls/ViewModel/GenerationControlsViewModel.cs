using System;
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
        float _generationDuration = 3.0f;

        public void GenerateAnimationClips()
        {
            if (_isGenerating)
            {
                return;
            }

            StartGeneration();
        }

        void StartGeneration()
        {
            _isGenerating = true;
            _generationStartTime = (float)EditorApplication.timeSinceStartup;

            EditorApplication.update += UpdateGenerationProgress;

            StartedGeneration?.Invoke();
            Debug.Log("...:: Generation Started ::...");
        }

        void UpdateGenerationProgress()
        {
            if (!_isGenerating) return;

            float elapsedTime = (float)EditorApplication.timeSinceStartup - _generationStartTime;
            float progress = Mathf.Clamp01(elapsedTime / _generationDuration);

            UpdatedGenerationProgress?.Invoke(progress);

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
            Debug.Log("...:: Generation Completed ::...");
        }
    }
}
