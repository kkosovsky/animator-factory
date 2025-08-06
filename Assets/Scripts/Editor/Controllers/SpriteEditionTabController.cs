using UnityEngine;
using UnityEngine.UIElements;

namespace AnimatorFactory.Editor
{
    public class SpriteEditionTabController
    {
        Image _spriteImage;

        public SpriteEditionTabController()
        {
        }

        public void SetSpriteImage(Image spriteImage)
        {
            _spriteImage = spriteImage;
        }

        public void OnTextureSelectionChanged(Texture2D texture)
        {
            if (_spriteImage == null) return;

            if (texture == null)
            {
                _spriteImage.style.display = DisplayStyle.None;
                _spriteImage.sprite = null;
                return;
            }

            Sprite sprite = CreateSpriteFromTexture(texture);
            _spriteImage.sprite = sprite;
            _spriteImage.style.width = texture.width * 2;
            _spriteImage.style.height = texture.height * 2;
            _spriteImage.style.display = DisplayStyle.Flex;
        }

        public void Dispose()
        {
            _spriteImage = null;
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
