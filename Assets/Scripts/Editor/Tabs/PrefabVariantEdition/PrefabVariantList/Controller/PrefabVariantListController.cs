using AnimatorFactory.Core.UI.SelectionList;
using UnityEngine;

namespace AnimatorFactory.PrefabVariants
{
    public class PrefabVariantListController : SelectionListViewController<GameObject, GameObject>
    {
        public PrefabVariantListController(
            PrefabVariantSelectionListViewModel viewModel,
            ListItemViewFactory itemViewFactory
        ) : base(headerText: "Select Variants:", viewModel: viewModel, itemViewFactory: itemViewFactory)
        {
        }

        public void OnPrefabSelected(GameObject prefab) => viewModel.OnSourceItemChanged(item: prefab);
    }
}
