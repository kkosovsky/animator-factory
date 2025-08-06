using System;
using UnityEngine;

namespace AnimatorFactory.SpriteEdition
{
    public class SpriteEditionViewModel
    {
        public event Action<Texture2D> TextureChanged;
        public event Action<string, bool> StatusChanged;

        Texture2D _currentTexture;

        public Texture2D CurrentTexture => _currentTexture;

        public void LoadTexture(Texture2D texture)
        {
            if (texture == null)
            {
                ClearTexture();
                ShowStatus(message: "No texture selected.", isError: false);
                return;
            }

            _currentTexture = texture;
            TextureChanged?.Invoke(obj: texture);
            ShowStatus(message: $"Texture loaded: {texture.name} ({texture.width}x{texture.height})", isError: false);
        }

        public void Clear()
        {
            ClearTexture();
        }

        void ClearTexture()
        {
            _currentTexture = null;
            TextureChanged?.Invoke(obj: null);
        }

        void ShowStatus(string message, bool isError)
        {
            StatusChanged?.Invoke(arg1: message, arg2: isError);
        }
    }
}