using AnimatorFactory.Core.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace AnimatorFactory
{
    public partial class AnimatorFactoryWindow
    {
        PrefabField _prefabParentField;

        VisualElement CreatePrefabVariantsTabContent()
        {
            VisualElement content = new()
            {
                style =
                {
                    flexGrow = 1,
                    backgroundColor = new Color(r: 0.3f, g: 0.3f, b: 0.3f)
                }
            };
            
            CreatePrefabParentSection(container: content);

            return content;
        }

        void CreatePrefabParentSection(VisualElement container)
        {
            _prefabField = PrefabField.Make(
                label: Strings.prefabSelectionLabel,
                OnPrefabSelectionChanged: OnPrefabParentSelectionChanged
            );
            container.Add(child: _prefabField);
        }

        void OnPrefabParentSelectionChanged(ChangeEvent<Object> evt)
        {
            Debug.Log(evt);
        }
    }
}
