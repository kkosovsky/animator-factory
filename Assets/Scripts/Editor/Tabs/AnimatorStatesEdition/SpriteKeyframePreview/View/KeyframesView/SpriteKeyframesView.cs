using System.IO;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

namespace AnimatorFactory.SpriteKeyframePreview
{
    /// <summary>
    /// UI component for displaying sprite keyframes.
    /// </summary>
    public class SpriteKeyframesView : VisualElement
    {
        TextField _titleField;
        VisualElement _editableInfoContainer;
        Label _durationLabel;
        FloatField _frameRateField;
        IntegerField _totalFramesField;
        TextField _destinationFolderField;
        Button _browseFolderButton;
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

        /// <summary>
        /// Fired when animation name is changed by user.
        /// </summary>
        public event System.Action<string> AnimationNameChanged;

        /// <summary>
        /// Fired when destination folder is changed by user.
        /// </summary>
        public event System.Action<string> DestinationFolderChanged;

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

            _titleField = new TextField(label: "Clip Name:")
            {
                value = "New Clip",
                style =
                {
                    unityFontStyleAndWeight = FontStyle.Bold,
                    marginBottom = 5
                }
            };
            _titleField.RegisterValueChangedCallback(callback: OnAnimationNameChanged);
            Add(child: _titleField);

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
                style = { height = 160 }
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
            _titleField.SetValueWithoutNotify(newValue: spriteInfo.animationName);

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
                text = "Replace Sprites",
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

            CreateDestinationFolderSection();
        }

        void CreateDestinationFolderSection()
        {
            VisualElement folderRow = new()
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    alignItems = Align.Center,
                    marginBottom = 10,
                    width = Length.Percent(value: 100)
                }
            };

            Label folderLabel = new(text: "Destination:")
            {
                style =
                {
                    fontSize = 11,
                    color = Color.gray,
                    marginRight = 5,
                    width = 80
                }
            };
            folderRow.Add(child: folderLabel);

            _destinationFolderField = new TextField
            {
                value = "Assets/",
                style =
                {
                    width = 200,
                    marginRight = 5
                }
            };
            _destinationFolderField.RegisterValueChangedCallback(callback: OnDestinationFolderChanged);
            folderRow.Add(child: _destinationFolderField);

            _browseFolderButton = new Button
            {
                text = "Browse...",
                style =
                {
                    width = 70,
                    height = 20
                }
            };
            _browseFolderButton.clicked += OnBrowseFolderClicked;
            folderRow.Add(child: _browseFolderButton);

            Add(child: folderRow);
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

        void OnAnimationNameChanged(ChangeEvent<string> evt)
        {
            if (!string.IsNullOrEmpty(value: evt.newValue))
            {
                AnimationNameChanged?.Invoke(obj: evt.newValue);
            }
        }

        void OnDestinationFolderChanged(ChangeEvent<string> evt)
        {
            string newValue = evt.newValue;
            if (string.IsNullOrEmpty(value: newValue))
            {
                return;
            }

            if (!newValue.EndsWith(value: Path.DirectorySeparatorChar))
            {
                _destinationFolderField.value = $"{evt}{Path.DirectorySeparatorChar}";
            }

            DestinationFolderChanged?.Invoke(obj: newValue);
        }

        void OnBrowseFolderClicked()
        {
            string newValue = _destinationFolderField.value;
            string currentFolder = newValue;
            if (string.IsNullOrEmpty(value: currentFolder))
            {
                currentFolder = "Assets";
            }

            string selectedFolder = EditorUtility.SaveFolderPanel(
                title: "Select Destination Folder",
                folder: currentFolder,
                defaultName: ""
            );

            if (string.IsNullOrEmpty(value: selectedFolder))
            {
                return;
            }

            string relativePath = FileUtil.GetProjectRelativePath(path: selectedFolder);
            if (string.IsNullOrEmpty(value: relativePath))
            {
                return;
            }

            if (!newValue.EndsWith(value: Path.DirectorySeparatorChar))
            {
                _destinationFolderField.value = $"{relativePath}{Path.DirectorySeparatorChar}";
            }

            DestinationFolderChanged?.Invoke(obj: _destinationFolderField.value);
        }
    }
}
