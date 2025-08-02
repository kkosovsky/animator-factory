using AnimatorFactory.Logic;
using UnityEditor;
using UnityEngine;

namespace AnimatorFactory
{
    public partial class AnimatorFactoryWindow
    {
        [SerializeField]
        GameObject _selectedPrefab;

        GameObject _lastSelectedPrefab;

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
                return;
            }
            
            _lastSelectedPrefab = _selectedPrefab;
            _hierarchyNodes = HierarchyBuilder.BuildHierarchy(selectedPrefab: _selectedPrefab);
        }
    }
}
