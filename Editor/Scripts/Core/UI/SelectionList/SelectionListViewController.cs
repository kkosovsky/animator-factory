using System;
using System.Collections.Generic;
using System.Linq;

namespace AnimatorFactory.Core.UI.SelectionList
{
    /// <summary>
    /// Custom UIElement for multi-selecting items from the project.
    /// Provides search, filtering, and multi-select functionality.
    /// </summary>
    public class SelectionListViewController<SourceItem, ListItem>
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

        public SelectionListView View => view;
        protected readonly SelectionListView view;
        protected readonly ISelectionListViewModel<SourceItem, ListItem> viewModel;

        protected SelectionListViewController(
            string headerText,
            ISelectionListViewModel<SourceItem, ListItem> viewModel,
            ListItemViewFactory itemViewFactory
        )
        {
            this.viewModel = viewModel;
            view = new SelectionListView(
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
            this.viewModel.DidFilterItems += OnDidFilterItems;
            this.viewModel.LoadAllItems();
            RefreshAllFilteredItems();
        }

        void OnDidFilterItems(List<ListItem> filteredItems)
        {
            view.list.itemsSource = filteredItems;
            view.list.RefreshItems();
        }

        /// <summary>
        /// Gets the currently selected items.
        /// </summary>
        public ListItem[] GetSelectedItems()
        {
            return view.list.selectedItems?.Cast<ListItem>().ToArray() ?? Array.Empty<ListItem>();
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
                .Where(predicate: s => viewModel.filteredItems.Contains(item: s))
                .ToList();
            List<int> indices = elementsToSelect
                .Select(selector: s => viewModel.filteredItems.IndexOf(item: s))
                .ToList();

            view.list.SetSelection(indices: indices);
        }

        /// <summary>
        /// Clears all selections.
        /// </summary>
        public void ClearSelection() => view.list.ClearSelection();

        /// <summary>
        /// Refreshes the items list from the project.
        /// </summary>
        public void RefreshItems()
        {
            viewModel.LoadAllItems();
            RefreshAllFilteredItems();
        }
        
        public void Show() => view.Show();

        public void Hide() => view.Hide();

        protected void RefreshList()
        {
            view.list.itemsSource = viewModel.filteredItems;
            view.list.RefreshItems();
            UpdateSelectionCount();
        }

        void RefreshAllFilteredItems()
        {
            viewModel.RefreshAllFilteredItems();
            RefreshList();
        }

        void OnSelectAllClicked()
        {
            List<int> allIndices = new List<int>();
            for (int i = 0; i < viewModel.filteredItems.Count; i++)
            {
                allIndices.Add(item: i);
            }

            view.list.SetSelection(indices: allIndices);
        }

        void OnClearAllClicked() => view.list.ClearSelection();

        void OnListSelectionChanged(IEnumerable<object> selectedItems)
        {
            UpdateSelectionCount();

            ListItem[] selectedSprites = selectedItems.Cast<ListItem>().ToArray();
            SelectionChanged?.Invoke(obj: selectedSprites);
        }

        void OnApplyClicked()
        {
            ListItem[] selectedItems = GetSelectedItems();
            if (selectedItems.Length > 0)
            {
                ItemsApplied?.Invoke(obj: selectedItems);
            }
        }

        void OnCancelClicked()
        {
            ClearSelection();
            CancelRequested?.Invoke();
        }

        void UpdateSelectionCount()
        {
            int count = view.list.selectedIndices?.Count() ?? 0;
            view.selectionCountLabel.text = $"Selected: {count}";
            view.applyButton.SetEnabled(value: count > 0);
        }
    }
}
