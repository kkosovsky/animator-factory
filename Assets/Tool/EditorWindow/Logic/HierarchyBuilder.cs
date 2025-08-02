using System.Collections.Generic;
using UnityEngine;

namespace AnimatorFactory.Logic
{
    public static class HierarchyBuilder
    {
        public static List<HierarchyNode> BuildHierarchy(GameObject selectedPrefab)
        {
            List<HierarchyNode> hierarchyNodes = new List<HierarchyNode>();
            BuildHierarchyRecursive(hierarchyNodes: hierarchyNodes, transform: selectedPrefab.transform, depth: 0);
            return hierarchyNodes;
        }

        static void BuildHierarchyRecursive(List<HierarchyNode> hierarchyNodes, Transform transform, int depth)
        {
            hierarchyNodes.Add(item: new HierarchyNode(gameObject: transform.gameObject, depth: depth));

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
