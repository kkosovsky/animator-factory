using UnityEngine;
using UnityEngine.UIElements;

namespace AnimatorFactory.SpriteKeyframePreview
{
    /// <summary>
    /// UI component for displaying sprite keyframes.
    /// </summary>
    public class SpriteKeyframesView : VisualElement
    {
        Label _titleLabel;
        VisualElement _editableInfoContainer;
        Label _durationLabel;
        FloatField _frameRateField;
        IntegerField _totalFramesField;
        ScrollView _keyframesScrollView;
        VisualElement _keyframesContainer;
        HelpBox _helpBox;
        SpriteSelectionListView _spriteSelectionListView;

        /// <summary>
        /// Fired when frame rate is changed by user.
        /// </summary>
        public event System.Action<float> FrameRateChanged;

        /// <summary>
        /// Fired when total frames is changed by user.
        /// </summary>
        public event System.Action<int> TotalFramesChanged;

        /// <summary>
        /// Fired when sprites are selected for replacement.
        /// </summary>
        public event System.Action<Sprite[]> SpritesSelected;

        public SpriteKeyframesView() => CreateUI();

        public void OnDataChanged(AnimationSpriteInfo spriteInfo)
        {
            HideStatus();
            DisplayKeyframes(spriteInfo: spriteInfo);
            style.display = DisplayStyle.Flex;
        }

        public void OnStatusChanged(string message, bool isError)
        {
            ShowStatus(message: message, type: isError ? HelpBoxMessageType.Error : HelpBoxMessageType.Info);
            style.display = DisplayStyle.Flex;
        }

        /// <summary>
        /// Hides the view and clears its content.
        /// </summary>
        public void Hide()
        {
            style.display = DisplayStyle.None;
            _keyframesContainer.Clear();
        }

        void CreateUI()
        {
            style.paddingTop = 10;
            style.display = DisplayStyle.None;

            _titleLabel = new Label(text: "Sprite Keyframes")
            {
                style =
                {
                    unityFontStyleAndWeight = FontStyle.Bold,
                    marginBottom = 5
                }
            };
            Add(child: _titleLabel);

            CreateEditableInfoSection();

            _helpBox = new HelpBox(text: "", messageType: HelpBoxMessageType.Info)
            {
                style =
                {
                    display = DisplayStyle.None,
                    marginBottom = 10
                }
            };
            Add(child: _helpBox);

            _keyframesScrollView = new ScrollView(scrollViewMode: ScrollViewMode.Horizontal)
            {
                style = { height = 80 }
            };

            _keyframesContainer = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    flexWrap = Wrap.NoWrap
                }
            };

            _keyframesScrollView.Add(child: _keyframesContainer);
            Add(child: _keyframesScrollView);

