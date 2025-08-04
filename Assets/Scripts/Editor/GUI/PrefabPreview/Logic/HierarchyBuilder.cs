using System.Collections.Generic;
using UnityEngine;

namespace AnimatorFactory
{
    public static class HierarchyBuilder
    {
        public static List<PrefabHierarchyListItem> BuildHierarchy(GameObject selectedPrefab)
        {
            List<PrefabHierarchyListItem> hierarchyNodes = new List<PrefabHierarchyListItem>();
            BuildHierarchyRecursive(hierarchyNodes: hierarchyNodes, transform: selectedPrefab.transform, depth: 0);
            return hierarchyNodes;
        }

        static void BuildHierarchyRecursive(List<PrefabHierarchyListItem> hierarchyNodes, Transform transform, int depth)
        {
            hierarchyNodes.Add(item: new PrefabHierarchyListItem(gameObject: transform.gameObject, depth: depth));

            for (int i = 0; i < transform.childCount; i++)
            {
                BuildHierarchyRecursive(
                    hierarchyNodes: hierarchyNodes,
                    transform: transform.GetChild(index: i),
                    depth: depth + 1
                );
            }
        }
    }
}
