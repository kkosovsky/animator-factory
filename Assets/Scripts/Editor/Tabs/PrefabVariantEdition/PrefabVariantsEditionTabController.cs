namespace AnimatorFactory.PrefabVariants
{
    public class PrefabVariantsEditionTabController
    {
        readonly PrefabVariantsEditionView _view;
        readonly PrefabVariantsEditionViewModel _viewModel;

        public PrefabVariantsEditionTabController(
            PrefabVariantsEditionView view,
            PrefabVariantsEditionViewModel viewModel
        )
        {
            _view = view;
            _viewModel = viewModel;

            BindEvents();
        }

        void BindEvents()
        {
        }

        void UnbindEvents()
        {
        }
    }
}
