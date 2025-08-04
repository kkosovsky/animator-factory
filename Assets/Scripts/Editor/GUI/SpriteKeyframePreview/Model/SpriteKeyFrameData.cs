using UnityEngine;

namespace AnimatorFactory
{
    public readonly struct SpriteKeyframeData
    {
        public readonly float time;
        public readonly Sprite sprite;
        public readonly string spriteName;

        public SpriteKeyframeData(float time, Sprite sprite)
        {
            this.time = time;
            this.sprite = sprite;
            spriteName = sprite != null ? sprite.name : "null";
        }
    }
}
