using AnimatorFactory.Core.UI.SelectionList;
using UnityEngine;

namespace AnimatorFactory.PrefabVariants
{
    public class PrefabVariantsEditionTabController
    {
        readonly PrefabVariantsEditionView _view;
        readonly PrefabVariantsEditionViewModel _viewModel;
        readonly PrefabVariantListController _listController;

        public PrefabVariantsEditionTabController(
            PrefabVariantsEditionView view,
            PrefabVariantsEditionViewModel viewModel
        )
        {
            _view = view;
            _viewModel = viewModel;
            _listController = new PrefabVariantListController(
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
            _view.DidSelectPrefab += OnDidSelectPrefab;
        }

        void UnbindEvents()
        {
            _view.DidSelectPrefab -= OnDidSelectPrefab;
        }

        void OnDidSelectPrefab(GameObject prefab)
        {
            _listController.OnPrefabSelected(prefab: prefab);
        }
    }
}
