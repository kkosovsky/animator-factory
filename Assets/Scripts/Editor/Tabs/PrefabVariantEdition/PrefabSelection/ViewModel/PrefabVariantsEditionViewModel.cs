using UnityEngine;

namespace AnimatorFactory.PrefabVariants
{
    public class PrefabVariantsEditionViewModel
    {
        string spriteSourcePath = string.Empty;
        GameObject _rootPrefab;
        GameObject[] _variants;

        public void SourceFolderChanged(string path) => spriteSourcePath = path;

        public void PrefabSelected(GameObject prefab) => _rootPrefab = prefab;

        public void VariantsSelected(GameObject[] variants) => _variants = variants;

        public void OnGenerateClicked()
        {
            Debug.Log($"Root Prefab: {_rootPrefab.name}");
            Debug.Log($"---------------------------------");
            foreach (GameObject variant in _variants)
            {
                Debug.Log($"Variant: {variant.name}");
            }
            Debug.Log($"---------------------------------");
            Debug.Log($"Destination");
        }
    }
}
