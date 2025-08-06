using System.Collections.Generic;
using AnimatorFactory.PrefabHierarchy;
using AnimatorFactory.SpriteKeyframePreview;
using AnimatorFactory.AnimatorStatePreview;
using AnimatorFactory.GenerationControls;
using UnityEditor.Animations;
using UnityEngine;

namespace AnimatorFactory.Editor
{
    public class AnimatorStateEditionTabController
    {
        readonly PrefabHierarchyViewModel _prefabHierarchyViewModel;
        readonly AnimatorStatesViewModel _animatorStatesViewModel;
        readonly SpriteKeyframeViewModel _spriteKeyframeViewModel;
        readonly GenerationControlsViewModel _generationControlsViewModel;

        readonly PrefabHierarchyView _prefabHierarchyView;
        readonly AnimatorStatesView _animatorStatesView;
        readonly SpriteKeyframesView _spriteKeyframesView;
        readonly GenerationControlsView _generationControlsView;

        public AnimatorStateEditionTabController(
            PrefabHierarchyViewModel prefabHierarchyViewModel,
            AnimatorStatesViewModel animatorStatesViewModel,
            SpriteKeyframeViewModel spriteKeyframeViewModel,
            GenerationControlsViewModel generationControlsViewModel,
            PrefabHierarchyView prefabHierarchyView,
            AnimatorStatesView animatorStatesView,
            SpriteKeyframesView spriteKeyframesView,
            GenerationControlsView generationControlsView
        )
        {
            _prefabHierarchyViewModel = prefabHierarchyViewModel;
            _animatorStatesViewModel = animatorStatesViewModel;
            _spriteKeyframeViewModel = spriteKeyframeViewModel;
            _generationControlsViewModel = generationControlsViewModel;

            _prefabHierarchyView = prefabHierarchyView;
            _animatorStatesView = animatorStatesView;
            _spriteKeyframesView = spriteKeyframesView;
            _generationControlsView = generationControlsView;

            BindEvents();
        }

        public void OnPrefabSelectionChanged(GameObject prefab)
        {
            if (prefab == null)
            {
                _prefabHierarchyViewModel.Clear();
                _animatorStatesViewModel.Clear();
                _spriteKeyframeViewModel.Clear();
            }
            else
            {
                _prefabHierarchyViewModel.LoadHierarchy(prefab: prefab);
                _animatorStatesViewModel.Clear();
                _spriteKeyframeViewModel.Clear();
            }
        }

        public void Dispose() => UnbindEvents();

        void BindEvents()
        {
            BindPrefabHierarchyPreviewEvents();
            BindAnimatorStatesPreviewEvents();
            BindSpriteKeyFramePreviewEvents();
            BindGenerationControlsEvents();
            BindHierarchyChangedEvent();
        }

        void UnbindEvents()
        {
            UnbindPrefabHierarchyEvents();
            UnbindAnimatorStatesPreviewEvents();
            UnbindSpriteKeyFramePreviewEvents();
            UnbindGenerationControlsEvents();
            UnbindHierarchyChangedEvent();
        }

        void BindHierarchyChangedEvent()
        {
            _prefabHierarchyViewModel.HierarchyChanged += OnPrefabHierarchyChanged;
        }

        void BindPrefabHierarchyPreviewEvents()
        {
            _prefabHierarchyViewModel.HierarchyChanged += _prefabHierarchyView.OnHierarchyChanged;
            _prefabHierarchyView.ItemSelected += OnPrefabHierarchyItemSelected;
        }

        void UnbindPrefabHierarchyEvents()
        {
            _prefabHierarchyViewModel.HierarchyChanged -= _prefabHierarchyView.OnHierarchyChanged;
            _prefabHierarchyView.ItemSelected -= OnPrefabHierarchyItemSelected;
        }

        void BindAnimatorStatesPreviewEvents()
        {
            _animatorStatesViewModel.StatesChanged += _animatorStatesView.OnStatesChanged;
            _animatorStatesViewModel.StatusChanged += _animatorStatesView.OnStatusChanged;

            _animatorStatesView.SelectedState += OnAnimatorStateSelected;
            _animatorStatesView.AddStateRequested += OnAddStateRequested;
        }

        void UnbindAnimatorStatesPreviewEvents()
        {
            _animatorStatesViewModel.StatesChanged -= _animatorStatesView.OnStatesChanged;
            _animatorStatesViewModel.StatusChanged -= _animatorStatesView.OnStatusChanged;
            
            _animatorStatesView.SelectedState -= OnAnimatorStateSelected;
            _animatorStatesView.AddStateRequested -= OnAddStateRequested;
        }

        void OnPrefabHierarchyChanged(List<PrefabHierarchyListItem> hierarchy)
        {
            _animatorStatesViewModel.Clear();
            _spriteKeyframeViewModel.Clear();
        }

        void OnAnimatorStateSelected(AnimatorState state)
        {
            _spriteKeyframeViewModel.LoadSpriteKeyframes(state: state);
        }

