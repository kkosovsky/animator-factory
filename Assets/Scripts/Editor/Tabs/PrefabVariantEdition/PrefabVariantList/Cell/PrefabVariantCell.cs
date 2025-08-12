using UnityEngine;
using UnityEngine.UIElements;

namespace AnimatorFactory.PrefabVariants
{
    public class PrefabVariantCell : VisualElement
    {
        public Image prefabImage;
        public Label prefabLabel;

        public PrefabVariantCell()
        {
            CreateUI();
        }

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
            if (image != null)
                prefabImage.image = image;

            prefabLabel.text = labelText ?? string.Empty;
        }

        public void SetUp(Sprite sprite, string labelText)
        {
            if (sprite != null)
            {
                prefabImage.sprite = sprite;
            }

            prefabLabel.text = labelText ?? string.Empty;
        }
    }
}
