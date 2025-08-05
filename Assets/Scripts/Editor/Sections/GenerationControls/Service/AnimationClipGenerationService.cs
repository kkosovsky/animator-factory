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
            string fullPath = Path.Combine(path1: destinationFolderPath, path2: $"{animationName}.anim");
            AssetDatabase.CreateAsset(asset: clip, path: fullPath);
            Debug.Log(message: $"Created asset at: {fullPath}");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            // Load the clip from the asset database to ensure we have the proper asset reference
            AnimationClip loadedClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(assetPath: fullPath);
            if (loadedClip == null)
            {
                Debug.LogError(message: $"Failed to load animation clip from path: {fullPath}");
                return clip; // Return the original clip as fallback
            }
            
            Debug.Log(message: $"Successfully loaded animation clip from asset database: {loadedClip.name}");
            return loadedClip;
        }
    }
}
