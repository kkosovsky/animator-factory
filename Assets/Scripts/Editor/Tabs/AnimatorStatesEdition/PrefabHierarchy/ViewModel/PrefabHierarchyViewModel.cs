using System;
using System.Collections.Generic;
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
                List<PrefabHierarchyListItem> hierarchy = HierarchyService.BuildHierarchy(selectedPrefab: prefab);
                _currentHierarchy = hierarchy;
                HierarchyChanged?.Invoke(obj: hierarchy);
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
