using System.Collections.Generic;
using AnimatorFactory.PrefabHierarchy;
using AnimatorFactory.SpriteKeyframePreview;
using AnimatorFactory.AnimatorStatePreview;
using AnimatorFactory.AnimatorStates;
using AnimatorFactory.GenerationControls;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UIElements;

namespace AnimatorFactory.Editor
{
    public class AnimatorStateEditionTabController
    {
        readonly PrefabHierarchyViewModel _prefabHierarchyViewModel;
        readonly AnimatorStatesViewModel _animatorStatesViewModel;
        readonly SpriteKeyframeViewModel _spriteKeyframeViewModel;
        readonly GenerationControlsViewModel _generationControlsViewModel;
        readonly AnimatorStatesEditionRootView _view;

        public AnimatorStateEditionTabController()
        {
            _prefabHierarchyViewModel = new PrefabHierarchyViewModel();
            _animatorStatesViewModel = new AnimatorStatesViewModel();
            _spriteKeyframeViewModel = new SpriteKeyframeViewModel();
            _generationControlsViewModel = new GenerationControlsViewModel();
            _view = new AnimatorStatesEditionRootView();

            BindEvents();
        }

        public void Dispose() => UnbindEvents();

        public VisualElement GetContent() => _view;

        void BindEvents()
        {
            BindPrefabSelectionEvents();
            BindPrefabHierarchyPreviewEvents();
            BindAnimatorStatesPreviewEvents();
            BindSpriteKeyFramePreviewEvents();
            BindGenerationControlsEvents();
            BindHierarchyChangedEvent();
        }

        void UnbindEvents()
        {
            UnbindPrefabSelectionEvents();
            UnbindPrefabHierarchyEvents();
            UnbindAnimatorStatesPreviewEvents();
            UnbindSpriteKeyFramePreviewEvents();
            UnbindGenerationControlsEvents();
            UnbindHierarchyChangedEvent();
        }

        void BindPrefabSelectionEvents() => _view.PrefabSelectionChanged += OnPrefabSelectionChanged;

        void BindHierarchyChangedEvent() => _prefabHierarchyViewModel.HierarchyChanged += OnPrefabHierarchyChanged;

        void BindPrefabHierarchyPreviewEvents()
        {
            _prefabHierarchyViewModel.HierarchyChanged += _view.prefabHierarchyView.OnHierarchyChanged;
            _view.prefabHierarchyView.ItemSelected += OnPrefabHierarchyItemSelected;
        }

        void UnbindPrefabSelectionEvents() => _view.PrefabSelectionChanged -= OnPrefabSelectionChanged;

        void UnbindPrefabHierarchyEvents()
        {
            _prefabHierarchyViewModel.HierarchyChanged -= _view.prefabHierarchyView.OnHierarchyChanged;
            _view.prefabHierarchyView.ItemSelected -= OnPrefabHierarchyItemSelected;
        }

        void BindAnimatorStatesPreviewEvents()
        {
            _animatorStatesViewModel.StatesChanged += _view.animatorStatesView.OnStatesChanged;
            _animatorStatesViewModel.StatusChanged += _view.animatorStatesView.OnStatusChanged;

            _view.animatorStatesView.SelectedState += OnAnimatorStateSelected;
            _view.animatorStatesView.AddStateRequested += OnAddStateRequested;
        }

        void UnbindAnimatorStatesPreviewEvents()
        {
            _animatorStatesViewModel.StatesChanged -= _view.animatorStatesView.OnStatesChanged;
            _animatorStatesViewModel.StatusChanged -= _view.animatorStatesView.OnStatusChanged;

            _view.animatorStatesView.SelectedState -= OnAnimatorStateSelected;
            _view.animatorStatesView.AddStateRequested -= OnAddStateRequested;
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
            _spriteKeyframeViewModel.StatusChanged += _view.spriteKeyframesView.OnStatusChanged;
            _spriteKeyframeViewModel.DataChanged += OnKeyframeDataChanged;

            _view.spriteKeyframesView.FrameRateChanged += OnFrameRateChanged;
            _view.spriteKeyframesView.TotalFramesChanged += OnTotalFramesChanged;
            _view.spriteKeyframesView.SpritesSelected += OnSpritesSelected;
            _view.spriteKeyframesView.AnimationNameChanged += OnAnimationNameChanged;
            _view.spriteKeyframesView.DestinationFolderChanged += OnDestinationFolderChanged;
        }

        void UnbindSpriteKeyFramePreviewEvents()
        {
            _spriteKeyframeViewModel.StatusChanged -= _view.spriteKeyframesView.OnStatusChanged;
            _spriteKeyframeViewModel.DataChanged -= OnKeyframeDataChanged;

            _view.spriteKeyframesView.FrameRateChanged -= OnFrameRateChanged;
            _view.spriteKeyframesView.TotalFramesChanged -= OnTotalFramesChanged;
            _view.spriteKeyframesView.SpritesSelected -= OnSpritesSelected;
            _view.spriteKeyframesView.AnimationNameChanged -= OnAnimationNameChanged;
            _view.spriteKeyframesView.DestinationFolderChanged -= OnDestinationFolderChanged;
        }

        void OnKeyframeDataChanged(AnimationSpriteInfo spriteInfo)
        {
            _view.spriteKeyframesView.OnDataChanged(spriteInfo: spriteInfo);
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
            _view.generationControlsView.ShowButton();
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
            _view.generationControlsView.GenerateButtonClicked += OnGenerateButtonClicked;
            _generationControlsViewModel.StartedGeneration +=
                _view.generationControlsView.ShowIsGeneratingDialogue;
            _generationControlsViewModel.UpdatedGenerationProgress +=
                _view.generationControlsView.UpdateGenerationProgressDialogue;
            _generationControlsViewModel.FinishedGeneration +=
                _view.generationControlsView.HideGeneratingDialogue;
            _generationControlsViewModel.AnimationClipGenerated += OnAnimationClipGenerated;
        }

        void UnbindGenerationControlsEvents()
        {
            _view.generationControlsView.GenerateButtonClicked -= OnGenerateButtonClicked;
            _generationControlsViewModel.StartedGeneration -=
                _view.generationControlsView.ShowIsGeneratingDialogue;
            _generationControlsViewModel.UpdatedGenerationProgress -=
                _view.generationControlsView.UpdateGenerationProgressDialogue;
            _generationControlsViewModel.FinishedGeneration -=
                _view.generationControlsView.HideGeneratingDialogue;
            _generationControlsViewModel.AnimationClipGenerated -= OnAnimationClipGenerated;
        }

        void UnbindHierarchyChangedEvent()
        {
            _prefabHierarchyViewModel.HierarchyChanged -= OnPrefabHierarchyChanged;
        }

        void OnPrefabSelectionChanged(GameObject prefab)
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

        void OnGenerateButtonClicked()
        {
            _view.generationControlsView.ShowIsGeneratingDialogue();
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
