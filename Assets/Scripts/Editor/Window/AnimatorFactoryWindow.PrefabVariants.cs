using UnityEngine;
using UnityEngine.UIElements;

namespace AnimatorFactory
{
    public partial class AnimatorFactoryWindow
    {
        VisualElement CreatePrefabVariantsTabContent()
        {
            VisualElement content = new()
            {
                style =
                {
                    flexGrow = 1,
                    backgroundColor = new Color(0.3f, 0.3f, 0.3f)
                }
            };

            return content;
        }
    }
}
