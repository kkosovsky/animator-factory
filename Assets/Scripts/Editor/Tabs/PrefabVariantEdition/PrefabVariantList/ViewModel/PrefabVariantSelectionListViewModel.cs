using System.Collections.Generic;
using AnimatorFactory.Core.UI.SelectionList;
using UnityEngine;
using UnityEngine.UIElements;

namespace AnimatorFactory.PrefabVariants
{
    public class PrefabVariantSelectionListViewModel : ISelectionListViewModel<GameObject, GameObject>
    {
        public string currentFilter { get; }

        public List<GameObject> allItems { get; set; }
        public List<GameObject> filteredItems { get; }

        public PrefabVariantSelectionListViewModel()
        {
        }

        public void OnSourceItemChanged(GameObject item)
        {
        }

        public void BindItem(VisualElement element, int index)
        {
        }

        public void Sort(List<GameObject> items)
        {
        }

        public bool Filter(GameObject item)
        {
            return false;
        }

        public void LoadAllItems()
        {
        }

        public void OnSearchChanged(ChangeEvent<string> evt)
        {
        }

        public void RefreshAllFilteredItems()
        {
        }
    }
}
