using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

namespace AnimatorFactory.SpriteKeyframePreview
{
    /// <summary>
    /// Custom UIElement for multi-selecting sprites from the project.
    /// Provides search, filtering, and multi-select functionality.
    /// </summary>
    public class SpriteSelectionListView : VisualElement
    {
        /// <summary>
        /// Fired when sprite selection changes.
        /// </summary>
        public event Action<Sprite[]> SelectionChanged;

        /// <summary>
        /// Fired when sprites are confirmed/applied.
        /// </summary>
        public event Action<Sprite[]> SpritesApplied;

        /// <summary>
        /// Fired when cancel is requested.
        /// </summary>
        public event Action CancelRequested;

        ListView _listView;
        TextField _searchField;
        Label _selectionCountLabel;
        Button _selectAllButton;
        Button _clearAllButton;
        Button _applyButton;
        Button _cancelButton;

        List<Sprite> _allSprites;
        List<Sprite> _filteredSprites;
        string _currentFilter = "";

        public SpriteSelectionListView()
        {
            CreateUI();
            LoadAllSprites();
            RefreshFilteredSprites();
        }

        /// <summary>
        /// Gets the currently selected sprites.
        /// </summary>
        public Sprite[] GetSelectedSprites()
        {
            return _listView.selectedItems?.Cast<Sprite>().ToArray() ?? new Sprite[0];
        }

        /// <summary>
        /// Sets the selected sprites programmatically.
        /// </summary>
        public void SetSelectedSprites(IEnumerable<Sprite> sprites)
        {
            if (sprites == null) return;

            List<Sprite> spritesToSelect = sprites
                .Where(predicate: s => _filteredSprites.Contains(item: s))
                .ToList();
            List<int> indices = spritesToSelect
                .Select(selector: s => _filteredSprites.IndexOf(item: s))
                .ToList();

            _listView.SetSelection(indices: indices);
        }

        /// <summary>
        /// Clears all selections.
        /// </summary>
        public void ClearSelection()
        {
            _listView.ClearSelection();
        }

        /// <summary>
        /// Refreshes the sprite list from the project.
        /// </summary>
        public void RefreshSprites()
        {
            LoadAllSprites();
            RefreshFilteredSprites();
        }

        void CreateUI()
        {
            style.backgroundColor = new Color(r: 0.2f, g: 0.2f, b: 0.2f, a: 0.95f);
            style.borderTopWidth = 1;
            style.borderBottomWidth = 1;
            style.borderLeftWidth = 1;
            style.borderRightWidth = 1;
            style.borderTopColor = Color.gray;
            style.borderBottomColor = Color.gray;
            style.borderLeftColor = Color.gray;
            style.borderRightColor = Color.gray;
            style.paddingTop = 10;
            style.paddingBottom = 10;
            style.paddingLeft = 10;
            style.paddingRight = 10;
            style.marginTop = 5;
            style.marginBottom = 5;

            CreateHeader();
            CreateSearchSection();
            CreateToolbarSection();
            CreateListViewSection();
            CreateButtonSection();
        }

        void CreateHeader()
        {
            Label headerLabel = new(text: "Select Sprites for Keyframes")
            {
                style =
                {
                    unityFontStyleAndWeight = FontStyle.Bold,
                    fontSize = 12,
                    marginBottom = 10,
                    unityTextAlign = TextAnchor.MiddleCenter
                }
            };
            Add(child: headerLabel);
        }

        void CreateSearchSection()
        {
            VisualElement searchContainer = new()
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    alignItems = Align.Center,
                    marginBottom = 5
                }
            };

            Label searchLabel = new(text: "Search:")
            {
                style =
                {
                    width = 50,
                    fontSize = 11
                }
            };
            searchContainer.Add(child: searchLabel);

            _searchField = new TextField
            {
                style =
                {
                    flexGrow = 1,
                    marginLeft = 5
                }
            };
            _searchField.RegisterValueChangedCallback(callback: OnSearchChanged);
            searchContainer.Add(child: _searchField);

