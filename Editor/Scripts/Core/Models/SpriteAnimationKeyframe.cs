using UnityEngine;

namespace AnimatorFactory
{
    /// <summary>
    /// Represents a single sprite keyframe in an animation.
    /// </summary>
    public readonly struct SpriteAnimationKeyframe
    {
        public readonly float time;
        public readonly Sprite sprite;
        public readonly string imageName;
        public readonly int index;

        public SpriteAnimationKeyframe(int index, float time, Sprite sprite)
        {
            this.index = index;
            this.time = time;
            this.sprite = sprite;
            imageName = sprite != null ? sprite.name : "null";
        }
    }
}
