using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace AnimatorFactory.Core.UI
{
    public class PrefabField : ObjectField
    {
        public PrefabField(string label) : base(label: label)
        {
            objectType = typeof(GameObject);
            allowSceneObjects = false;
        }

        public static PrefabField Make(
            string label,
            EventCallback<ChangeEvent<Object>> OnPrefabSelectionChanged
        )
        {
            PrefabField prefabField = new(label: label);
            prefabField.RegisterValueChangedCallback(callback: OnPrefabSelectionChanged);
            return prefabField;
        }
    }
}
