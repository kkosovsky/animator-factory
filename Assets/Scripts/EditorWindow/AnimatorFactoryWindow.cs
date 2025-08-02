using UnityEditor;
using UnityEngine;

namespace AnimatorFactory
{
    public partial class AnimatorFactoryWindow : EditorWindow
    {
        SerializedObject _serializedObject;

        [MenuItem(itemName: Strings.menuItemName)]
        public static void ShowWindow()
        {
            GetWindow<AnimatorFactoryWindow>(title: Strings.windowTitle);
        }

        void OnEnable()
        {
            _serializedObject ??= new SerializedObject(obj: this);
        }
    }
}
