using System.Collections.Generic;
using AnimatorFactory.Core.UI.SelectionList;
using UnityEngine;
using UnityEngine.UIElements;

namespace AnimatorFactory.PrefabVariants
{
    public class PrefabVariantSelectionListViewModel: ISelectionListViewModel<GameObject>
    {
        public string currentFilter { get; }
        
        public List<GameObject> allItems { get; set; }
        public List<GameObject> filteredItems { get; }

        public PrefabVariantSelectionListViewModel()
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

        public void LoadAllItems() => throw new System.NotImplementedException();

        public void OnSearchChanged(ChangeEvent<string> evt) => throw new System.NotImplementedException();

        public void RefreshAllFilteredItems() => throw new System.NotImplementedException();
    }
}
