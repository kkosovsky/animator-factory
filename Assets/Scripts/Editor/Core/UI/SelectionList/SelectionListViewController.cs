using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

namespace AnimatorFactory.Core.UI.SelectionList
{
    /// <summary>
    /// Custom UIElement for multi-selecting items from the project.
    /// Provides search, filtering, and multi-select functionality.
    /// </summary>
    public class SelectionListViewController<ListItem> : VisualElement
    {
        /// <summary>
        /// Fired when items selection changes.
        /// </summary>
        public event Action<ListItem[]> SelectionChanged;

        /// <summary>
        /// Fired when items are confirmed/applied.
        /// </summary>
        public event Action<ListItem[]> ItemsApplied;

        /// <summary>
        /// Fired when cancel is requested.
        /// </summary>
        public event Action CancelRequested;

        readonly SelectionListView _listView;
        readonly ISelectionListViewModel<ListItem> _viewModel;

        public SelectionListViewController(
            string headerText,
            ISelectionListViewModel<ListItem> viewModel,
            ListItemViewFactory itemViewFactory
        )
        {
            _viewModel = viewModel;
            _listView = new SelectionListView(
                headerText: headerText,
                OnSelectAllClicked: OnSelectAllClicked,
                OnSearchChanged: viewModel.OnSearchChanged,
                OnClearAllClicked: OnClearAllClicked,
                makeItem: itemViewFactory.MakeListItem,
                bindItem: viewModel.BindItem,
                OnListSelectionChanged: OnListSelectionChanged,
                OnApplyClicked: OnApplyClicked,
                OnCancelClicked: OnCancelClicked
            );
            _viewModel.LoadAllItems();
            RefreshAllFilteredItems();
        }

        /// <summary>
        /// Gets the currently selected items.
        /// </summary>
        public ListItem[] GetSelectedItems()
        {
            return _listView.list.selectedItems?.Cast<ListItem>().ToArray() ?? Array.Empty<ListItem>();
        }

        /// <summary>
        /// Sets the selected items programmatically.
        /// </summary>
        public void SetSelectedItems(IEnumerable<ListItem> items)
        {
            if (items == null)
            {
                return;
            }

            List<ListItem> elementsToSelect = items
                .Where(predicate: s => _viewModel.filteredItems.Contains(item: s))
                .ToList();
            List<int> indices = elementsToSelect
                .Select(selector: s => _viewModel.filteredItems.IndexOf(item: s))
                .ToList();

            _listView.list.SetSelection(indices: indices);
        }

        /// <summary>
        /// Clears all selections.
        /// </summary>
        public void ClearSelection() => _listView.list.ClearSelection();

        /// <summary>
        /// Refreshes the items list from the project.
        /// </summary>
        public void RefreshItems()
        {
            _viewModel.LoadAllItems();
            RefreshAllFilteredItems();
        }

        void RefreshAllFilteredItems()
        {
            // if (string.IsNullOrEmpty(value: _filter.currentFilter))
            // {
            //     _filteredItems = new List<ListItem>(collection: _allItems);
            // }
            // else
            // {
            //     _filteredItems = _allItems
            //         .Where(predicate: _filter.Filter)
            //         .ToList();
            // }

            _viewModel.RefreshAllFilteredItems();
            _listView.list.itemsSource = _viewModel.filteredItems;
            _listView.list.RefreshItems();
            UpdateSelectionCount();
        }

        // void OnSearchChanged(ChangeEvent<string> evt)
        // {
        //     _filter.currentFilter = evt.newValue ?? "";
        //     RefreshAllFilteredItems();
        // }

        void OnSelectAllClicked()
        {
            List<int> allIndices = new List<int>();
            for (int i = 0; i < _viewModel.filteredItems.Count; i++)
            {
                allIndices.Add(item: i);
            }
            
            _listView.list.SetSelection(indices: allIndices);
        }

        void OnClearAllClicked() => _listView.list.ClearSelection();

        void OnListSelectionChanged(IEnumerable<object> selectedItems)
        {
            UpdateSelectionCount();

            ListItem[] selectedSprites = selectedItems.Cast<ListItem>().ToArray();
            SelectionChanged?.Invoke(obj: selectedSprites);
        }

        void OnApplyClicked()
        {
            ListItem[] selectedSprites = GetSelectedItems();
            if (selectedSprites.Length > 0)
            {
                ItemsApplied?.Invoke(obj: selectedSprites);
            }
        }

        void OnCancelClicked()
        {
            ClearSelection();
            CancelRequested?.Invoke();
        }

        void UpdateSelectionCount()
        {
            int count = _listView.list.selectedIndices?.Count() ?? 0;
            _listView.selectionCountLabel.text = $"Selected: {count}";
            _listView.applyButton.SetEnabled(value: count > 0);
        }
    }
}
