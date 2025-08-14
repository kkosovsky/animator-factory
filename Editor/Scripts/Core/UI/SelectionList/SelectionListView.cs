using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AnimatorFactory.Core.UI.SelectionList
{
    public class SelectionListView : VisualElement
    {
        public ListView list;
        public Label selectionCountLabel;
        public Button applyButton;
        public Button cancelButton;
        public Button selectAllButton;
        public Button clearAllButton;
        
        TextField searchField;

        public SelectionListView(
            string headerText,
            Action OnSelectAllClicked,
            EventCallback<ChangeEvent<string>> OnSearchChanged,
            Action OnClearAllClicked,
            Func<VisualElement> makeItem,
            Action<VisualElement, int> bindItem,
            Action<IEnumerable<object>> OnListSelectionChanged,
            Action OnApplyClicked,
            Action OnCancelClicked
        )
        {
            SetStyle();
            CreateHeader(headerText: headerText);
            CreateSearchSection(OnSearchChanged: OnSearchChanged);
            CreateToolbarSection(OnSelectAllClicked: OnSelectAllClicked, OnClearAllClicked: OnClearAllClicked);
            CreateListViewSection(
                makeItem: makeItem,
                bindItem: bindItem,
                OnListSelectionChanged: OnListSelectionChanged
            );
            CreateButtonSection(OnApplyClicked: OnApplyClicked, OnCancelClicked: OnCancelClicked);
        }

        public void Show() => style.display = DisplayStyle.Flex;

        public void Hide() => style.display = DisplayStyle.None;

        void SetStyle()
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
        }

        void CreateHeader(string headerText)
        {
            Label headerLabel = new(text: headerText)
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

        void CreateSearchSection(EventCallback<ChangeEvent<string>> OnSearchChanged)
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

            searchField = new TextField
            {
                style =
                {
                    flexGrow = 1,
                    marginLeft = 5
                }
            };

            searchContainer.Add(child: searchField);
            searchField.RegisterValueChangedCallback(callback: OnSearchChanged);

            Add(child: searchContainer);
        }

        void CreateToolbarSection(Action OnSelectAllClicked, Action OnClearAllClicked)
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

            selectAllButton = new Button(clickEvent: OnSelectAllClicked)
            {
                text = "Select All",
                style =
                {
                    height = 20,
                    fontSize = 10,
                    marginRight = 2
                }
            };
            toolbarContainer.Add(child: selectAllButton);

            clearAllButton = new Button(clickEvent: OnClearAllClicked)
            {
                text = "Clear All",
                style =
                {
                    height = 20,
                    fontSize = 10,
                    marginRight = 10
                }
            };
            toolbarContainer.Add(child: clearAllButton);

            VisualElement spacer = new() { style = { flexGrow = 1 } };
            toolbarContainer.Add(child: spacer);

            selectionCountLabel = new Label(text: "Selected: 0")
            {
                style =
                {
                    fontSize = 10,
                    color = Color.gray
                }
            };
            toolbarContainer.Add(child: selectionCountLabel);

            Add(child: toolbarContainer);
        }

        void CreateListViewSection(
            Func<VisualElement> makeItem,
            Action<VisualElement, int> bindItem,
            Action<IEnumerable<object>> OnListSelectionChanged
        )
        {
            list = new ListView
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
                makeItem = makeItem,
                bindItem = bindItem
            };

            list.selectionChanged += OnListSelectionChanged;

            Add(child: list);
        }

        void CreateButtonSection(Action OnApplyClicked, Action OnCancelClicked)
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

            cancelButton = new Button(clickEvent: OnCancelClicked)
            {
                text = "Cancel",
                style =
                {
                    height = 22,
                    width = 60,
                    marginRight = 5
                }
            };
            buttonContainer.Add(child: cancelButton);

            applyButton = new Button(clickEvent: OnApplyClicked)
            {
                text = "Apply",
                style =
                {
                    height = 22,
                    width = 60
                }
            };
            buttonContainer.Add(child: applyButton);

            Add(child: buttonContainer);
        }
    }
}
