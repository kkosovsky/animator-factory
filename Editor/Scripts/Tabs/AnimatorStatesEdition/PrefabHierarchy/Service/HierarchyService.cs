using System.Collections.Generic;
using UnityEngine;

namespace AnimatorFactory.PrefabHierarchy
{
    /// <summary>
    /// Service responsible for building prefab hierarchies.
    /// Contains stateless logic for hierarchy generation.
    /// </summary>
    public static class HierarchyService
    {
        /// <summary>
        /// Builds a flat list representation of a prefab's hierarchy.
        /// </summary>
        /// <param name="selectedPrefab">The root prefab to analyze</param>
        /// <returns>List of hierarchy items with depth information</returns>
        public static List<PrefabHierarchyListItem> BuildHierarchy(GameObject selectedPrefab)
        {
            List<PrefabHierarchyListItem> hierarchyNodes = new List<PrefabHierarchyListItem>();
            BuildHierarchyRecursive(
                hierarchyNodes: hierarchyNodes,
                transform: selectedPrefab.transform,
                depth: 0
            );
            return hierarchyNodes;
        }

        /// <summary>
        /// Recursively builds the hierarchy by traversing the transform tree.
        /// </summary>
        /// <param name="hierarchyNodes">The list to add nodes to</param>
        /// <param name="transform">The current transform to process</param>
        /// <param name="depth">The current depth in the hierarchy</param>
        static void BuildHierarchyRecursive(
            List<PrefabHierarchyListItem> hierarchyNodes,
            Transform transform,
            int depth
        )
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
