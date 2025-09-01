using AnimatorFactory.PrefabHierarchy;
using UnityEngine;
using UnityEngine.UIElements;

namespace AnimatorFactory.PrefabVariants
{
    public class PrefabVariantsEditionTabController
    {
        readonly PrefabVariantsEditionView _view;
        readonly PrefabVariantsEditionViewModel _viewModel;
        readonly PrefabVariantListController _listController;
        readonly PrefabHierarchyViewModel _hierarchyViewModel;

        public PrefabVariantsEditionTabController()
        {
            _view = new PrefabVariantsEditionView();
            _viewModel = new PrefabVariantsEditionViewModel();
            _hierarchyViewModel = new PrefabHierarchyViewModel();
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
            _view.GenerateButtonClicked += _viewModel.OnGenerateClicked;
            _listController.ItemsApplied += OnVariantItemsApplied;
            _listController.SelectionChanged += OnVariantSelectionChanged;
            _hierarchyViewModel.HierarchyChanged += _view.HierarchyView.OnHierarchyChanged;
        }

        void UnbindEvents()
        {
            _view.PrefabSelected -= OnPrefabSelected;
            _listController.ItemsApplied -= OnVariantItemsApplied;
            _listController.SelectionChanged -= OnVariantSelectionChanged;
            _view.GenerateButtonClicked -= _viewModel.OnGenerateClicked;
            _hierarchyViewModel.HierarchyChanged -= _view.HierarchyView.OnHierarchyChanged;
        }

        void OnPrefabSelected(GameObject prefab)
        {
            if (prefab == null)
            {
                _view.HideHierarchy();
                _hierarchyViewModel.Clear();
            }
            else
            {
                _view.ShowHierarchy();
                _hierarchyViewModel.LoadHierarchy(prefab: prefab);
            }
            
            _listController.OnParentPrefabSelected(prefab: prefab);
        }

        void OnVariantItemsApplied(PrefabVariant[] variants)
        {
            _view.ShowSelectedItemsLabel(count: variants.Length);
            _viewModel.VariantsSelected(variants: variants);
            _view.ShowGenerateButton();
        }
        
        void OnVariantSelectionChanged(PrefabVariant[] variants)
        {
            _view.HideGenerateButton();
            _view.ShowSelectedItemsLabel(count: variants.Length);
            _viewModel.VariantsSelected(variants: variants);
        }
    }
}
