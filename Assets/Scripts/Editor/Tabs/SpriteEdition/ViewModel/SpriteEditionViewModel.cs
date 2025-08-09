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

            (SpriteImportMode spriteMode, int spriteCount) = GetSpriteInfo(texture: texture);
            SpriteModeChanged?.Invoke(arg1: spriteMode, arg2: spriteCount);

            string modeText = GetSpriteModeText(mode: spriteMode);
            string message = spriteMode == SpriteImportMode.Multiple
                ? $"Texture loaded: {texture.name} - Mode: {modeText} ({spriteCount} sprites)"
                : $"Texture loaded: {texture.name} - Mode: {modeText}";

            ShowStatus(message: message, isError: false);
        }

        static (SpriteImportMode mode, int count) GetSpriteInfo(Texture2D texture)
        {
            if (texture == null)
            {
                return (SpriteImportMode.None, 0);
            }

            string assetPath = AssetDatabase.GetAssetPath(assetObject: texture);
            TextureImporter textureImporter = AssetImporter.GetAtPath(path: assetPath) as TextureImporter;

            if (textureImporter == null)
            {
                return (SpriteImportMode.None, 0);
            }

            int spriteCount = 1;
            if (textureImporter.spriteImportMode == SpriteImportMode.Multiple)
            {
                UnityEngine.Object[] sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(assetPath: assetPath);
                spriteCount = sprites?.Length ?? 0;
            }

            return (textureImporter.spriteImportMode, spriteCount);
        }

        static string GetSpriteModeText(SpriteImportMode mode)
        {
            return mode switch
            {
                SpriteImportMode.Single => "Single",
                SpriteImportMode.Multiple => "Multiple",
                _ => "Unknown"
            };
        }

        public void ChangeSpriteMode(SpriteImportMode newMode)
        {
            if (_currentTexture == null)
            {
                ShowStatus(message: "No texture selected to change mode.", isError: true);
                return;
            }

            string assetPath = AssetDatabase.GetAssetPath(assetObject: _currentTexture);
            TextureImporter textureImporter = AssetImporter.GetAtPath(path: assetPath) as TextureImporter;

            if (textureImporter == null)
            {
                ShowStatus(message: "Cannot change sprite mode - texture importer not found.", isError: true);
                return;
            }

            if (textureImporter.spriteImportMode == newMode)
            {
                ShowStatus(message: $"Texture is already in {GetSpriteModeText(mode: newMode)} mode.", isError: false);
                return;
            }

            SpriteImportMode oldMode = textureImporter.spriteImportMode;
            textureImporter.spriteImportMode = newMode;

            AssetDatabase.ImportAsset(path: assetPath, options: ImportAssetOptions.ForceUpdate);

            (SpriteImportMode spriteMode, int spriteCount) = GetSpriteInfo(texture: _currentTexture);
            SpriteModeChanged?.Invoke(arg1: spriteMode, arg2: spriteCount);

            string message =
                $"Changed sprite mode from {GetSpriteModeText(mode: oldMode)} to {GetSpriteModeText(mode: newMode)}";
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

        public void GenerateFrames(FrameGenerationData generationData)
        {
            if (generationData.Texture == null)
            {
                ShowStatus(message: "Cannot generate frames: No texture selected.", isError: true);
                return;
            }

            (SpriteImportMode currentMode, _) = GetSpriteInfo(texture: generationData.Texture);

            if (currentMode == SpriteImportMode.Single)
            {
                ShowStatus(message: "Texture is in Single mode - no frame generation needed.", isError: false);
                return;
            }

            string processingMessage = $"Generating {generationData.Rows}x{generationData.Columns} frames ";
            processingMessage += $"(Frame size: {generationData.FrameWidth}x{generationData.FrameHeight})...";
            ShowStatus(message: processingMessage, isError: false);

            bool success = SpriteSheetSlicingService.SliceSpriteSheet(generationData: generationData);

            if (success)
            {
                (SpriteImportMode newMode, int newSpriteCount) = GetSpriteInfo(texture: generationData.Texture);
                SpriteModeChanged?.Invoke(arg1: newMode, arg2: newSpriteCount);

                string successMessage =
                    $"Successfully generated {newSpriteCount} frames from {generationData.Texture.name}";

                ShowStatus(message: successMessage, isError: false);
            }
            else
            {
                ShowStatus(message: "Failed to generate frames. Check console for details.", isError: true);
            }
        }

        void ShowStatus(string message, bool isError)
        {
            StatusChanged?.Invoke(arg1: message, arg2: isError);
        }
    }
}
