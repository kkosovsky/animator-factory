using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace AnimatorFactory.SpriteEdition
{
    public class SpriteEditionView : VisualElement
    {
        public event Action<Sprite> SpriteSelectionChanged;

        Image _spriteImage;
        HelpBox _helpBox;

        public SpriteEditionView() => CreateUI();

        public void OnSpriteChanged(Sprite sprite)
        {
            HideStatus();
            DisplaySprite(sprite: sprite);
        }

        public void OnStatusChanged(string message, bool isError)
        {
            ShowStatus(message: message, type: isError ? HelpBoxMessageType.Error : HelpBoxMessageType.Info);
        }

        public void SetSpriteSelectionField(UnityEditor.UIElements.ObjectField spriteField)
        {
            if (spriteField != null)
            {
                spriteField.RegisterValueChangedCallback(callback: OnSpriteFieldChanged);
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

        void DisplaySprite(Sprite sprite)
        {
            if (sprite == null)
            {
                _spriteImage.style.display = DisplayStyle.None;
                _spriteImage.sprite = null;
                return;
            }

            _spriteImage.sprite = sprite;
            _spriteImage.style.width = sprite.rect.width * 2;
            _spriteImage.style.height = sprite.rect.height * 2;
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

        void OnSpriteFieldChanged(ChangeEvent<UnityEngine.Object> evt)
        {
            Sprite selectedSprite = evt.newValue as Sprite;
            SpriteSelectionChanged?.Invoke(obj: selectedSprite);
        }
    }
}
