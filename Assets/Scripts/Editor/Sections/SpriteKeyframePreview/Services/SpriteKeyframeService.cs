using UnityEditor;
using UnityEngine;

namespace AnimatorFactory.SpriteKeyframePreview
{
    /// <summary>
    /// Service responsible for extracting sprite keyframe data from animation clips.
    /// Contains stateless business logic for sprite analysis.
    /// </summary>
    public static class SpriteKeyframeService
    {
        /// <summary>
        /// Extracts all sprite keyframes from an animation clip.
        /// </summary>
        /// <param name="clip">The animation clip to analyze</param>
        /// <returns>Animation sprite information with all keyframes</returns>
        public static AnimationSpriteInfo ExtractSpriteKeyframes(AnimationClip clip)
        {
            AnimationSpriteInfo info = new(clip: clip);

            EditorCurveBinding[] bindings = AnimationUtility.GetObjectReferenceCurveBindings(clip: clip);

            foreach (EditorCurveBinding binding in bindings)
            {
                // Look for SpriteRenderer.sprite bindings (classID 212 is SpriteRenderer)
                if (binding.type != typeof(SpriteRenderer) || binding.propertyName != "m_Sprite")
                {
                    continue;
                }

                ObjectReferenceKeyframe[] keyframes =
                    AnimationUtility.GetObjectReferenceCurve(clip: clip, binding: binding);

                foreach (ObjectReferenceKeyframe keyframe in keyframes)
                {
                    Sprite sprite = keyframe.value as Sprite;
                    info.keyframes.Add(item: new SpriteKeyframeData(time: keyframe.time, sprite: sprite));
                }
            }

            // Sort by time
            info.keyframes.Sort(comparison: (a, b) => a.time.CompareTo(value: b.time));

            return info;
        }
    }
}
