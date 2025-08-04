using AnimatorFactory.Editor;
using AnimatorFactory.PrefabHierarchy;
using AnimatorFactory.SpriteKeyframePreview;
using AnimatorFactory.AnimatorStatePreview;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace AnimatorFactory
{
    /// <summary>
    /// Main editor window for the Animator Factory tool.
    /// </summary>
    public partial class AnimatorFactoryWindow : EditorWindow
    {
        ObjectField _prefabField;
        PrefabHierarchyView _prefabHierarchyView;
        AnimatorStatesView _animatorStatesView;
        SpriteKeyframeView _spriteKeyframeView;

        AnimatorFactoryController _controller;

        /// <summary>
        /// Menu item to open the Animator Factory window.
        /// </summary>
        [MenuItem(itemName: Strings.menuItemName)]
        public static void ShowWindow() => GetWindow<AnimatorFactoryWindow>(title: Strings.windowTitle);

        void OnEnable()
        {
            CreateUIElements();
            InitializeController();
        }

        void OnDisable() => _controller?.Dispose();

        void CreateUIElements()
        {
            rootVisualElement.Clear();

            VisualElement mainContainer = new()
            {
                style =
                {
                    paddingTop = 10,
                    paddingBottom = 10,
                    paddingLeft = 10,
                    paddingRight = 10,
                    flexGrow = 1
                }
            };
            rootVisualElement.Add(child: mainContainer);

            CreatePrefabSelectionSection(container: mainContainer);
            CreateHierarchySection(container: mainContainer);
            CreateAnimatorStatesSection(container: mainContainer);
            CreateSpriteKeyframeSection(container: mainContainer);
        }

        void InitializeController()
        {
            _controller = new AnimatorFactoryController(
                prefabHierarchyView: _prefabHierarchyView,
                animatorStatesView: _animatorStatesView,
                spriteKeyframeView: _spriteKeyframeView
            );
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
                style = { 
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
            _spriteKeyframeView = new SpriteKeyframeView();
            container.Add(child: _spriteKeyframeView);
        }

        void OnPrefabSelectionChanged(ChangeEvent<Object> evt)
        {
            GameObject selectedPrefab = evt.newValue as GameObject;
            _controller?.OnPrefabSelectionChanged(prefab: selectedPrefab);
        }
    }
}
