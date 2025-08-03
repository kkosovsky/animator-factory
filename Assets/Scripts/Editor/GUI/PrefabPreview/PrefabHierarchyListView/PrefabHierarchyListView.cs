using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace AnimatorFactory
{
    public partial class PrefabHierarchyListView : VisualElement
    {
        HierarchyNode _selectedHierarchyItem;
        List<HierarchyNode> _hierarchyNodes = new();
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

        public void Refresh(List<HierarchyNode> hierarchyNodes)
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

        void RefreshItems()
        {
            _hierarchyListView.itemsSource = _hierarchyNodes;
            _hierarchyListView.RefreshItems();
        }

        void OnHierarchySelectionChanged(IEnumerable<object> selection)
        {
            HierarchyNode selectedItem = selection.FirstOrDefault() as HierarchyNode;
            _selectedHierarchyItem = selectedItem;

            if (selectedItem != null)
            {
                Debug.Log(
                    message:
                    $"Selected hierarchy item: {selectedItem.name} (GameObject: {selectedItem.gameObject.name})"
                );
            }
        }
    }
}
