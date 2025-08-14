using AnimatorFactory.SpriteEdition;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace AnimatorFactory.Editor
{
    public class SpriteEditionTabController
    {
        readonly SpriteEditionViewModel _viewModel;
        readonly SpriteEditionView _view;

        public SpriteEditionTabController()
        {
            _viewModel = new SpriteEditionViewModel();
            _view = new SpriteEditionView();

            BindEvents();
        }

        public void OnTextureSelectionChanged(Texture2D texture)
        {
            _viewModel.LoadTexture(texture: texture);
        }

        public void Dispose() => UnbindEvents();

        public VisualElement GetContent() => _view;

        void BindEvents()
        {
            _viewModel.TextureChanged += _view.OnTextureChanged;
            _viewModel.StatusChanged += _view.OnStatusChanged;
            _viewModel.SpriteModeChanged += _view.OnSpriteModeChanged;
            _view.TextureSelectionChanged += OnTextureSelectionChanged;
            _view.SpriteModeChangeRequested += OnSpriteModeChangeRequested;
            _view.FrameGenerationRequested += OnFrameGenerationRequested;
        }

        void UnbindEvents()
        {
            _viewModel.TextureChanged -= _view.OnTextureChanged;
            _viewModel.StatusChanged -= _view.OnStatusChanged;
            _viewModel.SpriteModeChanged -= _view.OnSpriteModeChanged;
            _view.TextureSelectionChanged -= OnTextureSelectionChanged;
            _view.SpriteModeChangeRequested -= OnSpriteModeChangeRequested;
            _view.FrameGenerationRequested -= OnFrameGenerationRequested;
        }

        void OnSpriteModeChangeRequested(SpriteImportMode newMode)
        {
            _viewModel.ChangeSpriteMode(newMode: newMode);
        }

        void OnFrameGenerationRequested(FrameGenerationData generationData)
        {
            _viewModel.GenerateFrames(generationData: generationData);
        }
    }
}
