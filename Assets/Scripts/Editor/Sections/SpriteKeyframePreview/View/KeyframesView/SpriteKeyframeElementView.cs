using UnityEngine;
using UnityEngine.UIElements;

namespace AnimatorFactory.SpriteKeyframePreview
{
    /// <summary>
    /// Represents a single keyframe visual element in the sprite keyframe preview.
    /// Displays the sprite, frame number, and sprite name for each keyframe.
    /// </summary>
    public class KeyframeElementView : VisualElement
    {
        const int CONTAINER_HEIGHT_OFFSET = 24;
        const int SPRITE_SIZE = 64;
        const int FRAME_LABEL_FONT_SIZE = 12;
        const int NAME_LABEL_FONT_SIZE = 10;
        const int EMPTY_LABEL_FONT_SIZE = 24;

        /// <summary>
        /// Creates a new keyframe element view for the given keyframe data.
        /// </summary>
        /// <param name="keyframe">The keyframe data to display</param>
        /// <returns>A configured VisualElement representing the keyframe</returns>
        public KeyframeElementView(SpriteKeyframeData keyframe)
        {
            (float width, float height) = CalculateOptimalSpriteSize(sprite: keyframe.sprite);
            VisualElement container = CreateContainer(spriteWidth: width, spriteHeight: height);
        
            Debug.Log($"width: {width}, height: {height}");
            
            VisualElement child = keyframe.sprite != null
                ? CreateSpriteImage(sprite: keyframe.sprite, width: width, height: height)
                : CreateEmptyPlaceholder();

            container.Add(child: child);

            container.Add(child: CreateFrameLabel(frameIndex: keyframe.index));
            container.Add(child: CreateNameLabel(sprite: keyframe.sprite));

            Add(child: container);
        }

        static VisualElement CreateContainer(float spriteWidth, float spriteHeight)
        {
            return new VisualElement
            {
                style =
                {
                    width = spriteWidth,
                    height = spriteHeight + CONTAINER_HEIGHT_OFFSET,
                    marginRight = 5,
                    backgroundColor = new Color(r: 0.3f, g: 0.3f, b: 0.3f, a: 0.5f),
                    borderTopWidth = 1,
                    borderBottomWidth = 1,
                    borderLeftWidth = 1,
                    borderRightWidth = 1,
                    borderTopColor = Color.gray,
                    borderBottomColor = Color.gray,
                    borderLeftColor = Color.gray,
                    borderRightColor = Color.gray,
                    paddingTop = 2,
                    paddingBottom = 2,
                    paddingLeft = 2,
                    paddingRight = 2
                }
            };
        }

        static Image CreateSpriteImage(Sprite sprite, float width, float height)
        {
            return new Image
            {
                sprite = sprite,
                scaleMode = ScaleMode.ScaleToFit,
                style =
                {
                    width = width,
                    height = height,
                    alignSelf = Align.Center
                }
            };
        }

        static VisualElement CreateEmptyPlaceholder()
        {
            VisualElement placeholder = new()
            {
                style =
                {
                    width = SPRITE_SIZE,
                    height = SPRITE_SIZE,
                    alignSelf = Align.Center,
                    backgroundColor = new Color(r: 0.2f, g: 0.2f, b: 0.2f, a: 0.8f),
                    borderTopWidth = 1,
                    borderBottomWidth = 1,
                    borderLeftWidth = 1,
                    borderRightWidth = 1,
                    borderTopColor = new Color(r: 0.5f, g: 0.5f, b: 0.5f, a: 0.8f),
                    borderBottomColor = new Color(r: 0.5f, g: 0.5f, b: 0.5f, a: 0.8f),
                    borderLeftColor = new Color(r: 0.5f, g: 0.5f, b: 0.5f, a: 0.8f),
                    borderRightColor = new Color(r: 0.5f, g: 0.5f, b: 0.5f, a: 0.8f)
                }
            };

            Label emptyLabel = new(text: "—")
            {
                style =
                {
                    alignSelf = Align.Center,
                    unityTextAlign = TextAnchor.MiddleCenter,
                    fontSize = EMPTY_LABEL_FONT_SIZE,
                    color = new Color(r: 0.6f, g: 0.6f, b: 0.6f, a: 1.0f),
                    flexGrow = 1
                }
            };

            placeholder.Add(child: emptyLabel);
            return placeholder;
        }

        static Label CreateFrameLabel(int frameIndex)
        {
            int frameNumber = frameIndex + 1;
            return new Label(text: $"F{frameNumber}")
            {
                style =
                {
                    fontSize = FRAME_LABEL_FONT_SIZE,
                    alignSelf = Align.Center,
                    color = Color.white
                }
            };
        }

        static Label CreateNameLabel(Sprite sprite)
        {
            string displayName = sprite != null ? sprite.name : "empty";
            return new Label(text: displayName)
            {
                style =
                {
                    fontSize = NAME_LABEL_FONT_SIZE,
                    alignSelf = Align.Center,
                    color = Color.gray,
                    whiteSpace = WhiteSpace.Normal,
                    textOverflow = TextOverflow.Ellipsis
                }
            };
        }

        static (float width, float height) CalculateOptimalSpriteSize(Sprite sprite)
        {
            if (sprite == null || sprite.texture == null)
            {
                return (SPRITE_SIZE, SPRITE_SIZE);
            }

            Rect spriteRect = sprite.rect;
            float spriteWidth = spriteRect.width;
            float spriteHeight = spriteRect.height;
            float sizeMultiplier = 2.0f;

            if (spriteWidth <= 0 || spriteHeight <= 0)
            {
                return (SPRITE_SIZE, SPRITE_SIZE);
            }

            return (spriteWidth * sizeMultiplier, spriteHeight * sizeMultiplier);
        }
    }
}
