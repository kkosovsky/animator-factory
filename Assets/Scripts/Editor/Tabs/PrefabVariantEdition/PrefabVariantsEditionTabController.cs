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
            _view.PrefabSelected += OnPrefabSelected;
            _view.DestinationChanged += OnDestinationChanged;
            _listController.ItemsApplied += OnVariantItemsApplied;
        }

        void UnbindEvents()
        {
            _view.PrefabSelected -= OnPrefabSelected;
            _view.DestinationChanged -= OnDestinationChanged;
            _listController.ItemsApplied -= OnVariantItemsApplied;
        }

        void OnPrefabSelected(GameObject prefab)
        {
            _listController.OnPrefabSelected(prefab: prefab);
            _viewModel.PrefabSelected(prefab: prefab);
        }

        void OnDestinationChanged(string path) => _viewModel.DestinationChanged(path: path);

        void OnVariantItemsApplied(GameObject[] items)
        {
            _listController.Hide();
            _view.ShowSelectedItemsLabel(count: items.Length);
        }
    }
}
