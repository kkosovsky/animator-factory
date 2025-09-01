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
        readonly Button _generateButton;

        public PrefabVariantsEditionTabController()
        {
            _view = new PrefabVariantsEditionView();
            _viewModel = new PrefabVariantsEditionViewModel();
            _hierarchyViewModel = new PrefabHierarchyViewModel();
            _listController = new PrefabVariantListController(
                viewModel: new PrefabVariantSelectionListViewModel(),
                itemViewFactory: new PrefabVariantListItemFactory()
            );

            _generateButton = new Button(clickEvent: OnGenerateButtonClicked)
            {
                text = "Generate",
                style =
                {
                    display = DisplayStyle.None,
                    height = 24.0f
                }
            };

            _view.Add(child: _listController.View);
            _view.Add(child: _generateButton);
            _listController.Hide();
            BindEvents();
        }

        public VisualElement GetContent() => _view;

        public void Dispose() => UnbindEvents();

        void BindEvents()
        {
            _view.PrefabSelected += OnPrefabSelected;
            _listController.ItemsApplied += OnVariantItemsApplied;
            _listController.SelectionChanged += OnVariantSelectionChanged;
            _hierarchyViewModel.HierarchyChanged += _view.HierarchyView.OnHierarchyChanged;
        }

        void UnbindEvents()
        {
            _view.PrefabSelected -= OnPrefabSelected;
            _listController.ItemsApplied -= OnVariantItemsApplied;
            _listController.SelectionChanged -= OnVariantSelectionChanged;
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
            ShowGenerateButton();
        }
        
        void OnVariantSelectionChanged(PrefabVariant[] variants)
        {
            HideGenerateButton();
            _view.ShowSelectedItemsLabel(count: variants.Length);
            _viewModel.VariantsSelected(variants: variants);
        }

        void OnGenerateButtonClicked() => _viewModel.OnGenerateClicked();

        void ShowGenerateButton()
        {
            _generateButton.style.display = DisplayStyle.Flex;
        }
        
        void HideGenerateButton()
        {
            _generateButton.style.display = DisplayStyle.None;
        }
    }
}
