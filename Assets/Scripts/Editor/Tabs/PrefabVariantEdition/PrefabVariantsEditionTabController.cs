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
            _view.SpritesSourceFolderChanged += _viewModel.SourceFolderChanged;
            _view.GenerateButtonClicked += _viewModel.OnGenerateClicked;
            _listController.ItemsApplied += OnVariantItemsApplied;
        }

        void UnbindEvents()
        {
            _view.PrefabSelected -= OnPrefabSelected;
            _listController.ItemsApplied -= OnVariantItemsApplied;
            _view.SpritesSourceFolderChanged -= _viewModel.SourceFolderChanged;
            _view.GenerateButtonClicked -= _viewModel.OnGenerateClicked;
        }

        void OnPrefabSelected(GameObject prefab)
        {
            _listController.OnParentPrefabSelected(prefab: prefab);
            _viewModel.PrefabSelected(prefab: prefab);
        }

        void OnVariantItemsApplied(PrefabVariant[] variants)
        {
            _listController.Hide();
            _view.ShowSelectedItemsLabel(count: variants.Length);
            _viewModel.VariantsSelected(variants: variants);
        }
    }
}
