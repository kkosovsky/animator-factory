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

        public AnimatorStatesEditionRootView()
        {
            CreatePrefabSelectionSection();
            CreateHierarchySection();
            CreateAnimatorStatesSection();
            CreateSpriteKeyframeSection();
            CreateGenerateControlsView();
        }

        void CreatePrefabSelectionSection()
        {
            prefabField = new PrefabField(label: Strings.prefabSelectionLabel);
            prefabField.RegisterValueChangedCallback(callback: OnPrefabSelectionChanged);
            Add(child: prefabField);
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
            Add(child: hierarchyLabel);

            prefabHierarchyView = new PrefabHierarchyView();
            Add(child: prefabHierarchyView);
        }

        void CreateAnimatorStatesSection()
        {
            animatorStatesView = new AnimatorStatesView();
            Add(child: animatorStatesView);
        }

        void CreateSpriteKeyframeSection()
        {
            spriteKeyframesView = new SpriteKeyframesView();
            Add(child: spriteKeyframesView);
        }

        void CreateGenerateControlsView()
        {
            generationControlsView = new GenerationControlsView();
            Add(child: generationControlsView);
        }

        void OnPrefabSelectionChanged(ChangeEvent<Object> evt)
        {
            GameObject selectedPrefab = evt.newValue as GameObject;
            PrefabSelectionChanged?.Invoke(obj: selectedPrefab);
        }
    }
}
