using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AnimatorFactory.PrefabHierarchy
{
    /// <summary>
    /// Handles presentation logic and state management for prefab hierarchy.
    /// </summary>
    public class PrefabHierarchyViewModel
    {
        /// <summary>
        /// Fired when the hierarchy data changes.
        /// </summary>
        public event Action<List<PrefabHierarchyListItem>> HierarchyChanged;

        List<PrefabHierarchyListItem> _currentHierarchy = new();

        /// <summary>
        /// Loads the hierarchy for the given prefab.
        /// </summary>
        /// <param name="prefab">The prefab to analyze</param>
        /// <param name="onlyShowAnimatorGameObjects">If true only displays game objects with Animator Component added</param>
        public void LoadHierarchy(GameObject prefab, bool onlyShowAnimatorGameObjects)
        {
            if (prefab == null)
            {
                Clear();
                return;
            }

            try
            {
                List<PrefabHierarchyListItem> hierarchy = HierarchyService.BuildHierarchy(
                    selectedPrefab: prefab,
                    showDepth: !onlyShowAnimatorGameObjects
                );
                _currentHierarchy = onlyShowAnimatorGameObjects
                    ? hierarchy.Where(predicate: item => item.gameObject.HasComponent<Animator>()).ToList()
                    : hierarchy;

                HierarchyChanged?.Invoke(obj: _currentHierarchy);
            }
            catch (Exception ex)
            {
                Debug.LogError(message: $"Error loading prefab hierarchy: {ex.Message}");
                Clear();
            }
        }

        /// <summary>
        /// Clears all hierarchy data.
        /// </summary>
        public void Clear()
        {
            _currentHierarchy.Clear();
            HierarchyChanged?.Invoke(obj: _currentHierarchy);
        }
    }
}
