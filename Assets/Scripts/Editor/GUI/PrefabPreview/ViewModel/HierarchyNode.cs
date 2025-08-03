using UnityEngine;

namespace AnimatorFactory
{
    public class HierarchyNode
    {
        public readonly string name;
        public readonly int depth;
        public readonly GameObject gameObject;

        public HierarchyNode(GameObject gameObject, int depth)
        {
            this.gameObject = gameObject;
            name = gameObject.name;
            this.depth = depth;
        }
    }
}
