using UnityEngine;

namespace AnimatorFactory
{
    [CreateAssetMenu(fileName = "AnimatorFactoryConfiguration", menuName = "Animator Factory/Configuration")]
    public class Configuration : ScriptableObject
    {
        [field: SerializeField] public string DefaultSourceSpritePath { get; private set; }
        [field: SerializeField] public string GeneratedClipsPath { get; private set; }
    }
}
