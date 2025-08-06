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
            CreateSpriteImage(container: content);
            
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
        
        void CreateSpriteImage(VisualElement container)
        {
            _spriteImage = new UnityEngine.UIElements.Image
            {
                scaleMode = ScaleMode.ScaleToFit,
                style =
                {
                    alignSelf = Align.Center,
                    display = DisplayStyle.None,
                    marginTop = 10
                }
            };
            
            container.Add(child: _spriteImage);
        }

        void OnTextureSelected(ChangeEvent<Object> evt)
        {
            Texture2D selectedTexture = evt.newValue as Texture2D;
            _controller?.OnTextureSelectionChanged(texture: selectedTexture);
        }
    }
}
