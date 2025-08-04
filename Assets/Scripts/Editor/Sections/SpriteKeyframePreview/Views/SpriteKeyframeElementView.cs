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
        const int CONTAINER_WIDTH = 60;
        const int CONTAINER_HEIGHT = 70;
        const int SPRITE_SIZE = 32;
        const int FRAME_LABEL_FONT_SIZE = 9;
        const int NAME_LABEL_FONT_SIZE = 8;
        const int EMPTY_LABEL_FONT_SIZE = 16;

        /// <summary>
        /// Creates a new keyframe element view for the given keyframe data.
        /// </summary>
        /// <param name="keyframe">The keyframe data to display</param>
        /// <returns>A configured VisualElement representing the keyframe</returns>
        public static VisualElement Create(SpriteKeyframeData keyframe)
        {
            VisualElement container = CreateContainer();
            
            if (keyframe.sprite != null)
            {
                container.Add(CreateSpriteImage(keyframe.sprite));
            }
            else
            {
                container.Add(CreateEmptyPlaceholder());
            }

            container.Add(CreateFrameLabel(keyframe.index));
            container.Add(CreateNameLabel(keyframe.sprite));

            return container;
        }

        static VisualElement CreateContainer()
        {
            return new VisualElement
            {
                style =
                {
                    width = CONTAINER_WIDTH,
                    height = CONTAINER_HEIGHT,
                    marginRight = 5,
                    backgroundColor = new Color(0.3f, 0.3f, 0.3f, 0.5f),
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

        static Image CreateSpriteImage(Sprite sprite)
        {
            return new Image
            {
                sprite = sprite,
                style =
                {
                    width = SPRITE_SIZE,
                    height = SPRITE_SIZE,
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
                    backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.8f),
                    borderTopWidth = 1,
                    borderBottomWidth = 1,
                    borderLeftWidth = 1,
                    borderRightWidth = 1,
                    borderTopColor = new Color(0.5f, 0.5f, 0.5f, 0.8f),
                    borderBottomColor = new Color(0.5f, 0.5f, 0.5f, 0.8f),
                    borderLeftColor = new Color(0.5f, 0.5f, 0.5f, 0.8f),
                    borderRightColor = new Color(0.5f, 0.5f, 0.5f, 0.8f)
                }
            };

            Label emptyLabel = new("—")
            {
                style =
                {
                    alignSelf = Align.Center,
                    unityTextAlign = TextAnchor.MiddleCenter,
                    fontSize = EMPTY_LABEL_FONT_SIZE,
                    color = new Color(0.6f, 0.6f, 0.6f, 1.0f),
                    flexGrow = 1
                }
            };

            placeholder.Add(emptyLabel);
            return placeholder;
        }

        static Label CreateFrameLabel(int frameIndex)
        {
            int frameNumber = frameIndex + 1;
            return new Label($"F{frameNumber}")
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
            return new Label(displayName)
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
    }
}
