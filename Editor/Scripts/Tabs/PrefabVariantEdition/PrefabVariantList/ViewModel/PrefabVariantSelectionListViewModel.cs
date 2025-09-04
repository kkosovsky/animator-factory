using System;
using System.Collections.Generic;
using System.Linq;
using AnimatorFactory.Core.UI.SelectionList;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace AnimatorFactory.PrefabVariants
{
    public class PrefabVariantSelectionListViewModel : ISelectionListViewModel<GameObject, PrefabVariant>
    {
        public event Action<List<PrefabVariant>> DidFilterItems;
        public List<PrefabVariant> allItems { get; set; } = new();
        public List<PrefabVariant> filteredItems { get; } = new();

        string _currentFilter = string.Empty;

        public void OnSourceItemChanged(GameObject item)
        {
            if (item == null)
            {
                return;
            }

            List<PrefabVariant> variants = FindAllPrefabVariants(parent: item)
                .Select(selector: gameObject => new PrefabVariant(gameObject: gameObject))
                .ToList();

            Sort(items: variants);
            allItems = variants;
            RefreshAllFilteredItems();
        }

        public void BindItem(VisualElement element, int index)
        {
            if (element is not PrefabVariantCell cell || index >= filteredItems.Count)
            {
                return;
            }

            PrefabVariant variant = filteredItems[index: index];
            Texture2D previewTexture = AssetPreview.GetAssetPreview(asset: variant.gameObject);
            cell.SetUp(
                id: variant.id,
                image: previewTexture,
                labelText: variant.name,
                initialSpritesSourceDir: variant.spriteSourcesDirPath,
                initialClipsDestinationDir: variant.generatedClipsPath,
                onSpritesSourceDirChanged: UpdateVariantSpriteSourceDir,
                onClipsDestinationPathDirChanged: UpdateClipsDestinationDir
            );
        }

        public void Sort(List<PrefabVariant> items)
        {
            items.Sort(
                comparison: (a, b) => string.Compare(
                    strA: a.name,
                    strB: b.name,
                    comparisonType: StringComparison.OrdinalIgnoreCase
                )
            );
        }

        public bool Filter(PrefabVariant item) =>
            string.IsNullOrEmpty(value: _currentFilter)
            || item.name.Contains(
                value: _currentFilter,
                comparisonType: StringComparison.OrdinalIgnoreCase
            );

        public void LoadAllItems()
        {
        }

        public void OnSearchChanged(ChangeEvent<string> evt)
        {
            _currentFilter = evt.newValue;
            RefreshAllFilteredItems();
        }

        public void RefreshAllFilteredItems()
        {
            filteredItems.Clear();
            filteredItems.AddRange(collection: allItems.Where(predicate: Filter));
            DidFilterItems?.Invoke(obj: filteredItems);
        }

        void UpdateClipsDestinationDir(Guid id, string path)
        {
            if (!allItems.Any())
            {
                return;
            }

            allItems
                .First(predicate: variant => variant.id == id)
                .generatedClipsPath = path;
        }

        void UpdateVariantSpriteSourceDir(Guid id, string path)
        {
            if (!allItems.Any())
            {
                return;
            }

            allItems
                .First(predicate: variant => variant.id == id)
                .spriteSourcesDirPath = path;
        }

        public static IEnumerable<GameObject> FindAllPrefabVariants(GameObject parent)
        {
            IEnumerable<GameObject> variants = AssetDatabase
                .FindAssets(filter: "t:prefab")
                .Select(selector: AssetDatabase.GUIDToAssetPath)
                .Select(selector: AssetDatabase.LoadAssetAtPath<GameObject>)
                .Where(predicate: go => go != null)
                .Where(
                    predicate: go =>
                        PrefabUtility.GetPrefabAssetType(componentOrGameObject: go) == PrefabAssetType.Variant
                );

            PrefabAssetType type = PrefabUtility.GetPrefabAssetType(componentOrGameObject: parent);
            return type switch
            {
                PrefabAssetType.Regular => GetVariantsForRegularPrefab(parent: parent, variants: variants),
                PrefabAssetType.Variant => GetVariantsForVariant(parent: parent, prefabs: variants),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        static IEnumerable<GameObject> GetVariantsForRegularPrefab(GameObject parent, IEnumerable<GameObject> variants)
        {
            return variants
                .Where(
                    predicate: go =>
                        PrefabUtility.GetCorrespondingObjectFromSource(componentOrGameObject: go) == parent
                );
        }

        static IEnumerable<GameObject> GetVariantsForVariant(GameObject parent, IEnumerable<GameObject> prefabs)
        {
            return prefabs
                .Where(predicate: go => IsDirectVariantOf(variant: go, potentialParent: parent));
        }

        static bool IsDirectVariantOf(GameObject variant, GameObject potentialParent)
        {
            string parentPath = AssetDatabase.GetAssetPath(assetObject: potentialParent);
            string[] parentDependencies = AssetDatabase.GetDependencies(pathName: parentPath, recursive: false);

            string variantPath = AssetDatabase.GetAssetPath(assetObject: variant);
            string[] variantDependencies = AssetDatabase.GetDependencies(pathName: variantPath, recursive: false);

            return variantDependencies.Intersect(parentDependencies).Count() == parentDependencies.Length;
        }
    }
}
