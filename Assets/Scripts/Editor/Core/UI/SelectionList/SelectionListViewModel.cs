using System.Collections.Generic;
using UnityEngine.UIElements;

namespace AnimatorFactory.Core.UI.SelectionList
{
    public interface ISelectionListViewModel<SourceItem, ListItem>
    {
        List<ListItem> allItems { get; set; }
        List<ListItem> filteredItems { get; }

        void BindItem(VisualElement element, int index);

        void Sort(List<ListItem> items);

        bool Filter(ListItem item);

        void OnSourceItemChanged(SourceItem item);
        
        void LoadAllItems();

        void OnSearchChanged(ChangeEvent<string> evt);

        void RefreshAllFilteredItems();
    }
}
