using UnityEngine;

namespace AnimatorFactory
{
    /// <summary>
    /// Represents a single item in the prefab hierarchy tree.
    /// Core model shared across the application.
    /// </summary>
    public class PrefabHierarchyListItem
    {
        public readonly string name;
        public readonly int depth;
        public readonly GameObject gameObject;

        public PrefabHierarchyListItem(GameObject gameObject, int depth)
        {
            this.gameObject = gameObject;
            name = gameObject.name;
            this.depth = depth;
        }
    }
}