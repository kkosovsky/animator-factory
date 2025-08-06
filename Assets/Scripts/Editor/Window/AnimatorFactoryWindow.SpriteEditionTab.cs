using AnimatorFactory.SpriteEdition;
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

            CreateSpriteEditionView(container: content);
            CreateSpriteSelectionSection(container: content);
            
            return content;
        }
        
        void CreateSpriteSelectionSection(VisualElement container)
        {
            ObjectField textureField = new(label: Strings.textureSelectionLabel)
            {
                objectType = typeof(Texture2D),
                allowSceneObjects = false
            };

            textureField.RegisterValueChangedCallback(callback: OnTextureSelected);
            container.Insert(index: 0, element: textureField);

            _spriteEditionView?.SetTextureSelectionField(textureField: textureField);
        }
        
        void CreateSpriteEditionView(VisualElement container)
        {
            _spriteEditionView = new SpriteEditionView();
            container.Add(child: _spriteEditionView);
        }

        void OnTextureSelected(ChangeEvent<Object> evt)
        {
            Texture2D selectedTexture = evt.newValue as Texture2D;
            _controller?.OnTextureSelectionChanged(texture: selectedTexture);
        }
    }
}
