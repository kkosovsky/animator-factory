using AnimatorFactory.PrefabHierarchy;
using AnimatorFactory.SpriteKeyframePreview;
using AnimatorFactory.AnimatorStatePreview;
using UnityEngine;

namespace AnimatorFactory.Editor
{
    /// <summary>
    /// Main controller that coordinates sections of the Animator Factory Tool.
    /// </summary>
    public partial class AnimatorFactoryController
    {
        readonly PrefabHierarchyViewModel _prefabHierarchyViewModel;
        readonly AnimatorStatesViewModel _animatorStatesViewModel;
        readonly SpriteKeyframeViewModel _spriteKeyframeViewModel;

        readonly PrefabHierarchyView _prefabHierarchyView;
        readonly AnimatorStatesView _animatorStatesView;
        readonly SpriteKeyframeView _spriteKeyframeView;

        /// <summary>
        /// Initializes the controller with all required views.
        /// </summary>
        /// <param name="prefabHierarchyView">The prefab hierarchy view</param>
        /// <param name="animatorStatesView">The animator states view</param>
        /// <param name="spriteKeyframeView">The sprite keyframe view</param>
        public AnimatorFactoryController(
            PrefabHierarchyView prefabHierarchyView,
            AnimatorStatesView animatorStatesView,
            SpriteKeyframeView spriteKeyframeView
        )
        {
            _prefabHierarchyViewModel = new PrefabHierarchyViewModel();
            _animatorStatesViewModel = new AnimatorStatesViewModel();
            _spriteKeyframeViewModel = new SpriteKeyframeViewModel();

            _prefabHierarchyView = prefabHierarchyView;
            _animatorStatesView = animatorStatesView;
            _spriteKeyframeView = spriteKeyframeView;

            BindEvents();
            WireFeatureInteractions();
        }

        /// <summary>
        /// Handles prefab selection changes from the main window.
        /// </summary>
        /// <param name="prefab">The selected prefab (null if cleared)</param>
        public void OnPrefabSelectionChanged(GameObject prefab)
        {
            if (prefab == null)
            {
                _prefabHierarchyViewModel.Clear();
                _animatorStatesViewModel.Clear();
                _spriteKeyframeViewModel.Clear();
            }
            else
            {
                _prefabHierarchyViewModel.LoadHierarchy(prefab: prefab);
                _animatorStatesViewModel.Clear();
                _spriteKeyframeViewModel.Clear();
            }
        }

        /// <summary>
        /// Disposes of the controller and cleans up event subscriptions.
        /// </summary>
        public void Dispose() => UnbindEvents();

        void BindEvents()
        {
            BindPrefabHierarchyPreviewEvents();
            BindAnimatorStatesPreviewEvents();
            BindSpriteKeyFramePreviewEvents();
        }

        void UnbindEvents()
        {
            UnbindPrefabHierarchyEvents();
            UnbindAnimatorStatesPreviewEvents();
            UnbindSpriteKeyFramePreviewEvents();
        }

        void WireFeatureInteractions()
        {

            _prefabHierarchyViewModel.HierarchyChanged += _ =>
            {
                _animatorStatesViewModel.Clear();
                _spriteKeyframeViewModel.Clear();
            };
        }
    }
}
