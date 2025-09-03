using AnimatorFactory.Core.UI;
using AnimatorFactory.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace AnimatorFactory
{
    /// <summary>
    /// Main editor window for the Animator Factory tool.
    /// </summary>
    public partial class AnimatorFactoryWindow : EditorWindow
    {
        TabView _mainTabView;
        AnimatorFactoryController _controller;

        public static Configuration Configuration { get; private set; }

        /// <summary>
        /// Menu item to open the Animator Factory window.
        /// </summary>
        [MenuItem(itemName: Strings.menuItemName)]
        public static void ShowWindow() => GetWindow<AnimatorFactoryWindow>(title: Strings.windowTitle);

        void OnEnable()
        {
            LoadConfiguration();
            _controller = new AnimatorFactoryController();
            CreateUIElements();
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

            _mainTabView.AddTab(title: Strings.spriteEditionTabLabel, content: _controller.GetSpriteEditionContent());
            _mainTabView.AddTab(title: Strings.animatorStatesTabLabel, content: _controller.GetAnimatorStatesContent());
            _mainTabView.AddTab(title: Strings.prefabVariantsTabLabel, content: _controller.GetPrefabVariantsContent());

            container.Add(child: _mainTabView);
        }

        static void LoadConfiguration()
        {
            string[] guids = AssetDatabase.FindAssets(filter: "t:Configuration");

            if (guids.Length > 0)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid: guids[0]);
                Configuration = AssetDatabase.LoadAssetAtPath<Configuration>(assetPath: assetPath);
            }

            if (Configuration == null)
            {
                Debug.LogWarning(
                    message:
                    "AnimatorFactory Configuration asset not found. Please create one using Create > Animator Factory > Configuration"
                );
            }
        }
    }
}
