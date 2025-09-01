using System;
using AnimatorFactory.AnimatorStatePreview;
using AnimatorFactory.Core.UI;
using AnimatorFactory.GenerationControls;
using AnimatorFactory.PrefabHierarchy;
using AnimatorFactory.SpriteKeyframePreview;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace AnimatorFactory.AnimatorStates
{
    public partial class AnimatorStatesEditionRootView : VisualElement
    {
        public event Action<GameObject> PrefabSelectionChanged;

        public PrefabField prefabField;
        public PrefabHierarchyView prefabHierarchyView;
        public AnimatorStatesView animatorStatesView;
        public SpriteKeyframesView spriteKeyframesView;
        public GenerationControlsView generationControlsView;

        ScrollView _mainScrollView;
        VisualElement _contentContainer;

        public AnimatorStatesEditionRootView()
        {
            CreateScrollableContainer();
            CreatePrefabSelectionSection();
            CreateHierarchySection();
            CreateAnimatorStatesSection();
            CreateSpriteKeyframeSection();
            CreateGenerateControlsView();
        }

        void CreateScrollableContainer()
        {
            _mainScrollView = new ScrollView(ScrollViewMode.Vertical)
            {
                style = 
                {
                    flexGrow = 1
                }
            };

            _contentContainer = new VisualElement()
            {
                style = 
                {
                    flexGrow = 1,
                    paddingLeft = 5,
                    paddingRight = 5,
                    paddingTop = 5,
                    paddingBottom = 5
                }
            };

            _mainScrollView.Add(_contentContainer);
            Add(_mainScrollView);
        }

        void CreatePrefabSelectionSection()
        {
            prefabField = new PrefabField(label: Strings.prefabSelectionLabel);
            prefabField.RegisterValueChangedCallback(callback: OnPrefabSelectionChanged);
            _contentContainer.Add(child: prefabField);
        }

        void CreateHierarchySection()
        {
            Label hierarchyLabel = new(text: Strings.hierarchyLabel)
            {
                style =
                {
                    unityFontStyleAndWeight = FontStyle.Bold,
                    marginTop = 10,
                    marginBottom = 5
                }
            };
            _contentContainer.Add(child: hierarchyLabel);

            prefabHierarchyView = new PrefabHierarchyView();
            _contentContainer.Add(child: prefabHierarchyView);
        }

        void CreateAnimatorStatesSection()
        {
            animatorStatesView = new AnimatorStatesView();
            _contentContainer.Add(child: animatorStatesView);
        }

        void CreateSpriteKeyframeSection()
        {
            spriteKeyframesView = new SpriteKeyframesView();
            _contentContainer.Add(child: spriteKeyframesView);
        }

        void CreateGenerateControlsView()
        {
            generationControlsView = new GenerationControlsView();
            _contentContainer.Add(child: generationControlsView);
        }

        void OnPrefabSelectionChanged(ChangeEvent<Object> evt)
        {
            GameObject selectedPrefab = evt.newValue as GameObject;
            PrefabSelectionChanged?.Invoke(obj: selectedPrefab);
        }
    }
}
