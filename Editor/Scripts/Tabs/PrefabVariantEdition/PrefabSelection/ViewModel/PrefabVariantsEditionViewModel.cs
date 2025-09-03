using System;
using System.Linq;
using UnityEngine;

namespace AnimatorFactory.PrefabVariants
{
    public class PrefabVariantsEditionViewModel
    {
        PrefabVariant[] _variants = Array.Empty<PrefabVariant>();
        PrefabHierarchyListItem _selectedHierarchyItem = null;

        public void VariantsSelected(PrefabVariant[] variants) => _variants = variants;

        public void OnGenerateClicked()
        {
            if (!_variants.Any() || _selectedHierarchyItem == null)
            {
                Debug.LogError(
                    message:
                    $"PrefabVariantsEditionViewModel does not have all required data! Selected variants count: {_variants.Length}, is selected hierarchy item null? {_selectedHierarchyItem == null}"
                );
                return;
            }

            foreach (PrefabVariant variant in _variants)
            {
                PrefabVariantsEditionService.CreateAnimatorOverrideControllerAsSubAsset(
                    originalAnimatorGameObject: _selectedHierarchyItem,
                    prefabVariant: variant
                );
            }
        }

        public void DidSelectHierarchyItem(PrefabHierarchyListItem item)
        {
            _selectedHierarchyItem = item;
        }
    }
}
