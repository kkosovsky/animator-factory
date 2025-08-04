using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AnimatorFactory
{
    public partial class AnimatorFactoryWindow
    {
        [SerializeField]
        GameObject _selectedPrefab;
        GameObject _lastSelectedPrefab;
        List<PrefabHierarchyListItem> _hierarchyNodes;
        
        PrefabHierarchyListItem _selectedHierarchyItem; 

        void DrawPrefabSelection()
        {
            EditorGUI.BeginChangeCheck();

            _selectedPrefab = (GameObject)EditorGUILayout.ObjectField(
                label: Strings.prefabSelectionLabel,
                obj: _selectedPrefab,
                objType: typeof(GameObject),
                allowSceneObjects: false
            );

            if (!EditorGUI.EndChangeCheck() && _selectedPrefab == _lastSelectedPrefab)
            {
                return;
            }

            if (_selectedPrefab == null)
            {
                _hierarchyNodes?.Clear();
                _listView.Reset();
                return;
            }

            _lastSelectedPrefab = _selectedPrefab;
            _hierarchyNodes = HierarchyBuilder.BuildHierarchy(selectedPrefab: _selectedPrefab);
            _listView.Refresh(hierarchyNodes: _hierarchyNodes);
        }

        void HierarchyListDidSelectItem(PrefabHierarchyListItem item)
        {
            _selectedHierarchyItem = item;
        }
    }
}
