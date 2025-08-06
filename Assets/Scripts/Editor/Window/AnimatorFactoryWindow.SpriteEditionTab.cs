using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace AnimatorFactory
{
    public partial class AnimatorFactoryWindow
    {
        VisualElement CreatSpriteEditionContent()
        {
            VisualElement content = new()
            {
                style = { flexGrow = 1 }
            };

            CreateSpriteSelectionSection(container: content);
            CreateSpriteImage(sprite: null, width: 0, height: 0, container: content);
            
            return content;
        }
        
        void CreateSpriteSelectionSection(VisualElement container)
        {
            _prefabField = new ObjectField(label: Strings.textureSelectionLabel)
            {
                objectType = typeof(Texture2D),
                allowSceneObjects = false
            };

            _prefabField.RegisterValueChangedCallback(callback: OnTextureSelected);
            container.Add(child: _prefabField);
        }
        
        void CreateSpriteImage(Sprite sprite, float width, float height, VisualElement container)
        {
            Image image = new()
            {
                sprite = sprite,
                scaleMode = ScaleMode.ScaleToFit,
                style =
                {
                    width = width,
                    height = height,
                    alignSelf = Align.Center,
                    display = DisplayStyle.None
                }
            };
            
            container.Add(child: image);
        }

        void OnTextureSelected(ChangeEvent<Object> evt)
        {
            Texture2D selectedTexture = evt.newValue as Texture2D;
            _controller?.OnTextureSelectionChanged(texture: selectedTexture);
        }
    }
}
