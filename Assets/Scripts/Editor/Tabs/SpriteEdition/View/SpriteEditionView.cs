using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace AnimatorFactory.SpriteEdition
{
    public class SpriteEditionView : VisualElement
    {
        public event Action<Texture2D> TextureSelectionChanged;
        public event Action<SpriteImportMode> SpriteModeChangeRequested;
        public event Action<FrameGenerationData> FrameGenerationRequested;

        ObjectField _textureField;
        Image _textureImage;
        Label _textureInfoLabel;
        HelpBox _helpBox;
        VisualElement _spriteModeContainer;
        Label _spriteModeLabel;
        Label _spriteCountLabel;
        DropdownField _spriteModeDropdown;
        VisualElement _frameCalculationContainer;
        TextField _rowsField;
        TextField _columnsField;
        Button _applyButton;
        Button _generateButton;
        Label _frameSizeLabel;
        SpriteGridOverlay _gridOverlay;

        public SpriteEditionView()
        {
            CreateUI();
            InitializeGridOverlay();
        }

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

        void CreateUI()
        {
            style.flexGrow = 1;

            CreateSpriteSelectionSection();
            CreateStatusSection();
            CreateSpriteModeSection();
            CreateImageSection();
            CreateTextureInfoSection();
            CreateFrameCalculationSection();
        }

        void CreateSpriteSelectionSection()
        {
            _textureField = new ObjectField(label: "Select Texture")
            {
                objectType = typeof(Texture2D),
                allowSceneObjects = false
            };

            _textureField.RegisterValueChangedCallback(callback: OnTextureFieldChanged);
            Add(child: _textureField);
        }

        void InitializeGridOverlay()
        {
            _gridOverlay = new SpriteGridOverlay(targetImage: _textureImage);
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
                    backgroundColor = new Color(r: 0.2f, g: 0.2f, b: 0.2f, a: 0.3f),
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

            _spriteModeLabel = new Label(text: "Current Mode:")
            {
                style =
                {
                    unityFontStyleAndWeight = FontStyle.Bold,
                    color = Color.white
                }
            };

            _spriteCountLabel = new Label(text: "")
            {
                style =
                {
                    color = new Color(r: 0.8f, g: 0.8f, b: 0.8f, a: 1f)
                }
            };

            topRow.Add(child: _spriteModeLabel);
            topRow.Add(child: _spriteCountLabel);

            _spriteModeDropdown = new DropdownField(
                label: "Change Mode:",
                choices: new List<string> { "Single", "Multiple" },
                defaultIndex: 0
            )
            {
                style =
                {
                    marginTop = 3,
                    width = 200
                }
            };
            _spriteModeDropdown.RegisterValueChangedCallback(callback: OnSpriteModeDropdownChanged);

            _spriteModeContainer.Add(child: topRow);
            _spriteModeContainer.Add(child: _spriteModeDropdown);
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
            _textureInfoLabel = new Label(text: "")
            {
                style =
                {
                    alignSelf = Align.Center,
                    marginTop = 8,
                    color = new Color(r: 0.8f, g: 0.8f, b: 0.8f, a: 1f),
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
                    backgroundColor = new Color(r: 0.15f, g: 0.15f, b: 0.15f, a: 0.5f),
                    borderTopLeftRadius = 4,
                    borderTopRightRadius = 4,
                    borderBottomLeftRadius = 4,
                    borderBottomRightRadius = 4,
                    display = DisplayStyle.None
                }
            };

            Label titleLabel = new Label(text: "Frame Calculation")
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

            Label rowsLabel = new Label(text: "Rows:")
            {
                style =
                {
                    fontSize = 11,
                    color = Color.gray,
                    marginRight = 5
                }
            };
            inputRow.Add(child: rowsLabel);

            _rowsField = new TextField
            {
                value = "1",
                style = { width = 50, marginRight = 15 }
            };

            Label columnsLabel = new Label(text: "Columns:")
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

            _applyButton = new Button(clickEvent: OnApplyButtonClicked)
            {
                text = "Apply",
                style = { width = 60 }
            };

            inputRow.Add(child: _rowsField);
            inputRow.Add(child: columnsLabel);
            inputRow.Add(child: _columnsField);
            inputRow.Add(child: _applyButton);

            _frameSizeLabel = new Label(text: "")
            {
                style =
                {
                    marginTop = 5,
                    color = new Color(r: 0.9f, g: 0.9f, b: 0.9f, a: 1f),
                    fontSize = 12,
                    display = DisplayStyle.None
                }
            };

            _frameCalculationContainer.Add(child: titleLabel);
            _frameCalculationContainer.Add(child: inputRow);
            _frameCalculationContainer.Add(child: _frameSizeLabel);

            // Create Generate button row
            VisualElement generateRow = new()
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    justifyContent = Justify.Center,
                    marginTop = 10
                }
            };

            _generateButton = new Button(clickEvent: OnGenerateButtonClicked)
            {
                text = "Generate Frames",
                style =
                {
                    width = 120,
                    height = 25,
                    backgroundColor = new Color(r: 0.2f, g: 0.6f, b: 0.2f, a: 1f),
                    color = Color.white,
                    unityFontStyleAndWeight = FontStyle.Bold
                }
            };

            generateRow.Add(child: _generateButton);
            _frameCalculationContainer.Add(child: generateRow);

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
                _gridOverlay?.ClearGrid();
                return;
            }

            Sprite sprite = CreateSpriteFromTexture(texture: texture);
            _textureImage.sprite = sprite;
            _textureImage.style.width = texture.width * 2;
            _textureImage.style.height = texture.height * 2;
            _textureImage.style.display = DisplayStyle.Flex;

            UpdateTextureInfo(texture: texture);
            _frameCalculationContainer.style.display = DisplayStyle.Flex;

            _frameSizeLabel.style.display = DisplayStyle.None;
        }

        void UpdateTextureInfo(Texture2D texture)
        {
            if (texture == null)
            {
                _textureInfoLabel.style.display = DisplayStyle.None;
                return;
            }

            string fileName = System.IO.Path.GetFileName(path: AssetDatabase.GetAssetPath(assetObject: texture));
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
                _gridOverlay?.ClearGrid();
            }

            // Update dropdown to reflect current mode without triggering callback
            string currentValue = _spriteModeDropdown.value;
            string newValue = modeText;

            if (currentValue != newValue)
            {
                _spriteModeDropdown.SetValueWithoutNotify(newValue: newValue);
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

            if (!int.TryParse(s: _rowsField.value, result: out int rows) || rows <= 0)
            {
                _frameSizeLabel.text = "Error: Invalid rows value. Must be a positive integer.";
                _frameSizeLabel.style.color = Color.red;
                _frameSizeLabel.style.display = DisplayStyle.Flex;
                _gridOverlay?.ClearGrid();
                return;
            }

            if (!int.TryParse(s: _columnsField.value, result: out int columns) || columns <= 0)
            {
                _frameSizeLabel.text = "Error: Invalid columns value. Must be a positive integer.";
                _frameSizeLabel.style.color = Color.red;
                _frameSizeLabel.style.display = DisplayStyle.Flex;
                _gridOverlay?.ClearGrid();
                return;
            }

            Texture2D texture = _textureImage.sprite.texture;
            int frameWidth = texture.width / columns;
            int frameHeight = texture.height / rows;

            _frameSizeLabel.text = $"Frame Size: {frameWidth} x {frameHeight} pixels";
            _frameSizeLabel.style.color = new Color(r: 0.9f, g: 0.9f, b: 0.9f, a: 1f);
            _frameSizeLabel.style.display = DisplayStyle.Flex;

            _gridOverlay?.ShowGrid(rows: rows, columns: columns, frameWidth: frameWidth, frameHeight: frameHeight);
        }

        void OnGenerateButtonClicked()
        {
            if (_textureImage.sprite == null)
            {
                ShowStatus(message: "No texture selected for frame generation.", type: HelpBoxMessageType.Warning);
                return;
            }

            if (!int.TryParse(s: _rowsField.value, result: out int rows) || rows <= 0)
            {
                ShowStatus(message: "Invalid rows value. Must be a positive integer.", type: HelpBoxMessageType.Error);
                return;
            }

            if (!int.TryParse(s: _columnsField.value, result: out int columns) || columns <= 0)
            {
                ShowStatus(
                    message: "Invalid columns value. Must be a positive integer.",
                    type: HelpBoxMessageType.Error
                );
                return;
            }

            Texture2D texture = _textureImage.sprite.texture;
            int frameWidth = texture.width / columns;
            int frameHeight = texture.height / rows;

            FrameGenerationData generationData = new FrameGenerationData
            {
                Texture = texture,
                Rows = rows,
                Columns = columns,
                FrameWidth = frameWidth,
                FrameHeight = frameHeight
            };

            FrameGenerationRequested?.Invoke(obj: generationData);
        }

        Sprite CreateSpriteFromTexture(Texture2D texture)
        {
            return Sprite.Create(
                texture: texture,
                rect: new Rect(x: 0, y: 0, width: texture.width, height: texture.height),
                pivot: new Vector2(x: 0.5f, y: 0.5f)
            );
        }
    }
}
