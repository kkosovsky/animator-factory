using UnityEditor;
using UnityEngine;

namespace AnimatorFactory
{
    public partial class AnimatorFactoryWindow
    {
        Vector2 _scrollPosition;

        void OnGUI()
        {
            _scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition: _scrollPosition);

            for (int i = 0; i < 50; i++)
            {
                EditorGUILayout.LabelField(label: $"Item {i}");
            }

            EditorGUILayout.EndScrollView();
        }
    }
}
