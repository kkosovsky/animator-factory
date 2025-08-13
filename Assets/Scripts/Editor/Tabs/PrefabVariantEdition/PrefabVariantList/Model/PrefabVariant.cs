using UnityEngine;

namespace AnimatorFactory.PrefabVariants
{
    public class PrefabVariant
    {
        public string name => gameObject.name;
        public GameObject gameObject;

        public PrefabVariant(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }
    }
}
