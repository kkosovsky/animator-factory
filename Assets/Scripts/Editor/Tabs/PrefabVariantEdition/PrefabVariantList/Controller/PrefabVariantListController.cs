using AnimatorFactory.Core.UI.SelectionList;
using UnityEngine;
using UnityEngine.UIElements;

namespace AnimatorFactory.PrefabVariants
{
    public class PrefabVariantListController : SelectionListViewController<GameObject, GameObject>
    {
        public PrefabVariantListController(
            PrefabVariantSelectionListViewModel viewModel,
            ListItemViewFactory itemViewFactory
        ) : base(headerText: "Select Variants:", viewModel: viewModel, itemViewFactory: itemViewFactory)
        {
            view.clearAllButton.style.display = DisplayStyle.None;
            view.selectAllButton.style.display = DisplayStyle.None;
        }

        public void OnPrefabSelected(GameObject prefab)
        {
            viewModel.OnSourceItemChanged(item: prefab);
            RefreshList();
            UpdateVisibility();
        }

        void UpdateVisibility()
        {
            if (view.list.itemsSource.Count == 0)
            {
                Hide();
                return;
            }
            
            Show();
        }
    }
}
