using AnimatorFactory.Core.UI.SelectionList;
using UnityEngine.UIElements;

namespace AnimatorFactory.PrefabVariants
{
    public sealed class PrefabVariantListItemFactory: ListItemViewFactory
    {
        public PrefabVariantListItemFactory()
        {
        }

        public override VisualElement MakeListItem()
        {
            return new VisualElement();
        }
    }
}
