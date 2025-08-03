using UnityEngine;

namespace AnimatorFactory
{
    public static class GameObjectExtensions
    {
        public static bool HasComponent<ComponentType>(this GameObject self)
            where ComponentType : Component => self.GetComponent<ComponentType>() != null;
    }
}
