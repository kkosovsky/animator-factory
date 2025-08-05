using System.Collections.Generic;
using UnityEditor;
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
        public readonly string path;

        public AnimationSpriteInfo(AnimationClip clip)
        {
            animationName = clip.name;
            duration = clip.length;
            frameRate = clip.frameRate;
            totalFrames = Mathf.RoundToInt(f: duration * frameRate);
            keyframes = new List<SpriteKeyframeData>();
            path = AssetDatabase.GetAssetPath(assetObject: clip);
        }

        public AnimationSpriteInfo(
            string animationName,
            float duration,
            float frameRate,
            int totalFrames,
            List<SpriteKeyframeData> keyframes,
            string path
        )
        {
            this.animationName = animationName;
            this.duration = duration;
            this.frameRate = frameRate;
            this.totalFrames = totalFrames;
            this.keyframes = keyframes;
            this.path = path;
        }

        public AnimationSpriteInfo WithName(string name)
        {
            return new AnimationSpriteInfo(
                animationName: name,
                duration: duration,
                frameRate: frameRate,
                totalFrames: totalFrames,
                keyframes: keyframes,
                path: path
            );
        }
        
        public AnimationSpriteInfo WithDestinationFolderPath(string destinationFolderPath)
        {
            return new AnimationSpriteInfo(
                animationName: animationName,
                duration: duration,
                frameRate: frameRate,
                totalFrames: totalFrames,
                keyframes: keyframes,
                path: $"{destinationFolderPath}{animationName}.anim"
            );
        }
    }
}
