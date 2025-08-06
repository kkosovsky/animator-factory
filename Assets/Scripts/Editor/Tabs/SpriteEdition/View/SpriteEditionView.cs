using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace AnimatorFactory.SpriteEdition
{
    public class SpriteEditionView : VisualElement
    {
        public event Action<Texture2D> TextureSelectionChanged;

        Image _spriteImage;
        HelpBox _helpBox;

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

        void CreateImageSection()
        {
            _spriteImage = new Image
            {
                scaleMode = ScaleMode.ScaleToFit,
                style =
                {
                    alignSelf = Align.Center,
                    display = DisplayStyle.None,
                    marginTop = 10
                }
            };

            Add(child: _spriteImage);
        }

        void DisplayTexture(Texture2D texture)
        {
            if (texture == null)
            {
                _spriteImage.style.display = DisplayStyle.None;
                _spriteImage.sprite = null;
                return;
            }

            Sprite sprite = CreateSpriteFromTexture(texture: texture);
            _spriteImage.sprite = sprite;
            _spriteImage.style.width = texture.width * 2;
            _spriteImage.style.height = texture.height * 2;
            _spriteImage.style.display = DisplayStyle.Flex;
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
                rect: new Rect(x: 0, y: 0, width: texture.width, height: texture.height),
                pivot: new Vector2(x: 0.5f, y: 0.5f)
            );
        }
    }
}
