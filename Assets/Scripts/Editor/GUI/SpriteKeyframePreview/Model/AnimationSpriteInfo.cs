using System.Collections.Generic;
using UnityEngine;

namespace AnimatorFactory
{
    public readonly struct AnimationSpriteInfo
    {
        public readonly string animationName;
        public readonly float duration;
        public readonly float frameRate;
        public readonly List<SpriteKeyframeData> keyframes;
        public readonly int totalFrames;

        public AnimationSpriteInfo(AnimationClip clip)
        {
            animationName = clip.name;
            duration = clip.length;
            frameRate = clip.frameRate;
            totalFrames = Mathf.RoundToInt(duration * frameRate);
            keyframes = new List<SpriteKeyframeData>();
        }
    }
}
