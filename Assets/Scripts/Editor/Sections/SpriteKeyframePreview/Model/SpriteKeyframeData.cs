using UnityEngine;

namespace AnimatorFactory.SpriteKeyframePreview
{
    /// <summary>
    /// Represents a single sprite keyframe in an animation.
    /// </summary>
    public readonly struct SpriteKeyframeData
    {
        public readonly float time;
        public readonly Sprite sprite;
        public readonly string spriteName;
        public readonly int index;

        public SpriteKeyframeData(int index, float time, Sprite sprite)
        {
            this.index = index;
            this.time = time;
            this.sprite = sprite;
            spriteName = sprite != null ? sprite.name : "null";
        }
    }
}