        void OnAddStateRequested()
        {
            _spriteKeyframeViewModel.CreateNewAnimationState(stateName: "New State");
        }

        void OnPrefabHierarchyItemSelected(PrefabHierarchyListItem item)
        {
            _animatorStatesViewModel.LoadAnimatorStates(item: item);
        }

        void BindSpriteKeyFramePreviewEvents()
        {
            _spriteKeyframeViewModel.StatusChanged += _spriteKeyframesView.OnStatusChanged;
            _spriteKeyframeViewModel.DataChanged += OnKeyframeDataChanged;
            
            _spriteKeyframesView.FrameRateChanged += OnFrameRateChanged;
            _spriteKeyframesView.TotalFramesChanged += OnTotalFramesChanged;
            _spriteKeyframesView.SpritesSelected += OnSpritesSelected;
            _spriteKeyframesView.AnimationNameChanged += OnAnimationNameChanged;
            _spriteKeyframesView.DestinationFolderChanged += OnDestinationFolderChanged;
        }

        void UnbindSpriteKeyFramePreviewEvents()
        {
            _spriteKeyframeViewModel.StatusChanged -= _spriteKeyframesView.OnStatusChanged;
            _spriteKeyframeViewModel.DataChanged -= OnKeyframeDataChanged;
            
            _spriteKeyframesView.FrameRateChanged -= OnFrameRateChanged;
            _spriteKeyframesView.TotalFramesChanged -= OnTotalFramesChanged;
            _spriteKeyframesView.SpritesSelected -= OnSpritesSelected;
            _spriteKeyframesView.AnimationNameChanged -= OnAnimationNameChanged;
            _spriteKeyframesView.DestinationFolderChanged -= OnDestinationFolderChanged;
        }

        void OnKeyframeDataChanged(AnimationSpriteInfo spriteInfo)
        {
            _spriteKeyframesView.OnDataChanged(spriteInfo: spriteInfo);
        }

        void OnFrameRateChanged(float newFrameRate)
        {
            _spriteKeyframeViewModel.UpdateFrameRate(newFrameRate: newFrameRate);
        }

        void OnTotalFramesChanged(int newTotalFrames)
        {
            _spriteKeyframeViewModel.UpdateTotalFrames(newTotalFrames: newTotalFrames);
        }

        void OnSpritesSelected(Sprite[] sprites)
        {
            _spriteKeyframeViewModel.SelectedSpritesChanged(sprites: sprites);
            _generationControlsView.ShowButton();
        }

        void OnAnimationNameChanged(string newAnimationName)
        {
            _spriteKeyframeViewModel.UpdateAnimationName(name: newAnimationName);
        }

        void OnDestinationFolderChanged(string newDestinationFolder)
        {
            _spriteKeyframeViewModel.UpdateDestinationFolder(destinationFolderPath: newDestinationFolder);
        }

        void BindGenerationControlsEvents()
        {
            _generationControlsView.GenerateButtonClicked += OnGenerateButtonClicked;
            _generationControlsViewModel.StartedGeneration +=
                _generationControlsView.ShowIsGeneratingDialogue;
            _generationControlsViewModel.UpdatedGenerationProgress +=
                _generationControlsView.UpdateGenerationProgressDialogue;
            _generationControlsViewModel.FinishedGeneration +=
                _generationControlsView.HideGeneratingDialogue;
            _generationControlsViewModel.AnimationClipGenerated += OnAnimationClipGenerated;
        }

        void UnbindGenerationControlsEvents()
        {
            _generationControlsView.GenerateButtonClicked -= OnGenerateButtonClicked;
            _generationControlsViewModel.StartedGeneration -=
                _generationControlsView.ShowIsGeneratingDialogue;
            _generationControlsViewModel.UpdatedGenerationProgress -=
                _generationControlsView.UpdateGenerationProgressDialogue;
            _generationControlsViewModel.FinishedGeneration -=
                _generationControlsView.HideGeneratingDialogue;
            _generationControlsViewModel.AnimationClipGenerated -= OnAnimationClipGenerated;
        }
        
        void UnbindHierarchyChangedEvent()
        {
            _prefabHierarchyViewModel.HierarchyChanged -= OnPrefabHierarchyChanged;
        }

        void OnGenerateButtonClicked()
        {
            _generationControlsView.ShowIsGeneratingDialogue();
            _generationControlsViewModel.GenerateAnimationClips(animationInfo: _spriteKeyframeViewModel.AnimationInfo);
        }

        void OnAnimationClipGenerated(AnimationClip animationClip, string stateName)
        {
            if (animationClip == null)
            {
                Debug.LogError(message: "Animation clip is null in OnAnimationClipGenerated");
                return;
            }

            if (string.IsNullOrEmpty(value: stateName))
            {
                Debug.LogError(message: "State name is null or empty in OnAnimationClipGenerated");
                return;
            }

            _animatorStatesViewModel.CreateNewStateWithClip(stateName: stateName, animationClip: animationClip);
        }
    }
}
