using UnityEngine;

namespace AnimatorFactory
{
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
