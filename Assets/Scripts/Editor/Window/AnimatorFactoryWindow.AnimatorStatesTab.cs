using AnimatorFactory.AnimatorStatePreview;
using AnimatorFactory.GenerationControls;
using AnimatorFactory.PrefabHierarchy;
using AnimatorFactory.SpriteKeyframePreview;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace AnimatorFactory
{
    public partial class AnimatorFactoryWindow
    {
        VisualElement CreateAnimatorStatesTabContent()
        {
            VisualElement content = new()
            {
                style = { flexGrow = 1 }
            };

            CreatePrefabSelectionSection(container: content);
            CreateHierarchySection(container: content);
            CreateAnimatorStatesSection(container: content);
            CreateSpriteKeyframeSection(container: content);
            CreateGenerateControlsView(container: content);

            return content;
        }

        void CreatePrefabSelectionSection(VisualElement container)
        {
            _prefabField = new ObjectField(label: Strings.prefabSelectionLabel)
            {
                objectType = typeof(GameObject),
                allowSceneObjects = false
            };

            _prefabField.RegisterValueChangedCallback(callback: OnPrefabSelectionChanged);
            container.Add(child: _prefabField);
        }

        void CreateHierarchySection(VisualElement container)
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
            container.Add(child: hierarchyLabel);

            _prefabHierarchyView = new PrefabHierarchyView();
            container.Add(child: _prefabHierarchyView);
        }

        void CreateAnimatorStatesSection(VisualElement container)
        {
            _animatorStatesView = new AnimatorStatesView();
            container.Add(child: _animatorStatesView);
        }

        void CreateSpriteKeyframeSection(VisualElement container)
        {
            _spriteKeyframesView = new SpriteKeyframesView();
            container.Add(child: _spriteKeyframesView);
        }

        void CreateGenerateControlsView(VisualElement container)
        {
            _generationControlsView = new GenerationControlsView();
            container.Add(child: _generationControlsView);
        }

        void OnPrefabSelectionChanged(ChangeEvent<Object> evt)
        {
            GameObject selectedPrefab = evt.newValue as GameObject;
            _controller?.OnPrefabSelectionChanged(prefab: selectedPrefab);
        }
    }
}
