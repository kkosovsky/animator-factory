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
        public void LoadHierarchy(GameObject prefab)
        {
            if (prefab == null)
            {
                Clear();
                return;
            }

            try
            {
                List<PrefabHierarchyListItem> hierarchy = HierarchyService.BuildHierarchy(
                    selectedPrefab: prefab
                );
                _currentHierarchy = hierarchy;
                EmitCurrentHierarchy(filtered: false);
            }
            catch (Exception ex)
            {
                Debug.LogError(message: $"Error loading prefab hierarchy: {ex.Message}");
                Clear();
            }
        }

        public void FilterDidChange(bool shouldFilter) => EmitCurrentHierarchy(filtered: shouldFilter);

        /// <summary>
        /// Clears all hierarchy data.
        /// </summary>
        public void Clear()
        {
            _currentHierarchy.Clear();
            HierarchyChanged?.Invoke(obj: _currentHierarchy);
        }

        void EmitCurrentHierarchy(bool filtered)
        {
            if (!filtered)
            {
                HierarchyChanged?.Invoke(obj: _currentHierarchy);
                return;
            }

            List<PrefabHierarchyListItem> filteredHierarchy =_currentHierarchy
                .Where(predicate: item => item.gameObject.HasComponent<Animator>())
                .Select(selector: item => new PrefabHierarchyListItem(gameObject: item.gameObject, depth: 0))
                .ToList();
            
            HierarchyChanged?.Invoke(obj: filteredHierarchy);
        }
    }
}
