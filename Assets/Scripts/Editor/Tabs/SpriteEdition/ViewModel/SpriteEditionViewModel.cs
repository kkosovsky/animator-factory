using System;
using UnityEditor;
using UnityEngine;

namespace AnimatorFactory.SpriteEdition
{
    public class SpriteEditionViewModel
    {
        public event Action<Sprite> SpriteChanged;
        public event Action<string, bool> StatusChanged;

        Sprite _currentSprite;

        public Sprite CurrentSprite => _currentSprite;

        public void LoadSprite(Sprite sprite)
        {
            if (sprite == null)
            {
                ClearSprite();
                ShowStatus(message: "No sprite selected.", isError: false);
                return;
            }

            _currentSprite = sprite;
            SpriteChanged?.Invoke(obj: sprite);

            string spriteMode = GetSpriteMode(sprite);
            ShowStatus(message: $"Sprite loaded: {sprite.name} - Mode: {spriteMode}", isError: false);
        }

        public void Clear()
        {
            ClearSprite();
        }

        string GetSpriteMode(Sprite sprite)
        {
            if (sprite == null || sprite.texture == null)
                return "Unknown";

            string assetPath = AssetDatabase.GetAssetPath(sprite.texture);
            TextureImporter textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            
            if (textureImporter == null)
                return "Unknown";

            return textureImporter.spriteImportMode switch
            {
                SpriteImportMode.Single => "Single",
                SpriteImportMode.Multiple => "Multiple",
                SpriteImportMode.Polygon => "Polygon",
                _ => "Unknown"
            };
        }

        public bool IsSpriteMultiple(Sprite sprite)
        {
            if (sprite == null || sprite.texture == null)
                return false;

            string assetPath = AssetDatabase.GetAssetPath(sprite.texture);
            TextureImporter textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            
            return textureImporter?.spriteImportMode == SpriteImportMode.Multiple;
        }

        void ClearSprite()
        {
            _currentSprite = null;
            SpriteChanged?.Invoke(obj: null);
        }

        void ShowStatus(string message, bool isError)
        {
            StatusChanged?.Invoke(arg1: message, arg2: isError);
        }
    }
}