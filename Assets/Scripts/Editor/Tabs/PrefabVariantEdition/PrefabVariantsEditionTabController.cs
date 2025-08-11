using AnimatorFactory.Core.UI.SelectionList;
using UnityEngine;

namespace AnimatorFactory.PrefabVariants
{
    public class PrefabVariantsEditionTabController
    {
        readonly PrefabVariantsEditionView _view;
        readonly PrefabVariantsEditionViewModel _viewModel;
        readonly SelectionListViewController<GameObject> _listController;

        public PrefabVariantsEditionTabController(
            PrefabVariantsEditionView view,
            PrefabVariantsEditionViewModel viewModel
        )
        {
            _view = view;
            _viewModel = viewModel;
            _listController = new SelectionListViewController<GameObject>(
                headerText: "Select Variants:",
                viewModel: new PrefabVariantSelectionListViewModel(),
                itemViewFactory: new PrefabVariantListItemFactory()
            );
            
            BindEvents();
        }

        public void Dispose()
        {
        }

        void BindEvents()
        {
        }

        void UnbindEvents()
        {
        }
    }
}
