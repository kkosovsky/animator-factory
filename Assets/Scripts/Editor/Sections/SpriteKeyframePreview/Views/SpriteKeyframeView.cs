using UnityEngine;
using UnityEngine.UIElements;

namespace AnimatorFactory.SpriteKeyframePreview
{
    /// <summary>
    /// Pure UI component for displaying sprite keyframes.
    /// Binds to SpriteKeyframeViewModel through events.
    /// </summary>
    public class SpriteKeyframeView : VisualElement
    {
        Label _titleLabel;
        Label _infoLabel;
        ScrollView _keyframesScrollView;
        VisualElement _keyframesContainer;
        HelpBox _helpBox;

        public SpriteKeyframeView() => CreateUI();

        public void OnDataChanged(AnimationSpriteInfo spriteInfo)
        {
            HideStatus();
            DisplayKeyframes(spriteInfo: spriteInfo);
            style.display = DisplayStyle.Flex;
        }

        public void OnStatusChanged(string message, bool isError)
        {
            ShowStatus(message: message, type: isError ? HelpBoxMessageType.Error : HelpBoxMessageType.Info);
            style.display = DisplayStyle.Flex;
        }

        /// <summary>
        /// Hides the view and clears its content.
        /// </summary>
        public void Hide()
        {
            style.display = DisplayStyle.None;
            _keyframesContainer.Clear();
        }

        void CreateUI()
        {
            style.paddingTop = 10;
            style.display = DisplayStyle.None;

            _titleLabel = new Label(text: "Sprite Keyframes")
            {
                style =
                {
                    unityFontStyleAndWeight = FontStyle.Bold,
                    marginBottom = 5
                }
            };
            Add(child: _titleLabel);

            _infoLabel = new Label()
            {
                style =
                {
                    fontSize = 11,
                    color = Color.gray,
                    marginBottom = 10
                }
            };
            Add(child: _infoLabel);

            _helpBox = new HelpBox(text: "", messageType: HelpBoxMessageType.Info)
            {
                style =
                {
                    display = DisplayStyle.None,
                    marginBottom = 10
                }
            };
            Add(child: _helpBox);

            _keyframesScrollView = new ScrollView(scrollViewMode: ScrollViewMode.Horizontal)
            {
                style = { height = 80 }
            };

            _keyframesContainer = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    flexWrap = Wrap.NoWrap
                }
            };

            _keyframesScrollView.Add(child: _keyframesContainer);
            Add(child: _keyframesScrollView);
        }

        void DisplayKeyframes(AnimationSpriteInfo spriteInfo)
        {
            _keyframesContainer.Clear();
            _titleLabel.text = $"Sprite Keyframes - {spriteInfo.animationName}";
            _infoLabel.text =
                $"Duration: {spriteInfo.duration:F2}s | Frame Rate: {spriteInfo.frameRate} fps | Total Frames: {spriteInfo.totalFrames}";

            foreach (var keyframe in spriteInfo.keyframes)
            {
                VisualElement keyframeElement = CreateKeyframeElement(
                    keyframe: keyframe,
                    frameRate: spriteInfo.frameRate
                );
                _keyframesContainer.Add(child: keyframeElement);
            }
        }

        void ShowStatus(string message, HelpBoxMessageType type)
        {
            _helpBox.text = message;
            _helpBox.messageType = type;
            _helpBox.style.display = DisplayStyle.Flex;
            _keyframesScrollView.style.display = DisplayStyle.None;
            _infoLabel.style.display = DisplayStyle.None;
        }

        void HideStatus()
        {
            _helpBox.style.display = DisplayStyle.None;
            _keyframesScrollView.style.display = DisplayStyle.Flex;
            _infoLabel.style.display = DisplayStyle.Flex;
        }

        VisualElement CreateKeyframeElement(SpriteKeyframeData keyframe, float frameRate)
        {
            VisualElement container = new()
            {
                style =
                {
                    width = 60,
                    height = 70,
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

            // Sprite preview (if available)
            if (keyframe.sprite != null)
            {
                Image spriteImage = new()
                {
                    sprite = keyframe.sprite,
                    style =
                    {
                        width = 32,
                        height = 32,
                        alignSelf = Align.Center
                    }
                };
                container.Add(child: spriteImage);
            }

            // Time and frame info
            int frameNumber = Mathf.RoundToInt(f: keyframe.time * frameRate);
            Label timeLabel = new(text: $"F{frameNumber}")
            {
                style =
                {
                    fontSize = 9,
                    alignSelf = Align.Center,
                    color = Color.white
                }
            };
            container.Add(child: timeLabel);

            Label nameLabel = new(text: keyframe.sprite != null ? keyframe.sprite.name : "null")
            {
                style =
                {
                    fontSize = 8,
                    alignSelf = Align.Center,
                    color = Color.gray,
                    whiteSpace = WhiteSpace.Normal,
                    textOverflow = TextOverflow.Ellipsis
                }
            };
            container.Add(child: nameLabel);

            return container;
        }
    }
}
