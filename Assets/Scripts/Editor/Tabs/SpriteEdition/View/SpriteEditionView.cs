using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace AnimatorFactory.SpriteEdition
{
    public class SpriteEditionView : VisualElement
    {
        public event Action<Texture2D> TextureSelectionChanged;
        public event Action<SpriteImportMode> SpriteModeChangeRequested;

        Image _textureImage;
        HelpBox _helpBox;
        VisualElement _spriteModeContainer;
        Label _spriteModeLabel;
        Label _spriteCountLabel;
        DropdownField _spriteModeDropdown;
        Label _textureInfoLabel;
        VisualElement _frameCalculationContainer;
        TextField _rowsField;
        TextField _columnsField;
        Button _applyButton;
        Label _frameSizeLabel;
        VisualElement _gridOverlay;

        public SpriteEditionView() => CreateUI();

        public void OnTextureChanged(Texture2D texture)
        {
            HideStatus();
            DisplayTexture(texture: texture);
        }

        public void OnStatusChanged(string message, bool isError)
        {
            ShowStatus(message: message, type: isError ? HelpBoxMessageType.Error : HelpBoxMessageType.Info);
        }

        public void OnSpriteModeChanged(SpriteImportMode mode, int spriteCount)
        {
            UpdateSpriteModeDisplay(mode: mode, spriteCount: spriteCount);
        }

        public void SetTextureSelectionField(UnityEditor.UIElements.ObjectField textureField)
        {
            if (textureField != null)
            {
                textureField.RegisterValueChangedCallback(callback: OnTextureFieldChanged);
            }
        }

        void CreateUI()
        {
            style.flexGrow = 1;

            CreateStatusSection();
            CreateSpriteModeSection();
            CreateImageSection();
            CreateTextureInfoSection();
            CreateFrameCalculationSection();
        }

        void CreateStatusSection()
        {
            _helpBox = new HelpBox()
            {
                style =
                {
                    display = DisplayStyle.None,
                    marginBottom = 10
                }
            };

            Add(child: _helpBox);
        }

        void CreateSpriteModeSection()
        {
            _spriteModeContainer = new VisualElement
            {
                style =
                {
                    marginBottom = 10,
                    paddingTop = 8,
                    paddingBottom = 8,
                    paddingLeft = 10,
                    paddingRight = 10,
                    backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.3f),
                    borderTopLeftRadius = 4,
                    borderTopRightRadius = 4,
                    borderBottomLeftRadius = 4,
                    borderBottomRightRadius = 4,
                    display = DisplayStyle.None
                }
            };

            VisualElement topRow = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    justifyContent = Justify.SpaceBetween,
                    alignItems = Align.Center,
                    marginBottom = 5
                }
            };

            _spriteModeLabel = new Label("Current Mode:")
            {
                style =
                {
                    unityFontStyleAndWeight = FontStyle.Bold,
                    color = Color.white
                }
            };

            _spriteCountLabel = new Label("")
            {
                style =
                {
                    color = new Color(0.8f, 0.8f, 0.8f, 1f)
                }
            };

            topRow.Add(_spriteModeLabel);
            topRow.Add(_spriteCountLabel);

            _spriteModeDropdown = new DropdownField("Change Mode:", 
                new List<string> { "Single", "Multiple" }, 0)
            {
                style =
                {
                    marginTop = 3,
                    width = 200
                }
            };
            _spriteModeDropdown.RegisterValueChangedCallback(OnSpriteModeDropdownChanged);

            _spriteModeContainer.Add(topRow);
            _spriteModeContainer.Add(_spriteModeDropdown);
            Add(child: _spriteModeContainer);
        }

        void CreateImageSection()
        {
            _textureImage = new Image
            {
                scaleMode = ScaleMode.ScaleToFit,
                style =
                {
                    alignSelf = Align.Center,
                    display = DisplayStyle.None,
                    marginTop = 10
                }
            };

            Add(child: _textureImage);
        }

        void CreateTextureInfoSection()
        {
            _textureInfoLabel = new Label("")
            {
                style =
                {
                    alignSelf = Align.Center,
                    marginTop = 8,
                    color = new Color(0.8f, 0.8f, 0.8f, 1f),
                    fontSize = 12,
                    display = DisplayStyle.None
                }
            };

            Add(child: _textureInfoLabel);
        }

        void CreateFrameCalculationSection()
        {
            _frameCalculationContainer = new VisualElement
            {
                style =
                {
                    marginTop = 15,
                    paddingTop = 10,
                    paddingBottom = 10,
                    paddingLeft = 10,
                    paddingRight = 10,
                    backgroundColor = new Color(0.15f, 0.15f, 0.15f, 0.5f),
                    borderTopLeftRadius = 4,
                    borderTopRightRadius = 4,
                    borderBottomLeftRadius = 4,
                    borderBottomRightRadius = 4,
                    display = DisplayStyle.None
                }
            };

            Label titleLabel = new Label("Frame Calculation")
            {
                style =
                {
                    unityFontStyleAndWeight = FontStyle.Bold,
                    marginBottom = 8,
                    color = Color.white
                }
            };

            VisualElement inputRow = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    alignItems = Align.Center,
                    marginBottom = 8
                }
            };

            Label rowsLabel = new Label("Rows:")
            {
                style =
                {
                    fontSize = 11,
                    color = Color.gray,
                    marginRight = 5
                }
            };
            inputRow.Add(rowsLabel);

            _rowsField = new TextField
            {
                value = "1",
                style = { width = 50, marginRight = 15 }
            };

            Label columnsLabel = new Label("Columns:")
            {
                style =
                {
                    fontSize = 11,
                    color = Color.gray,
                    marginRight = 5
                }
            };

            _columnsField = new TextField
            {
                value = "1",
                style = { width = 50, marginRight = 15 }
            };

            _applyButton = new Button(OnApplyButtonClicked)
            {
                text = "Apply",
                style = { width = 60 }
            };

            inputRow.Add(_rowsField);
            inputRow.Add(columnsLabel);
            inputRow.Add(_columnsField);
            inputRow.Add(_applyButton);

            _frameSizeLabel = new Label("")
            {
                style =
                {
                    marginTop = 5,
                    color = new Color(0.9f, 0.9f, 0.9f, 1f),
                    fontSize = 12,
                    display = DisplayStyle.None
                }
            };

            _frameCalculationContainer.Add(titleLabel);
            _frameCalculationContainer.Add(inputRow);
            _frameCalculationContainer.Add(_frameSizeLabel);

            Add(child: _frameCalculationContainer);
        }



        void DisplayTexture(Texture2D texture)
        {
            if (texture == null)
            {
                _textureImage.style.display = DisplayStyle.None;
                _textureImage.sprite = null;
                _spriteModeContainer.style.display = DisplayStyle.None;
                _textureInfoLabel.style.display = DisplayStyle.None;
                _frameCalculationContainer.style.display = DisplayStyle.None;
                ClearGridOverlay();
                return;
            }

            Sprite sprite = CreateSpriteFromTexture(texture);
            _textureImage.sprite = sprite;
            _textureImage.style.width = texture.width * 2;
            _textureImage.style.height = texture.height * 2;
            _textureImage.style.display = DisplayStyle.Flex;

            UpdateTextureInfo(texture);
            _frameCalculationContainer.style.display = DisplayStyle.Flex;
            
            // Reset frame size display when new texture is loaded
            _frameSizeLabel.style.display = DisplayStyle.None;
        }

        void UpdateTextureInfo(Texture2D texture)
        {
            if (texture == null)
            {
                _textureInfoLabel.style.display = DisplayStyle.None;
                return;
            }

            string fileName = System.IO.Path.GetFileName(UnityEditor.AssetDatabase.GetAssetPath(texture));
            _textureInfoLabel.text = $"{fileName} ({texture.width} x {texture.height})";
            _textureInfoLabel.style.display = DisplayStyle.Flex;
        }

        void UpdateSpriteModeDisplay(SpriteImportMode mode, int spriteCount)
        {
            if (mode == SpriteImportMode.None)
            {
                _spriteModeContainer.style.display = DisplayStyle.None;
                return;
            }

            _spriteModeContainer.style.display = DisplayStyle.Flex;
            
            string modeText = mode switch
            {
                SpriteImportMode.Single => "Single",
                SpriteImportMode.Multiple => "Multiple",
                _ => "Unknown"
            };

            _spriteModeLabel.text = $"Current Mode: {modeText}";
            
            if (mode == SpriteImportMode.Multiple)
            {
                _spriteCountLabel.text = $"{spriteCount} sprites";
                _spriteCountLabel.style.display = DisplayStyle.Flex;
                _frameCalculationContainer.style.display = DisplayStyle.Flex;
            }
            else
            {
                _spriteCountLabel.style.display = DisplayStyle.None;
                _frameCalculationContainer.style.display = DisplayStyle.None;
                ClearGridOverlay();
            }

            // Update dropdown to reflect current mode without triggering callback
            string currentValue = _spriteModeDropdown.value;
            string newValue = modeText;
            
            if (currentValue != newValue)
            {
                _spriteModeDropdown.SetValueWithoutNotify(newValue);
            }
        }

        void ShowStatus(string message, HelpBoxMessageType type)
        {
            _helpBox.text = message;
            _helpBox.messageType = type;
            _helpBox.style.display = DisplayStyle.Flex;
        }

        void HideStatus()
        {
            _helpBox.style.display = DisplayStyle.None;
        }

        void OnTextureFieldChanged(ChangeEvent<UnityEngine.Object> evt)
        {
            Texture2D selectedTexture = evt.newValue as Texture2D;
            TextureSelectionChanged?.Invoke(obj: selectedTexture);
        }

        void OnSpriteModeDropdownChanged(ChangeEvent<string> evt)
        {
            string selectedMode = evt.newValue;
            
            SpriteImportMode mode = selectedMode switch
            {
                "Single" => SpriteImportMode.Single,
                "Multiple" => SpriteImportMode.Multiple,
                _ => SpriteImportMode.Single
            };

            SpriteModeChangeRequested?.Invoke(obj: mode);
        }

        void OnApplyButtonClicked()
        {
            if (_textureImage.sprite == null)
                return;

            if (!int.TryParse(_rowsField.value, out int rows) || rows <= 0)
            {
                _frameSizeLabel.text = "Error: Invalid rows value. Must be a positive integer.";
                _frameSizeLabel.style.color = Color.red;
                _frameSizeLabel.style.display = DisplayStyle.Flex;
                ClearGridOverlay();
                return;
            }

            if (!int.TryParse(_columnsField.value, out int columns) || columns <= 0)
            {
                _frameSizeLabel.text = "Error: Invalid columns value. Must be a positive integer.";
                _frameSizeLabel.style.color = Color.red;
                _frameSizeLabel.style.display = DisplayStyle.Flex;
                ClearGridOverlay();
                return;
            }

            Texture2D texture = _textureImage.sprite.texture;
            int frameWidth = texture.width / columns;
            int frameHeight = texture.height / rows;

            _frameSizeLabel.text = $"Frame Size: {frameWidth} x {frameHeight} pixels";
            _frameSizeLabel.style.color = new Color(0.9f, 0.9f, 0.9f, 1f);
            _frameSizeLabel.style.display = DisplayStyle.Flex;

            // Show grid overlay on existing image
            ShowGridOverlay(rows, columns, frameWidth, frameHeight);
        }

        Sprite CreateSpriteFromTexture(Texture2D texture)
        {
            return Sprite.Create(
                texture: texture,
                rect: new Rect(0, 0, texture.width, texture.height),
                pivot: new Vector2(0.5f, 0.5f)
            );
        }

        void ShowGridOverlay(int rows, int columns, int frameWidth, int frameHeight)
        {
            ClearGridOverlay();
            
            if (_textureImage.sprite == null)
                return;
                
            // Get the actual displayed image dimensions (image is scaled 2x)
            float displayedWidth = _textureImage.sprite.texture.width * 2;
            float displayedHeight = _textureImage.sprite.texture.height * 2;
            
            // Calculate frame dimensions in display coordinates
            float displayFrameWidth = displayedWidth / columns;
            float displayFrameHeight = displayedHeight / rows;
                
            _gridOverlay = new VisualElement
            {
                style =
                {
                    position = Position.Absolute,
                    top = 0,
                    left = 0,
                    width = displayedWidth,
                    height = displayedHeight
                }
            };
            
            // Create vertical grid lines (including start and end borders)
            for (int col = 0; col <= columns; col++)
            {
                VisualElement verticalLine = new VisualElement
                {
                    style =
                    {
                        position = Position.Absolute,
                        left = col * displayFrameWidth,
                        top = 0,
                        width = 1,
                        height = Length.Percent(100),
                        backgroundColor = Color.white
                    }
                };
                _gridOverlay.Add(verticalLine);
            }
            
            // Create horizontal grid lines
            for (int row = 1; row < rows; row++)
            {
                VisualElement horizontalLine = new VisualElement
                {
                    style =
                    {
                        position = Position.Absolute,
                        left = 0,
                        top = row * displayFrameHeight,
                        width = Length.Percent(100),
                        height = 1,
                        backgroundColor = Color.white
                    }
                };
                _gridOverlay.Add(horizontalLine);
            }
            
            // Add overlay as a child of the image itself
            _textureImage.Add(_gridOverlay);
        }
        
        void ClearGridOverlay()
        {
            if (_gridOverlay != null)
            {
                _gridOverlay.RemoveFromHierarchy();
                _gridOverlay = null;
            }
        }
    }
}
