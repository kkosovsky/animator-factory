using UnityEngine;

namespace AnimatorFactory.PrefabVariants
{
    public class PrefabVariantsEditionViewModel
    {
        string destinationPath = string.Empty;
        GameObject _rootPrefab;

        public void DestinationChanged(string path)
        {
            destinationPath = path;
        }

        public void PrefabSelected(GameObject prefab)
        {
            _rootPrefab = prefab;
        }
    }
}
