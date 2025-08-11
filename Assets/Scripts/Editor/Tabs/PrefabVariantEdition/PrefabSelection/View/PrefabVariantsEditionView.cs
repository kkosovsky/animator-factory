using AnimatorFactory.Core.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace AnimatorFactory.PrefabVariants
{
    public class PrefabVariantsEditionView : VisualElement
    {
        PrefabField _prefabField;

        public PrefabVariantsEditionView() => CreateUI();

        void CreateUI()
        {
            SetStyle();
            AddPrefabSelection();
        }

        void SetStyle()
        {
            style.flexGrow = 1;
            style.backgroundColor = new Color(r: 0.3f, g: 0.3f, b: 0.3f);
        }

        void AddPrefabSelection()
        {
            _prefabField = PrefabField.Make(
                label: "Prefab Selection: ",
                OnPrefabSelectionChanged: OnPrefabSelected
            );
            Add(child: _prefabField);
        }

        void OnPrefabSelected(ChangeEvent<Object> evt)
        {
            GameObject value = (GameObject) evt.newValue;
            ShowVariantsList(value: value);
        }

        void ShowVariantsList(GameObject value)
        {
            //
            
        }
    }
}
