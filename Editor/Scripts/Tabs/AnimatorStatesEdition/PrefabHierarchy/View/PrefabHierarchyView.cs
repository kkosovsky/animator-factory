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
        int _selectedIndex = -1;

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

            int newSelectedIndex = _hierarchyListView.selectedIndex;

            if (_selectedIndex >= 0)
            {
                RefreshSingleItem(index: _selectedIndex);
            }

            if (newSelectedIndex >= 0)
            {
                RefreshSingleItem(index: newSelectedIndex);
            }

            _selectedIndex = newSelectedIndex;
            ItemSelected?.Invoke(obj: selectedItem);
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
            AddNameLabelAndSelectionIndicator(container: container);
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

            Image selectionIndicator = element.Q<Image>(name: "selection-indicator");
            bool isSelected = _hierarchyListView.selectedIndex == index;
            selectionIndicator.style.display = isSelected ? DisplayStyle.Flex : DisplayStyle.None;

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

        static void AddNameLabelAndSelectionIndicator(VisualElement container)
        {
            VisualElement labelContainer = new()
            {
                name = "label-container",
                style =
                {
                    flexDirection = FlexDirection.Row,
                    alignItems = Align.Center,
                    flexGrow = 1
                }
            };

            Label nameLabel = new()
            {
                name = "name-label",
                style =
                {
                    unityTextAlign = TextAnchor.MiddleLeft,
                    marginRight = 4
                }
            };

            labelContainer.Add(child: nameLabel);
            AddSelectionIndicator(container: labelContainer);
            container.Add(child: labelContainer);
        }

        static void AddSelectionIndicator(VisualElement container)
        {
            Image selectionIndicator = new()
            {
                name = "selection-indicator",
                style =
                {
                    width = 16,
                    height = 16,
                    marginLeft = 4,
                    display = DisplayStyle.None
                }
            };

            selectionIndicator.image = EditorGUIUtility.IconContent(name: "d_FilterSelectedOnly").image;
            container.Add(child: selectionIndicator);
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

        void RefreshSingleItem(int index)
        {
            if (index >= 0 && index < _hierarchyNodes.Count)
            {
                _hierarchyListView.RefreshItem(index: index);
            }
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
