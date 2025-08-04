using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UIElements;

namespace AnimatorFactory
{
    public class SpriteKeyframeView : VisualElement
    {
        readonly Label _titleLabel;
        readonly Label _infoLabel;
        readonly ScrollView _keyframesScrollView;
        readonly VisualElement _keyframesContainer;
        readonly HelpBox _helpBox;

        public SpriteKeyframeView()
        {
            style.paddingTop = 10;
            style.display = DisplayStyle.None; // Initially hidden

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

        public void ShowSpriteKeyframes(AnimatorState state)
        {
            ClearKeyframes();

            if (state.motion is not AnimationClip clip)
            {
                ShowMessage(
                    message: "Selected state does not contain an Animation Clip.",
                    type: HelpBoxMessageType.Info
                );
                return;
            }

            AnimationSpriteInfo spriteInfo = ExtractSpriteKeyframes(clip: clip);

            if (spriteInfo.keyframes.Count == 0)
            {
                ShowMessage(message: "No sprite keyframes found in this animation.", type: HelpBoxMessageType.Info);
                return;
            }

            HideMessage();
            DisplayKeyframes(spriteInfo: spriteInfo);
            style.display = DisplayStyle.Flex;
        }

        public void Hide()
        {
            style.display = DisplayStyle.None;
            ClearKeyframes();
        }

        void DisplayKeyframes(AnimationSpriteInfo spriteInfo)
        {
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

        void ShowMessage(string message, HelpBoxMessageType type)
        {
            _helpBox.text = message;
            _helpBox.messageType = type;
            _helpBox.style.display = DisplayStyle.Flex;
            _keyframesScrollView.style.display = DisplayStyle.None;
            _infoLabel.style.display = DisplayStyle.None;
            style.display = DisplayStyle.Flex;
        }

        void HideMessage()
        {
            _helpBox.style.display = DisplayStyle.None;
            _keyframesScrollView.style.display = DisplayStyle.Flex;
            _infoLabel.style.display = DisplayStyle.Flex;
        }

        void ClearKeyframes()
        {
            _keyframesContainer.Clear();
        }

        VisualElement CreateKeyframeElement(SpriteKeyframeData keyframe, float frameRate)
        {
            VisualElement container = new VisualElement
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
                Image spriteImage = new Image
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
            Label timeLabel = new Label(text: $"F{frameNumber}")
            {
                style =
                {
                    fontSize = 9,
                    alignSelf = Align.Center,
                    color = Color.white
                }
            };
            container.Add(child: timeLabel);

            Label nameLabel = new Label(text: keyframe.sprite != null ? keyframe.sprite.name : "null")
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

        static AnimationSpriteInfo ExtractSpriteKeyframes(AnimationClip clip)
        {
            AnimationSpriteInfo info = new AnimationSpriteInfo(clip: clip);

            // Get all curves that affect sprite properties
            EditorCurveBinding[] bindings = AnimationUtility.GetObjectReferenceCurveBindings(clip: clip);

            foreach (EditorCurveBinding binding in bindings)
            {
                // Look for SpriteRenderer.sprite bindings (classID 212 is SpriteRenderer)
                if (binding.type == typeof(SpriteRenderer) && binding.propertyName == "m_Sprite")
                {
                    ObjectReferenceKeyframe[] keyframes =
                        AnimationUtility.GetObjectReferenceCurve(clip: clip, binding: binding);

                    foreach (ObjectReferenceKeyframe keyframe in keyframes)
                    {
                        Sprite sprite = keyframe.value as Sprite;
                        info.keyframes.Add(item: new SpriteKeyframeData(time: keyframe.time, sprite: sprite));
                    }
                }
            }

            // Sort by time
            info.keyframes.Sort(comparison: (a, b) => a.time.CompareTo(value: b.time));

            return info;
        }
    }
}
