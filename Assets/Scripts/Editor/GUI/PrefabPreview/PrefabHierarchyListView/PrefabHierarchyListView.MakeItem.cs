using UnityEngine;
using UnityEngine.UIElements;

namespace AnimatorFactory
{
    public partial class PrefabHierarchyListView
    {
        VisualElement MakeItem()
        {
            VisualElement container = new()
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    alignItems = Align.Center,
                    paddingLeft = 2,
                    paddingRight = 2,
                    paddingTop = 1,
                    paddingBottom = 1
                }
            };

            AddIndentSpace(container: container);
            AddImageIcon(container: container);
            AddNameLabel(container: container);
            AddIconsContainer(container: container);

            return container;
        }

        static void AddIndentSpace(VisualElement container)
        {
            VisualElement indentSpace = new()
            {
                name = "indent-space",
                style = { width = 0 }
            };
            container.Add(child: indentSpace);
        }

        static void AddImageIcon(VisualElement container)
        {
            Image icon = new()
            {
                name = "game-object-icon",
                style =
                {
                    width = 16,
                    height = 16,
                    marginRight = 4,
                    marginLeft = 4
                }
            };
            container.Add(child: icon);
        }

        static void AddNameLabel(VisualElement container)
        {
            Label nameLabel = new()
            {
                name = "name-label",
                style =
                {
                    flexGrow = 1,
                    unityTextAlign = TextAnchor.MiddleLeft
                }
            };
            container.Add(child: nameLabel);
        }

        static void AddIconsContainer(VisualElement container)
        {
            VisualElement iconsContainer = new()
            {
                name = "icons-container",
                style =
                {
                    flexDirection = FlexDirection.Row,
                    alignItems = Align.Center
                }
            };
            container.Add(child: iconsContainer);
        }
    }
}
