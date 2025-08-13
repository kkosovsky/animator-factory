using System;
using System.IO;
using AnimatorFactory.Core.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace AnimatorFactory.PrefabVariants
{
    public class PrefabVariantCell : VisualElement
    {
        public Image prefabImage;
        public Label prefabLabel;
        public FolderField spriteSourceFolderField;
        public FolderField clipsDestinationFolderField;
        Guid _id;

        public PrefabVariantCell() => CreateUI();

        void CreateUI()
        {
            SetStyle();
            prefabImage = new Image
            {
                style =
                {
                    width = 16,
                    height = 16,
                    marginRight = 8,
                    flexShrink = 0
                }
            };
            Add(child: prefabImage);

            prefabLabel = new Label
            {
                style =
                {
                    flexGrow = 1,
                    fontSize = 12,
                    unityTextAlign = TextAnchor.MiddleLeft
                }
            };
            Add(child: prefabLabel);

            spriteSourceFolderField = new FolderField(
                labelText: "Sprite Sources:",
                initialValue: $"Assets{Path.DirectorySeparatorChar}"
            );

            clipsDestinationFolderField = new FolderField(
                labelText: "Clips Destination:",
                initialValue: $"Assets{Path.DirectorySeparatorChar}"
            );

            Add(child: spriteSourceFolderField);
            Add(child: clipsDestinationFolderField);
        }

        void SetStyle()
        {
            style.flexDirection = FlexDirection.Row;
            style.alignItems = Align.Center;
            style.paddingLeft = 5;
            style.paddingRight = 5;
            style.paddingTop = 2;
            style.paddingBottom = 2;
        }

        public void SetUp(Texture2D image, string labelText)
        {
        }

        public void SetUp(Sprite sprite, string labelText)
        {
            if (sprite != null)
            {
                prefabImage.sprite = sprite;
            }

            prefabLabel.text = labelText ?? string.Empty;
        }

        public void SetUp(
            Guid id,
            Texture2D image,
            string labelText,
            string initialSpritesSourceDir,
            string initialClipsDestinationDir,
            Action<Guid, string> onSpritesSourceDirChanged,
            Action<Guid, string> onClipsDestinationPathDirChanged
        )
        {
            _id = id;
            if (image != null)
            {
                prefabImage.image = image;
            }

            prefabLabel.text = labelText ?? string.Empty;
            spriteSourceFolderField.SetPath(path: initialSpritesSourceDir);
            spriteSourceFolderField.SetId(id: _id);
            spriteSourceFolderField.AddListener(onValueChanged: onSpritesSourceDirChanged);
            
            clipsDestinationFolderField.SetPath(path: initialClipsDestinationDir);
            clipsDestinationFolderField.SetId(id: _id);
            clipsDestinationFolderField.AddListener(onValueChanged: onClipsDestinationPathDirChanged);
        }
    }
}
