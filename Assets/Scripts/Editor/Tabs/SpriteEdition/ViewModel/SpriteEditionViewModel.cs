using System;
using UnityEditor;
using UnityEngine;

namespace AnimatorFactory.SpriteEdition
{
    public class SpriteEditionViewModel
    {
        public event Action<Texture2D> TextureChanged;
        public event Action<string, bool> StatusChanged;
        public event Action<SpriteImportMode, int> SpriteModeChanged;

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

            var (spriteMode, spriteCount) = GetSpriteInfo(texture);
            SpriteModeChanged?.Invoke(arg1: spriteMode, arg2: spriteCount);
            
            string modeText = GetSpriteModeText(spriteMode);
            string message = spriteMode == SpriteImportMode.Multiple 
                ? $"Texture loaded: {texture.name} - Mode: {modeText} ({spriteCount} sprites)"
                : $"Texture loaded: {texture.name} - Mode: {modeText}";
                
            ShowStatus(message: message, isError: false);
        }

        public void Clear()
        {
            ClearTexture();
        }

        (SpriteImportMode mode, int count) GetSpriteInfo(Texture2D texture)
        {
            if (texture == null)
                return (SpriteImportMode.None, 0);

            string assetPath = AssetDatabase.GetAssetPath(texture);
            TextureImporter textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            
            if (textureImporter == null)
                return (SpriteImportMode.None, 0);

            int spriteCount = 1;
            if (textureImporter.spriteImportMode == SpriteImportMode.Multiple)
            {
                UnityEngine.Object[] sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(assetPath);
                spriteCount = sprites?.Length ?? 0;
            }

            return (textureImporter.spriteImportMode, spriteCount);
        }

        string GetSpriteModeText(SpriteImportMode mode)
        {
            return mode switch
            {
                SpriteImportMode.Single => "Single",
                SpriteImportMode.Multiple => "Multiple",
                _ => "Unknown"
            };
        }

        public bool IsTextureMultipleSprite(Texture2D texture)
        {
            if (texture == null)
                return false;

            string assetPath = AssetDatabase.GetAssetPath(texture);
            TextureImporter textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            
            return textureImporter?.spriteImportMode == SpriteImportMode.Multiple;
        }

        public void ChangeSpriteMode(SpriteImportMode newMode)
        {
            if (_currentTexture == null)
            {
                ShowStatus(message: "No texture selected to change mode.", isError: true);
                return;
            }

            string assetPath = AssetDatabase.GetAssetPath(_currentTexture);
            TextureImporter textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            
            if (textureImporter == null)
            {
                ShowStatus(message: "Cannot change sprite mode - texture importer not found.", isError: true);
                return;
            }

            if (textureImporter.spriteImportMode == newMode)
            {
                ShowStatus(message: $"Texture is already in {GetSpriteModeText(newMode)} mode.", isError: false);
                return;
            }

            SpriteImportMode oldMode = textureImporter.spriteImportMode;
            textureImporter.spriteImportMode = newMode;
            
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
            
            var (spriteMode, spriteCount) = GetSpriteInfo(_currentTexture);
            SpriteModeChanged?.Invoke(arg1: spriteMode, arg2: spriteCount);
            
            string message = $"Changed sprite mode from {GetSpriteModeText(oldMode)} to {GetSpriteModeText(newMode)}";
            if (newMode == SpriteImportMode.Multiple)
            {
                message += $" ({spriteCount} sprites detected)";
            }
            
            ShowStatus(message: message, isError: false);
        }

        void ClearTexture()
        {
            _currentTexture = null;
            TextureChanged?.Invoke(obj: null);
            SpriteModeChanged?.Invoke(arg1: SpriteImportMode.None, arg2: 0);
        }

        void ShowStatus(string message, bool isError)
        {
            StatusChanged?.Invoke(arg1: message, arg2: isError);
        }
    }
}
