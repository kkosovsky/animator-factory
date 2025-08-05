using System.Collections.Generic;
using UnityEngine;

namespace AnimatorFactory.SpriteKeyframePreview
{
    /// <summary>
    /// Contains all sprite keyframe information for an animation clip.
    /// </summary>
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
            totalFrames = Mathf.RoundToInt(f: duration * frameRate);
            keyframes = new List<SpriteKeyframeData>();
        }

        public AnimationSpriteInfo(
            string animationName,
            float duration,
            float frameRate,
            int totalFrames,
            List<SpriteKeyframeData> keyframes
        )
        {
            this.animationName = animationName;
            this.duration = duration;
            this.frameRate = frameRate;
            this.totalFrames = totalFrames;
            this.keyframes = keyframes;
        }
    }
}
