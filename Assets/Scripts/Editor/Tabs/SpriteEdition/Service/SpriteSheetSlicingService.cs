using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;

namespace AnimatorFactory.SpriteEdition
{
    public static class SpriteSheetSlicingService
    {
        public static bool SliceSpriteSheet(FrameGenerationData generationData)
        {
            if (generationData.Texture == null)
            {
                Debug.LogError(message: "Cannot slice sprite sheet: Texture is null");
                return false;
            }

            string assetPath = AssetDatabase.GetAssetPath(assetObject: generationData.Texture);
            if (string.IsNullOrEmpty(value: assetPath))
            {
                Debug.LogError(message: "Cannot slice sprite sheet: Asset path not found");
                return false;
            }

            TextureImporter textureImporter = AssetImporter.GetAtPath(path: assetPath) as TextureImporter;
            if (textureImporter == null)
            {
                Debug.LogError(message: $"Cannot slice sprite sheet: TextureImporter not found for {assetPath}");
                return false;
            }

            if (textureImporter.spriteImportMode == SpriteImportMode.Single)
            {
                Debug.Log(message: "Sprite is already in Single mode - no slicing needed");
                return true;
            }

            return PerformSpriteSlicing(
                textureImporter: textureImporter,
                generationData: generationData
            );
        }

        static bool PerformSpriteSlicing(
            TextureImporter textureImporter,
            FrameGenerationData generationData
        )
        {
            try
            {
                textureImporter.spriteImportMode = SpriteImportMode.Multiple;
                ConfigureTextureImportSettings(textureImporter: textureImporter);
                CreateSpriteDataUsingModernAPI(textureImporter: textureImporter, generationData: generationData);

                AssetDatabase.StartAssetEditing();
                textureImporter.SaveAndReimport();
                AssetDatabase.StopAssetEditing();
                AssetDatabase.Refresh();

                int totalSprites = generationData.Rows * generationData.Columns;
                Debug.Log(
                    message:
                    $"Sprite sheet sliced successfully: {generationData.Rows}x{generationData.Columns} = {totalSprites} sprites"
                );
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError(message: $"Failed to slice sprite sheet: {ex.Message}");
                return false;
            }
        }

        static void ConfigureTextureImportSettings(TextureImporter textureImporter)
        {
            bool wasSprite = textureImporter.textureType == TextureImporterType.Sprite;

            if (!wasSprite)
            {
                textureImporter.textureType = TextureImporterType.Sprite;
            }

            if (textureImporter.spritePixelsPerUnit <= 0)
            {
                textureImporter.spritePixelsPerUnit = 100;
            }
        }

        static void CreateSpriteDataUsingModernAPI(TextureImporter textureImporter, FrameGenerationData generationData)
        {
            SpriteDataProviderFactories factory = new();
            factory.Init();

            ISpriteEditorDataProvider dataProvider =
                factory.GetSpriteEditorDataProviderFromObject(obj: textureImporter);
            dataProvider.InitSpriteEditorDataProvider();

            List<SpriteRect> spriteRects = CreateSpriteRects(generationData: generationData);
            dataProvider.SetSpriteRects(spriteRects: spriteRects.ToArray());

            // For Unity 2021.2 and newer, we also need to register name-file ID pairs
            ISpriteNameFileIdDataProvider spriteNameFileIdDataProvider =
                dataProvider.GetDataProvider<ISpriteNameFileIdDataProvider>();
            if (spriteNameFileIdDataProvider != null)
            {
                List<SpriteNameFileIdPair> nameFileIdPairs = spriteRects
                    .Select(selector: rect => new SpriteNameFileIdPair(name: rect.name, fileId: rect.spriteID))
                    .ToList();
                spriteNameFileIdDataProvider.SetNameFileIdPairs(nameFileIdPairs: nameFileIdPairs);
            }

            dataProvider.Apply();
        }

        static List<SpriteRect> CreateSpriteRects(FrameGenerationData generationData)
        {
            var spriteRects = new List<SpriteRect>();
            int index = 0;

            // Unity's texture coordinates start from bottom-left, so we iterate from top to bottom
            for (int row = generationData.Rows - 1; row >= 0; row--)
            {
                for (int col = 0; col < generationData.Columns; col++)
                {
                    SpriteRect spriteRect = new()
                    {
                        name = GenerateSpriteName(textureBaseName: generationData.Texture.name, index: index),
                        spriteID = GUID.Generate(),
                        rect = new Rect(
                            x: col * generationData.FrameWidth,
                            y: row * generationData.FrameHeight,
                            width: generationData.FrameWidth,
                            height: generationData.FrameHeight
                        ),
                        pivot = new Vector2(x: 0.5f, y: 0.5f),
                        alignment = SpriteAlignment.Center
                    };
                    spriteRects.Add(item: spriteRect);
                    index++;
                }
            }

            return spriteRects;
        }

        static string GenerateSpriteName(string textureBaseName, int index)
        {
            return $"{textureBaseName}_{index:D2}";
        }
    }
}
