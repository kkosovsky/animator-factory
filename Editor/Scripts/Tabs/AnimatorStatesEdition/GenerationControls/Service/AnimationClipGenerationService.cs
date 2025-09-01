using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace AnimatorFactory.GenerationControls
{
    public static class AnimationClipGenerationService
    {
        const string m_sprite = "m_Sprite";

        public static AnimationClip CreateAnimationClip(
            Sprite[] sprites,
            int keyframeCount,
            float frameRate,
            bool hasLoopTime,
            WrapMode wrapMode,
            string animationName,
            string destinationFolderPath
        )
        {
            string sanitizedDestinationPath = SanitizeDestinationPath(destinationPath: destinationFolderPath);
            string fullPath = Path.Combine(path1: sanitizedDestinationPath, path2: $"{animationName}.anim");

            // Check if animation clip already exists
            AnimationClip existingClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(assetPath: fullPath);
            if (existingClip != null)
            {
                return UpdateExistingAnimationClip(
                    existingClip: existingClip,
                    sprites: sprites,
                    keyframeCount: keyframeCount,
                    frameRate: frameRate,
                    hasLoopTime: hasLoopTime,
                    wrapMode: wrapMode
                );
            }

            // Create new clip if it doesn't exist
            AnimationClip clip = new()
            {
                frameRate = frameRate,
                wrapMode = wrapMode
            };

            AnimationClipSettings clipSettings = AnimationUtility.GetAnimationClipSettings(clip: clip);
            clipSettings.loopTime = hasLoopTime;
            AnimationUtility.SetAnimationClipSettings(clip: clip, srcClipInfo: clipSettings);

            EditorCurveBinding spriteBinding = new()
            {
                type = typeof(SpriteRenderer),
                path = string.Empty,
                propertyName = m_sprite
            };

            var spriteKeyFrames = new ObjectReferenceKeyframe[keyframeCount];

            for (int i = 0; i < keyframeCount; i++)
                spriteKeyFrames[i] = new ObjectReferenceKeyframe
                {
                    time = i / clip.frameRate,
                    value = sprites[i]
                };

            AnimationUtility.SetObjectReferenceCurve(clip: clip, binding: spriteBinding, keyframes: spriteKeyFrames);

            AssetDatabase.CreateAsset(asset: clip, path: fullPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            AnimationClip loadedClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(assetPath: fullPath);
            if (loadedClip == null)
            {
                Debug.LogError(message: $"Failed to load animation clip from path: {fullPath}");
                return clip;
            }

            return loadedClip;
        }

        public static AnimationClip UpdateExistingAnimationClip(
            AnimationClip existingClip,
            Sprite[] sprites,
            int keyframeCount,
            float frameRate,
            bool hasLoopTime,
            WrapMode wrapMode
        )
        {
            existingClip.frameRate = frameRate;
            existingClip.wrapMode = wrapMode;

            AnimationClipSettings clipSettings = AnimationUtility.GetAnimationClipSettings(clip: existingClip);
            clipSettings.loopTime = hasLoopTime;
            AnimationUtility.SetAnimationClipSettings(clip: existingClip, srcClipInfo: clipSettings);

            AnimationUtility.SetObjectReferenceCurves(
                clip: existingClip,
                bindings: Array.Empty<EditorCurveBinding>(),
                keyframes: Array.Empty<ObjectReferenceKeyframe[]>()
            );

            EditorCurveBinding spriteBinding = new()
            {
                type = typeof(SpriteRenderer),
                path = string.Empty,
                propertyName = m_sprite
            };

            var spriteKeyFrames = new ObjectReferenceKeyframe[keyframeCount];

            for (int i = 0; i < keyframeCount; i++)
                spriteKeyFrames[i] = new ObjectReferenceKeyframe
                {
                    time = i / existingClip.frameRate,
                    value = sprites[i]
                };

            AnimationUtility.SetObjectReferenceCurve(
                clip: existingClip,
                binding: spriteBinding,
                keyframes: spriteKeyFrames
            );

            EditorUtility.SetDirty(target: existingClip);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return existingClip;
        }

        /// <summary>
        /// Sanitizes the destination path to ensure it's a directory path, not a file path.
        /// This prevents issues where a full file path might be passed instead of just the directory.
        /// </summary>
        /// <param name="destinationPath">The destination path to sanitize</param>
        /// <returns>A clean directory path</returns>
        static string SanitizeDestinationPath(string destinationPath)
        {
            if (string.IsNullOrEmpty(value: destinationPath))
            {
                return $"Assets{Path.DirectorySeparatorChar}";
            }

            if (destinationPath.EndsWith(value: ".anim", comparisonType: System.StringComparison.OrdinalIgnoreCase))
            {
                return Path.GetDirectoryName(path: destinationPath) ?? $"Assets{Path.DirectorySeparatorChar}";
            }

            return destinationPath;
        }
    }
}
