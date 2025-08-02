using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AnimatorFactory
{
    public partial class AnimatorFactoryWindow
    {
        List<HierarchyNode> _hierarchyNodes = new List<HierarchyNode>();

        void DrawPrefabHierarchy()
        {
            if (_hierarchyNodes.Count == 0)
            {
                EditorGUILayout.HelpBox(message: Strings.noPrefabSelected, type: MessageType.Info);
                return;
            }

            EditorGUILayout.LabelField(label: Strings.hierarchyLabel, style: EditorStyles.boldLabel);
            DrawHierarchyNodes(startIndex: 0, maxDepth: -1);
        }

        int DrawHierarchyNodes(int startIndex, int maxDepth)
        {
            int currentIndex = startIndex;

            while (currentIndex < _hierarchyNodes.Count)
            {
                HierarchyNode node = _hierarchyNodes[index: currentIndex];

                if (DidTraverseBackToShallowerNode(nodeDepth: node.depth, maxDepth: maxDepth))
                {
                    break;
                }

                DrawHierarchyNode(node: node, nodeIndex: currentIndex);
                currentIndex++;

                bool hasChildren = HasChildren(nodeIndex: currentIndex - 1);
                bool isExpanded = node.isExpanded;

                currentIndex = (hasChildren, isExpanded) switch
                {
                    (hasChildren: true, isExpanded: true) => DrawHierarchyNodes(
                        startIndex: currentIndex,
                        maxDepth: node.depth
                    ),
                    (hasChildren: true, isExpanded: false) => SkipChildren(parentIndex: currentIndex - 1) + 1,
                    _ => currentIndex
                };
            }

            return currentIndex;
        }

        static bool DidTraverseBackToShallowerNode(int nodeDepth, int maxDepth) =>
            maxDepth >= 0 && nodeDepth <= maxDepth;

        void DrawHierarchyNode(HierarchyNode node, int nodeIndex)
        {
            EditorGUILayout.BeginHorizontal();

            GUILayout.Space(pixels: node.depth * Margins.hierarchyNodeSpacing);
            bool hasChildren = HasChildren(nodeIndex: nodeIndex);

            if (hasChildren)
            {
                node.isExpanded = DrawCustomFoldout(isExpanded: node.isExpanded);
            }
            else
            {
                GUILayout.Space(pixels: Margins.hierarchyNodeChildlessSpacing);
            }

            GUIContent content = new(
                text: node.name,
                image: AssetPreview.GetMiniThumbnail(obj: node.gameObject)
            );

            GUILayout.Label(content: content, options: GUILayout.Height(height: Layout.height16));
            EditorGUILayout.EndHorizontal();
        }

        bool HasChildren(int nodeIndex)
        {
            if (nodeIndex >= _hierarchyNodes.Count - 1)
                return false;

            HierarchyNode currentNode = _hierarchyNodes[index: nodeIndex];
            HierarchyNode nextNode = _hierarchyNodes[index: nodeIndex + 1];

            return nextNode.depth > currentNode.depth;
        }

        int SkipChildren(int parentIndex)
        {
            if (parentIndex >= _hierarchyNodes.Count - 1)
                return parentIndex;

            HierarchyNode parentNode = _hierarchyNodes[index: parentIndex];
            int lastChildIndex = parentIndex;

            for (int i = parentIndex + 1; i < _hierarchyNodes.Count; i++)
            {
                if (_hierarchyNodes[index: i].depth <= parentNode.depth)
                    break;

                lastChildIndex = i;
            }

            return lastChildIndex;
        }
    }
}
