using UnityEngine;
using UnityEngine.UIElements;

namespace AnimatorFactory.PrefabVariants
{
    public class PrefabVariantsEditionTabController
    {
        readonly PrefabVariantsEditionView _view;
        readonly PrefabVariantsEditionViewModel _viewModel;
        readonly PrefabVariantListController _listController;

        public PrefabVariantsEditionTabController()
        {
            _view = new PrefabVariantsEditionView();
            _viewModel = new PrefabVariantsEditionViewModel();
            _listController = new PrefabVariantListController(
                viewModel: new PrefabVariantSelectionListViewModel(),
                itemViewFactory: new PrefabVariantListItemFactory()
            );

            _view.Add(child: _listController.View);
            _listController.Hide();
            BindEvents();
        }

        public VisualElement GetContent() => _view;

        public void Dispose() => UnbindEvents();

        void BindEvents()
        {
            _view.DidSelectPrefab += OnDidSelectPrefab;
            _listController.ItemsApplied += OnItemsApplied;
        }

        void UnbindEvents()
        {
            _view.DidSelectPrefab -= OnDidSelectPrefab;
            _listController.ItemsApplied -= OnItemsApplied;
        }

        void OnDidSelectPrefab(GameObject prefab) => _listController.OnPrefabSelected(prefab: prefab);

        void OnItemsApplied(GameObject[] items)
        {
        }
    }
}
