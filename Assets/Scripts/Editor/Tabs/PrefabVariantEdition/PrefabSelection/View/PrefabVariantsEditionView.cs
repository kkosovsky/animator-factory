using UnityEngine;
using UnityEngine.UIElements;

namespace AnimatorFactory.PrefabVariants
{
    public class PrefabVariantsEditionView : VisualElement
    {
        public PrefabVariantsEditionView() => CreateUI();

        void CreateUI()
        {
            style.flexGrow = 1;
            style.backgroundColor = new Color(r: 0.3f, g: 0.3f, b: 0.3f);
        }
    }
}
