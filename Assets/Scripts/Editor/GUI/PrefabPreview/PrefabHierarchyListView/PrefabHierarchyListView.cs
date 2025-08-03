using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace AnimatorFactory
{
    public partial class PrefabHierarchyListView : VisualElement
    {
        event Action<PrefabHierarchyListItem> DidSelectItem;
        
        PrefabHierarchyListItem _selectedItem;
        List<PrefabHierarchyListItem> _hierarchyNodes = new();
        readonly ListView _hierarchyListView;

        public PrefabHierarchyListView()
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

        public void Refresh(List<PrefabHierarchyListItem> hierarchyNodes)
        {
            if (_hierarchyListView == null || !hierarchyNodes.Any())
            {
                return;
            }
            
            _hierarchyNodes = hierarchyNodes;
            RefreshItems();
        }

        public void Reset()
        {
            _hierarchyNodes.Clear();
            EditorApplication.delayCall += RefreshItems;
        }

        public void AddListener(Action<PrefabHierarchyListItem> onSelectItem) => DidSelectItem += onSelectItem;

        public void RemoveAllListeners() => ClearDelegates();

        void RefreshItems()
        {
            _hierarchyListView.itemsSource = _hierarchyNodes;
            _hierarchyListView.RefreshItems();
        }

        void OnHierarchySelectionChanged(IEnumerable<object> selection)
        {
            PrefabHierarchyListItem selectedItem = selection.FirstOrDefault() as PrefabHierarchyListItem;
            _selectedItem = selectedItem;
            DidSelectItem?.Invoke(_selectedItem);

            if (selectedItem != null)
            {
                Debug.Log(
                    message:
                    $"Selected hierarchy item: {selectedItem.name} (GameObject: {selectedItem.gameObject.name})"
                );
            }
        }
        
        void ClearDelegates()
        {
            if (DidSelectItem == null)
            {
                return;
            }

            Delegate[] onSelectDelegates = DidSelectItem.GetInvocationList();
            foreach (Delegate @delegate in onSelectDelegates)
            {
                DidSelectItem -= (Action<PrefabHierarchyListItem>)@delegate;
            }
        }
    }
}
