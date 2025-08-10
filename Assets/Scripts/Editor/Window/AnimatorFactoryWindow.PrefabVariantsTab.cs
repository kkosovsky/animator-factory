using AnimatorFactory.Core.UI;
using AnimatorFactory.PrefabVariants;
using UnityEngine;
using UnityEngine.UIElements;

namespace AnimatorFactory
{
    public partial class AnimatorFactoryWindow
    {
        // PrefabField _prefabParentField;

        VisualElement CreatePrefabVariantsTabContent()
        {
            VisualElement content = new()
            {
                style =
                {
                    flexGrow = 1,
                }
            };

            CreatePrefabParentSection(container: content);

            return content;
        }

        void CreatePrefabParentSection(VisualElement container)
        {
            // _prefabField = PrefabField.Make(
            //     label: Strings.prefabSelectionLabel,
            //     OnPrefabSelectionChanged: OnPrefabParentSelectionChanged
            // );

            _prefabVariantsEditionView = new PrefabVariantsEditionView();
            container.Add(child: _prefabVariantsEditionView);
        }

        void OnPrefabParentSelectionChanged(ChangeEvent<Object> evt)
        {
            Debug.Log(evt);
        }
    }
}
