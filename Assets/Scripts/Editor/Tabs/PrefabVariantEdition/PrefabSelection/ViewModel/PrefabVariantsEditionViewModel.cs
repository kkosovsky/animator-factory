using System.IO;
using UnityEngine;

namespace AnimatorFactory.PrefabVariants
{
    public class PrefabVariantsEditionViewModel
    {
        string spriteSourcePath = $"Assets{Path.DirectorySeparatorChar}";
        GameObject _rootPrefab;
        GameObject[] _variants;

        public void SourceFolderChanged(string path) => spriteSourcePath = path;

        public void PrefabSelected(GameObject prefab) => _rootPrefab = prefab;

        public void VariantsSelected(GameObject[] variants) => _variants = variants;

        public void OnGenerateClicked()
        {
            foreach (GameObject variant in _variants)
            {
                PrefabVariantsEditionService.CreateAnimatorOverrideControllerAsSubAsset(
                    prefabVariant: variant,
                    replacementSpritesPath: spriteSourcePath
                );
            }
        }
    }
}
