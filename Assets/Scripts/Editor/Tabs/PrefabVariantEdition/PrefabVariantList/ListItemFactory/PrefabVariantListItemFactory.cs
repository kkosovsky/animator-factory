using AnimatorFactory.Core.UI.SelectionList;
using UnityEngine.UIElements;

namespace AnimatorFactory.PrefabVariants
{
    public sealed class PrefabVariantListItemFactory: ListItemViewFactory
    {
        public override VisualElement MakeListItem() => new PrefabVariantCell();
    }
}
