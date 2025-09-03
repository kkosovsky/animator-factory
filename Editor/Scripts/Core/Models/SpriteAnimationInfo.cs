using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AnimatorFactory
{
    /// <summary>
    /// Contains all sprite keyframe information for an animation clip.
    /// </summary>
    public readonly struct SpriteAnimationInfo
    {
        public readonly string animationName;
        public readonly float duration;
        public readonly float frameRate;
        public readonly List<SpriteAnimationKeyframe> keyframes;
        public readonly int totalFrames;
        public readonly string destinationFolderPath;

        public SpriteAnimationInfo(AnimationClip clip)
        {
            animationName = clip.name;
            duration = clip.length;
            frameRate = clip.frameRate;
            totalFrames = Mathf.RoundToInt(f: duration * frameRate);
            keyframes = new List<SpriteAnimationKeyframe>();
            
            string fullPath = AssetDatabase.GetAssetPath(assetObject: clip);
            destinationFolderPath = System.IO.Path.GetDirectoryName(path: fullPath);
        }

        public SpriteAnimationInfo(
            string animationName,
            float duration,
            float frameRate,
            int totalFrames,
            List<SpriteAnimationKeyframe> keyframes,
            string destinationFolderPath
        )
        {
            this.animationName = animationName;
            this.duration = duration;
            this.frameRate = frameRate;
            this.totalFrames = totalFrames;
            this.keyframes = keyframes;
            this.destinationFolderPath = destinationFolderPath;
        }

        public SpriteAnimationInfo WithName(string name)
        {
            return new SpriteAnimationInfo(
                animationName: name,
                duration: duration,
                frameRate: frameRate,
                totalFrames: totalFrames,
                keyframes: keyframes,
                destinationFolderPath: destinationFolderPath
            );
        }
        
        public SpriteAnimationInfo WithDestinationFolderPath(string destinationFolderPath)
        {
            return new SpriteAnimationInfo(
                animationName: animationName,
                duration: duration,
                frameRate: frameRate,
                totalFrames: totalFrames,
                keyframes: keyframes,
                destinationFolderPath: destinationFolderPath
            );
        }
    }
}
