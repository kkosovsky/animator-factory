using System;
using System.Collections.Generic;
using System.Linq;
using AnimatorFactory.Core.UI.SelectionList;
using UnityEditor;
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
            currentFilter = string.Empty;
            allItems = new List<GameObject>();
            filteredItems = new List<GameObject>();
        }

        public void OnSourceItemChanged(GameObject item)
        {
            PrefabAssetType type = PrefabUtility.GetPrefabAssetType(componentOrGameObject: item);
            if (type != PrefabAssetType.Regular)
            {
                return;
            }

            Debug.Log(message: "Is regular prefab type");
            List<GameObject> variants = FindAllPrefabVariants(parent: item).ToList();
            Sort(items: variants);
            allItems = variants;
            foreach (GameObject anItem in allItems)
            {
                Debug.Log(message: anItem);
            }
        }

        public void BindItem(VisualElement element, int index)
        {
            
        }

        public void Sort(List<GameObject> items)
        {
            items.Sort(
                comparison: (a, b) => string.Compare(
                    strA: a.name,
                    strB: b.name,
                    comparisonType: StringComparison.OrdinalIgnoreCase
                )
            );
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

        public static IEnumerable<GameObject> FindAllPrefabVariants(GameObject parent)
        {
            return AssetDatabase
                .FindAssets(filter: "t:prefab")
                .Select(selector: AssetDatabase.GUIDToAssetPath)
                .Select(selector: AssetDatabase.LoadAssetAtPath<GameObject>)
                .Where(predicate: go => go != null)
                .Where(
                    predicate: go =>
                        PrefabUtility.GetPrefabAssetType(componentOrGameObject: go) == PrefabAssetType.Variant
                )
                .Where(
                    predicate: go => 
                        PrefabUtility.GetCorrespondingObjectFromSource(componentOrGameObject: go) == parent
                );
        }
    }
}
