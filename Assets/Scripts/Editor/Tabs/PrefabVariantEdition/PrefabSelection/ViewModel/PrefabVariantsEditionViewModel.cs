using System.IO;

namespace AnimatorFactory.PrefabVariants
{
    public class PrefabVariantsEditionViewModel
    {
        string spriteSourcePath = $"Assets{Path.DirectorySeparatorChar}";
        PrefabVariant[] _variants;

        public void SourceFolderChanged(string path) => spriteSourcePath = path;
        
        public void VariantsSelected(PrefabVariant[] variants) => _variants = variants;

        public void OnGenerateClicked()
        {
            foreach (PrefabVariant variant in _variants)
            {
                PrefabVariantsEditionService.CreateAnimatorOverrideControllerAsSubAsset(
                    prefabVariant: variant,
                    replacementSpritesPath: spriteSourcePath
                );
            }
        }
    }
}
