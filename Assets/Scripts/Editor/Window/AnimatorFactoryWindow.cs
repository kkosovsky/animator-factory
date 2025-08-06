using AnimatorFactory.Core.UI;
using AnimatorFactory.Editor;
using AnimatorFactory.PrefabHierarchy;
using AnimatorFactory.SpriteKeyframePreview;
using AnimatorFactory.AnimatorStatePreview;
using AnimatorFactory.GenerationControls;
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
        TabView _mainTabView;
        PrefabHierarchyView _prefabHierarchyView;
        AnimatorStatesView _animatorStatesView;
        SpriteKeyframesView _spriteKeyframesView;
        GenerationControlsView _generationControlsView;

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

            CreateTabs(container: mainContainer);
        }

        void CreateTabs(VisualElement container)
        {
            _mainTabView = new TabView
            {
                style =
                {
                    flexGrow = 1
                }
            };

            VisualElement spriteEditionContent = CreatSpriteEditionContent();
            _mainTabView.AddTab(title: Strings.spriteEditionTabLabel, content: spriteEditionContent);
            
            VisualElement animatorStatesContent = CreateAnimatorStatesTabContent();
            _mainTabView.AddTab(title: Strings.animatorStatesTabLabel, content: animatorStatesContent);

            container.Add(child: _mainTabView);
        }

        void InitializeController()
        {
            _controller = new AnimatorFactoryController(
                prefabHierarchyView: _prefabHierarchyView,
                animatorStatesView: _animatorStatesView,
                spriteKeyframesView: _spriteKeyframesView,
                generationControlsView: _generationControlsView
            );
        }
    }
}
