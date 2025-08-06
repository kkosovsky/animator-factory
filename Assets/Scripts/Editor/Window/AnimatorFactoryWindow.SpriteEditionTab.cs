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
            ObjectField spriteField = new(label: Strings.spriteSelectionLabel)
            {
                objectType = typeof(Sprite),
                allowSceneObjects = false
            };

            spriteField.RegisterValueChangedCallback(callback: OnSpriteSelected);
            container.Insert(index: 0, element: spriteField);

            _spriteEditionView?.SetSpriteSelectionField(spriteField: spriteField);
        }
        
        void CreateSpriteEditionView(VisualElement container)
        {
            _spriteEditionView = new SpriteEditionView();
            container.Add(child: _spriteEditionView);
        }

        void OnSpriteSelected(ChangeEvent<Object> evt)
        {
            Sprite selectedSprite = evt.newValue as Sprite;
            _controller?.OnSpriteSelectionChanged(sprite: selectedSprite);
        }
    }
}
