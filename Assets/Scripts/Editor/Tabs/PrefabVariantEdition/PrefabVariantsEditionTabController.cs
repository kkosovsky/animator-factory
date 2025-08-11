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

            BindEvents();
        }

        public VisualElement GetContent() => _view;

        public void Dispose() => UnbindEvents();

        void BindEvents() => _view.DidSelectPrefab += OnDidSelectPrefab;

        void UnbindEvents() => _view.DidSelectPrefab -= OnDidSelectPrefab;

        void OnDidSelectPrefab(GameObject prefab) => _listController.OnPrefabSelected(prefab: prefab);
    }
}