            Add(child: searchContainer);
        }

        void CreateToolbarSection()
        {
            VisualElement toolbarContainer = new()
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    alignItems = Align.Center,
                    marginBottom = 5,
                    marginTop = 5
                }
            };

            _selectAllButton = new Button(clickEvent: OnSelectAllClicked)
            {
                text = "Select All",
                style =
                {
                    height = 20,
                    fontSize = 10,
                    marginRight = 2
                }
            };
            toolbarContainer.Add(child: _selectAllButton);

            _clearAllButton = new Button(clickEvent: OnClearAllClicked)
            {
                text = "Clear All",
                style =
                {
                    height = 20,
                    fontSize = 10,
                    marginRight = 10
                }
            };
            toolbarContainer.Add(child: _clearAllButton);

            // Spacer
            VisualElement spacer = new() { style = { flexGrow = 1 } };
            toolbarContainer.Add(child: spacer);

            _selectionCountLabel = new Label(text: "Selected: 0")
            {
                style =
                {
                    fontSize = 10,
                    color = Color.gray
                }
            };
            toolbarContainer.Add(child: _selectionCountLabel);

            Add(child: toolbarContainer);
        }

        void CreateListViewSection()
        {
            _listView = new ListView
            {
                selectionType = SelectionType.Multiple,
                showBorder = true,
                reorderable = false,
                fixedItemHeight = 20,
                style =
                {
                    height = 200,
                    marginBottom = 10,
                    borderTopWidth = 1,
                    borderBottomWidth = 1,
                    borderLeftWidth = 1,
                    borderRightWidth = 1,
                    borderTopColor = new Color(r: 0.5f, g: 0.5f, b: 0.5f),
                    borderBottomColor = new Color(r: 0.5f, g: 0.5f, b: 0.5f),
                    borderLeftColor = new Color(r: 0.5f, g: 0.5f, b: 0.5f),
                    borderRightColor = new Color(r: 0.5f, g: 0.5f, b: 0.5f)
                },
                makeItem = MakeListItem,
                bindItem = BindListItem
            };

            _listView.selectionChanged += OnListSelectionChanged;

            Add(child: _listView);
        }

        void CreateButtonSection()
        {
            VisualElement buttonContainer = new()
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    justifyContent = Justify.FlexEnd,
                    alignItems = Align.Center
                }
            };

            _cancelButton = new Button(clickEvent: OnCancelClicked)
            {
                text = "Cancel",
                style =
                {
                    height = 22,
                    width = 60,
                    marginRight = 5
                }
            };
            buttonContainer.Add(child: _cancelButton);

            _applyButton = new Button(clickEvent: OnApplyClicked)
            {
                text = "Apply",
                style =
                {
                    height = 22,
                    width = 60
                }
            };
            buttonContainer.Add(child: _applyButton);

            Add(child: buttonContainer);
        }

        VisualElement MakeListItem()
        {
            VisualElement container = new()
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    alignItems = Align.Center,
                    paddingLeft = 5,
                    paddingRight = 5
                }
            };

            // Sprite icon placeholder
            VisualElement iconContainer = new()
            {
                style =
                {
                    width = 16,
                    height = 16,
                    marginRight = 5,
                    backgroundColor = new Color(r: 0.3f, g: 0.3f, b: 0.3f)
                }
            };
            container.Add(child: iconContainer);

            // Sprite name label
            Label nameLabel = new()
            {
                style =
                {
                    flexGrow = 1,
                    fontSize = 11
                }
            };
            container.Add(child: nameLabel);

            return container;
        }

        void BindListItem(VisualElement element, int index)
        {
            if (index >= _filteredSprites.Count) return;

            Sprite sprite = _filteredSprites[index];
            Label nameLabel = element.Q<Label>();
            VisualElement iconContainer = element.Children().First();

            nameLabel.text = sprite.name;

            // Use the sprite texture directly instead of asset preview
            if (sprite.texture != null)
            {
                // Create a proper sprite background
                iconContainer.style.backgroundImage = new StyleBackground(sprite);
                iconContainer.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;
                iconContainer.style.backgroundColor = Color.clear;
            }
            else
            {
                iconContainer.style.backgroundImage = null;
                iconContainer.style.backgroundColor = new Color(0.3f, 0.3f, 0.3f);
            }
        }

        void LoadAllSprites()
        {
            _allSprites = new List<Sprite>();

            string[] guids = AssetDatabase.FindAssets("t:Sprite");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);

                if (IsFromUnityPackage(path))
                {
                    continue;
                }

                UnityEngine.Object[] allAssets = AssetDatabase.LoadAllAssetsAtPath(path);

                foreach (UnityEngine.Object asset in allAssets)
                {
                    if (asset is Sprite sprite)
                    {
                        _allSprites.Add(sprite);
                    }
                }
            }

            _allSprites.Sort((a, b) => string.Compare(a.name, b.name, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Checks if the asset path belongs to a Unity package.
        /// </summary>
        /// <param name="assetPath">The asset path to check</param>
        /// <returns>True if the asset is from a package, false otherwise</returns>
        static bool IsFromUnityPackage(string assetPath)
        {
            if (string.IsNullOrEmpty(value: assetPath))
                return false;

            // Check for common package paths
            return assetPath.StartsWith(value: "Packages/", comparisonType: StringComparison.OrdinalIgnoreCase)
                || assetPath.StartsWith(
                    value: "Library/PackageCache/",
                    comparisonType: StringComparison.OrdinalIgnoreCase
                );
        }

        void RefreshFilteredSprites()
        {
            if (string.IsNullOrEmpty(value: _currentFilter))
            {
                _filteredSprites = new List<Sprite>(collection: _allSprites);
            }
            else
            {
                _filteredSprites = _allSprites
                    .Where(predicate: sprite => sprite.name.ToLower().Contains(value: _currentFilter.ToLower()))
                    .ToList();
            }

            _listView.itemsSource = _filteredSprites;
            _listView.RefreshItems();
            UpdateSelectionCount();
        }

        void OnSearchChanged(ChangeEvent<string> evt)
        {
            _currentFilter = evt.newValue ?? "";
            RefreshFilteredSprites();
        }

        void OnSelectAllClicked()
        {
            List<int> allIndices = new List<int>();
            for (int i = 0; i < _filteredSprites.Count; i++)
            {
                allIndices.Add(item: i);
            }

            _listView.SetSelection(indices: allIndices);
        }

        void OnClearAllClicked()
        {
            _listView.ClearSelection();
        }

        void OnListSelectionChanged(IEnumerable<object> selectedItems)
        {
            UpdateSelectionCount();

            Sprite[] selectedSprites = selectedItems.Cast<Sprite>().ToArray();
            SelectionChanged?.Invoke(obj: selectedSprites);
        }

        void OnApplyClicked()
        {
            Sprite[] selectedSprites = GetSelectedSprites();
            if (selectedSprites.Length > 0)
            {
                SpritesApplied?.Invoke(obj: selectedSprites);
            }
        }

        void OnCancelClicked()
        {
            ClearSelection();
            CancelRequested?.Invoke();
        }

        void UpdateSelectionCount()
        {
            int count = _listView.selectedIndices?.Count() ?? 0;
            _selectionCountLabel.text = $"Selected: {count}";

            // Enable/disable apply button based on selection
            _applyButton.SetEnabled(value: count > 0);
        }
    }
}
