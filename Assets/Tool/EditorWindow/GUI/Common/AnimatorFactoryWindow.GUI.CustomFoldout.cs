using UnityEditor;
using UnityEngine;

namespace AnimatorFactory
{
    public partial class AnimatorFactoryWindow
    {
        bool DrawCustomFoldout(bool isExpanded)
        {
            string chevron = isExpanded ? "▼" : "▶";

            GUIStyle foldoutStyle = new(other: EditorStyles.label)
            {
                fontSize = 10,
                padding = new RectOffset(left: 0, right: 0, top: 0, bottom: 0),
                margin = new RectOffset(left: 0, right: 0, top: 0, bottom: 0)
            };

            if (
                GUILayout.Button(
                    text: chevron,
                    style: foldoutStyle,
                    options: new[] { GUILayout.Width(width: 16f), GUILayout.Height(height: 16f) }
                )
            )
            {
                isExpanded = !isExpanded;
            }

            return isExpanded;
        }
    }
}
