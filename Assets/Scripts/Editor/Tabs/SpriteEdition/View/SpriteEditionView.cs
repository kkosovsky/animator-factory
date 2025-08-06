using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace AnimatorFactory.SpriteEdition
{
    public class SpriteEditionView : VisualElement
    {
        public event Action<Texture2D> TextureSelectionChanged;

        Image _textureImage;
        HelpBox _helpBox;
        VisualElement _spriteModeContainer;
        Label _spriteModeLabel;
        Label _spriteCountLabel;

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
                    flexDirection = FlexDirection.Row,
                    justifyContent = Justify.SpaceBetween,
                    marginBottom = 10,
                    paddingTop = 5,
                    paddingBottom = 5,
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

            _spriteModeLabel = new Label("Mode: Unknown")
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

            _spriteModeContainer.Add(_spriteModeLabel);
            _spriteModeContainer.Add(_spriteCountLabel);
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

        void DisplayTexture(Texture2D texture)
        {
            if (texture == null)
            {
                _textureImage.style.display = DisplayStyle.None;
                _textureImage.sprite = null;
                _spriteModeContainer.style.display = DisplayStyle.None;
                return;
            }

            Sprite sprite = CreateSpriteFromTexture(texture);
            _textureImage.sprite = sprite;
            _textureImage.style.width = texture.width * 2;
            _textureImage.style.height = texture.height * 2;
            _textureImage.style.display = DisplayStyle.Flex;
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
                SpriteImportMode.Polygon => "Polygon",
                _ => "Unknown"
            };

            _spriteModeLabel.text = $"Mode: {modeText}";
            
            if (mode == SpriteImportMode.Multiple)
            {
                _spriteCountLabel.text = $"{spriteCount} sprites";
                _spriteCountLabel.style.display = DisplayStyle.Flex;
            }
            else
            {
                _spriteCountLabel.style.display = DisplayStyle.None;
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

        Sprite CreateSpriteFromTexture(Texture2D texture)
        {
            return Sprite.Create(
                texture: texture,
                rect: new Rect(0, 0, texture.width, texture.height),
                pivot: new Vector2(0.5f, 0.5f)
            );
        }
    }
}
