using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AnimatorFactory.Core.UI
{
    /// <summary>
    /// A reusable tab view component that provides tabbed interface functionality.
    /// Allows adding multiple tabs with their own content and handles tab switching automatically.
    /// </summary>
    public class TabView : VisualElement
    {
        /// <summary>
        /// Fired when the active tab changes.
        /// </summary>
        public event Action<int> TabChanged;

        readonly VisualElement _tabButtonsContainer;
        readonly VisualElement _tabContentContainer;
        readonly List<TabData> _tabs;
        int _activeTabIndex = -1;

        /// <summary>
        /// Gets the index of the currently active tab.
        /// </summary>
        public int ActiveTabIndex => _activeTabIndex;

        /// <summary>
        /// Gets the total number of tabs.
        /// </summary>
        public int TabCount => _tabs.Count;

        /// <summary>
        /// Initializes a new instance of the TabView class.
        /// </summary>
        public TabView()
        {
            _tabs = new List<TabData>();

            style.flexGrow = 1;
            style.flexDirection = FlexDirection.Column;

            _tabButtonsContainer = CreateTabButtonsContainer();
            _tabContentContainer = CreateTabContentContainer();

            Add(child: _tabButtonsContainer);
            Add(child: _tabContentContainer);
        }

        /// <summary>
        /// Adds a new tab with the specified title and content.
        /// </summary>
        /// <param name="title">The title displayed on the tab button</param>
        /// <param name="content">The content to display when this tab is active</param>
        /// <returns>The index of the added tab</returns>
        public int AddTab(string title, VisualElement content)
        {
            if (string.IsNullOrEmpty(value: title))
                throw new ArgumentException(message: "Tab title cannot be null or empty", paramName: nameof(title));

            if (content == null)
                throw new ArgumentNullException(paramName: nameof(content));

            int newIndex = _tabs.Count;
            Button tabButton = CreateTabButton(text: title, onClick: () => ShowTab(tabIndex: newIndex));

            content.style.flexGrow = 1;
            content.style.display = DisplayStyle.None;

            TabData tabData = new TabData(title: title, button: tabButton, content: content);
            _tabs.Add(item: tabData);

            _tabButtonsContainer.Add(child: tabButton);
            _tabContentContainer.Add(child: content);

            if (_activeTabIndex == -1)
            {
                ShowTab(tabIndex: 0);
            }

            return newIndex;
        }

        /// <summary>
        /// Shows the tab at the specified index.
        /// </summary>
        /// <param name="tabIndex">The index of the tab to show</param>
        public void ShowTab(int tabIndex)
        {
            if (tabIndex < 0 || tabIndex >= _tabs.Count)
                throw new ArgumentOutOfRangeException(
                    paramName: nameof(tabIndex),
                    message: "Tab index is out of range"
                );

            if (_activeTabIndex == tabIndex)
                return;

            DeactivateAllTabs();
            ActivateTab(tabIndex: tabIndex);

            _activeTabIndex = tabIndex;
            TabChanged?.Invoke(obj: tabIndex);
        }

        /// <summary>
        /// Removes the tab at the specified index.
        /// </summary>
        /// <param name="tabIndex">The index of the tab to remove</param>
        public void RemoveTab(int tabIndex)
        {
            if (tabIndex < 0 || tabIndex >= _tabs.Count)
                throw new ArgumentOutOfRangeException(
                    paramName: nameof(tabIndex),
                    message: "Tab index is out of range"
                );

            TabData tabData = _tabs[index: tabIndex];

            _tabButtonsContainer.Remove(element: tabData.Button);
            _tabContentContainer.Remove(element: tabData.Content);
            _tabs.RemoveAt(index: tabIndex);

            if (_activeTabIndex == tabIndex)
            {
                _activeTabIndex = -1;
                if (_tabs.Count > 0)
                {
                    int newActiveIndex = Math.Min(val1: tabIndex, val2: _tabs.Count - 1);
                    ShowTab(tabIndex: newActiveIndex);
                }
            }
            else if (_activeTabIndex > tabIndex)
            {
                _activeTabIndex--;
            }
        }

        /// <summary>
        /// Gets the title of the tab at the specified index.
        /// </summary>
        /// <param name="tabIndex">The index of the tab</param>
        /// <returns>The title of the tab</returns>
        public string GetTabTitle(int tabIndex)
        {
            if (tabIndex < 0 || tabIndex >= _tabs.Count)
                throw new ArgumentOutOfRangeException(
                    paramName: nameof(tabIndex),
                    message: "Tab index is out of range"
                );

            return _tabs[index: tabIndex].Title;
        }

        /// <summary>
        /// Sets the title of the tab at the specified index.
        /// </summary>
        /// <param name="tabIndex">The index of the tab</param>
        /// <param name="title">The new title for the tab</param>
        public void SetTabTitle(int tabIndex, string title)
        {
            if (tabIndex < 0 || tabIndex >= _tabs.Count)
                throw new ArgumentOutOfRangeException(
                    paramName: nameof(tabIndex),
                    message: "Tab index is out of range"
                );

            if (string.IsNullOrEmpty(value: title))
                throw new ArgumentException(message: "Tab title cannot be null or empty", paramName: nameof(title));

            _tabs[index: tabIndex].Title = title;
            _tabs[index: tabIndex].Button.text = title;
        }

        VisualElement CreateTabButtonsContainer()
        {
            return new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    marginBottom = 5,
                    borderBottomWidth = 1,
                    borderBottomColor = Color.gray
                }
            };
        }

        VisualElement CreateTabContentContainer()
        {
            return new VisualElement
            {
                style =
                {
                    flexGrow = 1,
                    paddingTop = 10
                }
            };
        }

        Button CreateTabButton(string text, Action onClick)
        {
            return new Button(clickEvent: onClick)
            {
                text = text,
                style =
                {
                    paddingTop = 8,
                    paddingBottom = 8,
                    paddingLeft = 16,
                    paddingRight = 16,
                    marginRight = 2,
                    borderTopLeftRadius = 4,
                    borderTopRightRadius = 4,
                    borderBottomWidth = 0,
                    borderLeftWidth = 1,
                    borderRightWidth = 1,
                    borderTopWidth = 1,
                    borderLeftColor = Color.gray,
                    borderRightColor = Color.gray,
                    borderTopColor = Color.gray,
                    backgroundColor = new Color(r: 0.5f, g: 0.5f, b: 0.5f, a: 1f)
                }
            };
        }

        void DeactivateAllTabs()
        {
            foreach (TabData tab in _tabs)
            {
                tab.Button.style.backgroundColor = new Color(r: 0.3f, g: 0.3f, b: 0.3f, a: 1f);
                tab.Content.style.display = DisplayStyle.None;
            }
        }

        void ActivateTab(int tabIndex)
        {
            TabData tab = _tabs[index: tabIndex];
            tab.Button.style.backgroundColor = new Color(r: 0.3f, g: 0.3f, b: 0.3f, a: 1f);
            tab.Content.style.display = DisplayStyle.Flex;
        }

        /// <summary>
        /// Internal data structure to hold tab information.
        /// </summary>
        class TabData
        {
            public string Title { get; set; }
            public Button Button { get; }
            public VisualElement Content { get; }

            public TabData(string title, Button button, VisualElement content)
            {
                Title = title;
                Button = button;
                Content = content;
            }
        }
    }
}
