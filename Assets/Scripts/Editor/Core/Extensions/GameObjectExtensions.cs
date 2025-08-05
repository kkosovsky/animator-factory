using UnityEngine;

namespace AnimatorFactory
{
    /// <summary>
    /// Extension methods for GameObject to provide common functionality.
    /// </summary>
    public static class GameObjectExtensions
    {
        /// <summary>
        /// Checks if the GameObject has a component of the specified type.
        /// </summary>
        /// <typeparam name="ComponentType">The type of component to check for</typeparam>
        /// <param name="self">The GameObject to check</param>
        /// <returns>True if the component exists, false otherwise</returns>
        public static bool HasComponent<ComponentType>(this GameObject self)
            where ComponentType : Component => self.GetComponent<ComponentType>() != null;
    }
}