            _spriteSelectionListView = new SpriteSelectionListView
            {
                style = { display = DisplayStyle.None }
            };
            _spriteSelectionListView.SpritesApplied += OnSpritesApplied;
            _spriteSelectionListView.CancelRequested += OnSpriteSelectionCancelled;
            Add(child: _spriteSelectionListView);
        }

        void OnChangeSpriteButtonClicked()
        {
            bool isCurrentlyVisible = _spriteSelectionListView.style.display == DisplayStyle.Flex;

            if (isCurrentlyVisible)
            {
                HideSpriteSelection();
            }
            else
            {
                ShowSpriteSelection();
            }
        }

        void ShowSpriteSelection()
        {
            _spriteSelectionListView.style.display = DisplayStyle.Flex;
            _spriteSelectionListView.RefreshSprites();
            _spriteSelectionListView.ClearSelection();

            _keyframesScrollView.style.display = DisplayStyle.None;
        }

        void HideSpriteSelection()
        {
            _spriteSelectionListView.style.display = DisplayStyle.None;
            _keyframesScrollView.style.display = DisplayStyle.Flex;
        }

        void OnSpritesApplied(Sprite[] selectedSprites)
        {
            HideSpriteSelection();
            SpritesSelected?.Invoke(obj: selectedSprites);
        }

        void OnSpriteSelectionCancelled() => HideSpriteSelection();

        void DisplayKeyframes(AnimationSpriteInfo spriteInfo)
        {
            _keyframesContainer.Clear();
            _titleLabel.text = $"Sprite Keyframes - {spriteInfo.animationName}";

            _durationLabel.text = $"Duration: {spriteInfo.duration:F2}s";
            _frameRateField.SetValueWithoutNotify(newValue: spriteInfo.frameRate);
            _totalFramesField.SetValueWithoutNotify(newValue: spriteInfo.totalFrames);

            RefreshKeyframeElements(spriteInfo: spriteInfo);
        }

        void RefreshKeyframeElements(AnimationSpriteInfo spriteInfo)
        {
            _keyframesContainer.Clear();

            foreach (SpriteKeyframeData keyframe in spriteInfo.keyframes)
            {
                VisualElement keyframeElement = new KeyframeElementView(keyframe: keyframe);
                _keyframesContainer.Add(child: keyframeElement);
            }
        }

        void ShowStatus(string message, HelpBoxMessageType type)
        {
            _helpBox.text = message;
            _helpBox.messageType = type;
            _helpBox.style.display = DisplayStyle.Flex;
            _keyframesScrollView.style.display = DisplayStyle.None;
            _editableInfoContainer.style.display = DisplayStyle.None;
        }

        void HideStatus()
        {
            _helpBox.style.display = DisplayStyle.None;
            _keyframesScrollView.style.display = DisplayStyle.Flex;
            _editableInfoContainer.style.display = DisplayStyle.Flex;
        }

        void CreateEditableInfoSection()
        {
            _editableInfoContainer = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    alignItems = Align.Center,
                    marginBottom = 10,
                    flexWrap = Wrap.Wrap
                }
            };

            _durationLabel = new Label(text: "Duration: 0.00s")
            {
                style =
                {
                    fontSize = 11,
                    color = Color.gray,
                    marginRight = 10
                }
            };
            _editableInfoContainer.Add(child: _durationLabel);

            Label frameRateLabel = new Label(text: "Frame Rate:")
            {
                style =
                {
                    fontSize = 11,
                    color = Color.gray,
                    marginRight = 5
                }
            };
            _editableInfoContainer.Add(child: frameRateLabel);

            _frameRateField = new FloatField
            {
                value = 12.0f,
                style =
                {
                    width = 50,
                    marginRight = 5
                }
            };
            _frameRateField.RegisterValueChangedCallback(callback: OnFrameRateChanged);
            _editableInfoContainer.Add(child: _frameRateField);

            Label fpsLabel = new Label(text: "fps")
            {
                style =
                {
                    fontSize = 11,
                    color = Color.gray,
                    marginRight = 15
                }
            };
            _editableInfoContainer.Add(child: fpsLabel);

            Label totalFramesLabel = new Label(text: "Total Frames:")
            {
                style =
                {
                    fontSize = 11,
                    color = Color.gray,
                    marginRight = 5
                }
            };
            _editableInfoContainer.Add(child: totalFramesLabel);

            _totalFramesField = new IntegerField
            {
                value = 10,
                style =
                {
                    width = 50,
                    marginRight = 5
                }
            };
            _totalFramesField.RegisterValueChangedCallback(callback: OnTotalFramesChanged);
            _editableInfoContainer.Add(child: _totalFramesField);

            // Change button next to total frames
            Button changeButton = new()
            {
                text = "Change Sprite",
                tooltip = "Select sprites to replace all the keyframes",
                style =
                {
                    height = 20,
                    width = 100,
                    fontSize = 10,
                    borderTopWidth = 2,
                    borderBottomWidth = 2,
                    borderLeftWidth = 2,
                    borderRightWidth = 2,
                    borderTopLeftRadius = 3,
                    borderTopRightRadius = 3,
                    borderBottomLeftRadius = 3,
                    borderBottomRightRadius = 3
                }
            };
            changeButton.clicked += OnChangeSpriteButtonClicked;
            _editableInfoContainer.Add(child: changeButton);

            Add(child: _editableInfoContainer);
        }


        void OnFrameRateChanged(ChangeEvent<float> evt)
        {
            if (evt.newValue > 0)
            {
                FrameRateChanged?.Invoke(obj: evt.newValue);
            }
        }

        void OnTotalFramesChanged(ChangeEvent<int> evt)
        {
            if (evt.newValue > 0)
            {
                TotalFramesChanged?.Invoke(obj: evt.newValue);
            }
        }
    }
}
