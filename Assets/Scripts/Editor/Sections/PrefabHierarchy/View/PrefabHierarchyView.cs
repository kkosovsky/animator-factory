using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace AnimatorFactory.PrefabHierarchy
{
    /// <summary>
    /// UI component for displaying prefab hierarchy.
    /// </summary>
    public class PrefabHierarchyView : VisualElement
    {
        public event Action<PrefabHierarchyListItem> ItemSelected;

        ListView _hierarchyListView;
        List<PrefabHierarchyListItem> _hierarchyNodes = new();

        public PrefabHierarchyView() => CreateUI();

        public void OnHierarchyChanged(List<PrefabHierarchyListItem> hierarchy)
        {
            _hierarchyNodes = hierarchy;
            RefreshItems();
        }

        void CreateUI()
        {
            _hierarchyListView = new ListView
            {
                itemsSource = _hierarchyNodes,
                fixedItemHeight = 20,
                selectionType = SelectionType.Single,
                style = { flexGrow = 1 },
                makeItem = MakeItem,
                bindItem = BindItem
            };

            _hierarchyListView.selectionChanged += OnHierarchySelectionChanged;
            Add(child: _hierarchyListView);
        }

        void RefreshItems()
        {
            _hierarchyListView.itemsSource = _hierarchyNodes;
            _hierarchyListView.RefreshItems();
        }

        void OnHierarchySelectionChanged(IEnumerable<object> selection)
        {
            if (selection.FirstOrDefault() is not PrefabHierarchyListItem selectedItem)
            {
                return;
            }

            ItemSelected?.Invoke(selectedItem);
        }

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

        void BindItem(VisualElement element, int index)
        {
            if (index >= _hierarchyNodes.Count)
            {
                return;
            }

            PrefabHierarchyListItem listItem = _hierarchyNodes[index: index];

            VisualElement indentSpace = element.Q<VisualElement>(name: "indent-space");
            indentSpace.style.width = listItem.depth * 20;

            Image icon = element.Q<Image>(name: "game-object-icon");
            Texture2D gameObjectIcon = AssetPreview.GetMiniThumbnail(obj: listItem.gameObject);
            if (gameObjectIcon != null)
            {
                icon.image = gameObjectIcon;
            }

            Label nameLabel = element.Q<Label>(name: "name-label");
            nameLabel.text = listItem.name;

            VisualElement iconsContainer = element.Q<VisualElement>(name: "icons-container");
            iconsContainer.Clear();

            if (listItem.gameObject.GetComponent<SpriteRenderer>() != null)
            {
                AddComponentIcon<SpriteRenderer>(container: iconsContainer, tooltip: "Has SpriteRenderer component");
            }

            if (listItem.gameObject.GetComponent<Animator>() != null)
            {
                AddComponentIcon<Animator>(container: iconsContainer, tooltip: "Has Animator component");
            }
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

        static void AddComponentIcon<T>(VisualElement container, string tooltip)
            where T : Component
        {
            Texture2D componentTexture = EditorGUIUtility.ObjectContent(obj: null, type: typeof(T)).image as Texture2D;
            if (componentTexture == null) return;

            Image icon = new()
            {
                image = componentTexture,
                tooltip = tooltip,
                style =
                {
                    width = 16,
                    height = 16,
                    marginLeft = 2
                }
            };
            container.Add(child: icon);
        }
    }
}
