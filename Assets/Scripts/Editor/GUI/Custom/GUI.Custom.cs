using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace AnimatorFactory
{
    public static class Custom
    {
        public static bool DrawFoldout(bool isExpanded)
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

        public static void DrawIcon<ComponentType>(GameObject gameObject, Vector2 labelSize, string tooltip)
            where ComponentType : Component
        {
            if (gameObject.GetComponent<ComponentType>() == null)
            {
                return;
            }

            Texture2D componentTexture =
                EditorGUIUtility.ObjectContent(obj: null, type: typeof(ComponentType)).image as Texture2D;

            if (componentTexture == null)
            {
                return;
            }

            GUIContent content = new(image: componentTexture, tooltip: tooltip);
            GUILayout.Label(
                content: content,
                options: new[] { GUILayout.Width(width: labelSize.x), GUILayout.Height(height: labelSize.y) }
            );
        }
        
        public static void AddComponentIcon<T>(VisualElement container, string tooltip)
            where T : Component
        {
            Texture componentIcon = EditorGUIUtility.ObjectContent(obj: null, type: typeof(T)).image;
            if (componentIcon == null)
            {
                return;
            }

            Image iconImage = new()
            {
                image = componentIcon,
                tooltip = tooltip,
                style =
                {
                    width = 16,
                    height = 16,
                    marginLeft = 2
                }
            };
            
            container.Add(child: iconImage);
        }
    }
}
