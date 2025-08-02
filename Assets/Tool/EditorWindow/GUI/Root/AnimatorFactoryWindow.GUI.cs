using UnityEditor;
using UnityEngine;

namespace AnimatorFactory
{
    public partial class AnimatorFactoryWindow
    {
        Vector2 _scrollPosition;

        void OnGUI()
        {
            _serializedObject.Update();
            _scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition: _scrollPosition);
            DrawPrefabSelection();

            if (_selectedPrefab != null)
            {
                EditorGUILayout.Space();
                DrawPrefabHierarchy();
            }

            EditorGUILayout.EndScrollView();
            _serializedObject.ApplyModifiedProperties();
        }
    }
}
