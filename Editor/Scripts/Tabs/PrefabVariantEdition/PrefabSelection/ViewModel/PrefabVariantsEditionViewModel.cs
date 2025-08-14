namespace AnimatorFactory.PrefabVariants
{
    public class PrefabVariantsEditionViewModel
    {
        PrefabVariant[] _variants;

        public void VariantsSelected(PrefabVariant[] variants) => _variants = variants;

        public void OnGenerateClicked()
        {
            foreach (PrefabVariant variant in _variants)
            {
                PrefabVariantsEditionService.CreateAnimatorOverrideControllerAsSubAsset(
                    prefabVariant: variant
                );
            }
        }
    }
}
