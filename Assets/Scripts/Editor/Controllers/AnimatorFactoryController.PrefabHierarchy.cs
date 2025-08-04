namespace AnimatorFactory.Editor
{
    public partial class AnimatorFactoryController
    {
        void BindPrefabHierarchyPreviewEvents()
        {
            _prefabHierarchyViewModel.HierarchyChanged += _prefabHierarchyView.OnHierarchyChanged;
            _prefabHierarchyView.ItemSelected += OnPrefabHierarchyItemSelected;
        }

        void UnbindPrefabHierarchyEvents()
        {
            _prefabHierarchyViewModel.HierarchyChanged -= _prefabHierarchyView.OnHierarchyChanged;
            _prefabHierarchyView.ItemSelected -= OnPrefabHierarchyItemSelected;
        }

        void OnPrefabHierarchyItemSelected(PrefabHierarchyListItem item)
        {
            _animatorStatesViewModel.LoadAnimatorStates(item: item);
        }
    }
}
