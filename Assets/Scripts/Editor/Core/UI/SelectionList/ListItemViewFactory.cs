using UnityEngine.UIElements;

namespace AnimatorFactory.Core.UI.SelectionList
{
    public abstract class ListItemViewFactory
    {
        public abstract VisualElement MakeListItem();
    }
}

/*
 * VisualElement MakeListItem()
   {
       VisualElement container = new()
       {
           style =
           {
               flexDirection = FlexDirection.Row,
               alignItems = Align.Center,
               paddingLeft = 5,
               paddingRight = 5
           }
       };

       // Sprite icon placeholder
       VisualElement iconContainer = new()
       {
           style =
           {
               width = 16,
               height = 16,
               marginRight = 5,
               backgroundColor = new Color(r: 0.3f, g: 0.3f, b: 0.3f)
           }
       };
       container.Add(child: iconContainer);

       // Sprite name label
       Label nameLabel = new()
       {
           style =
           {
               flexGrow = 1,
               fontSize = 11
           }
       };
       container.Add(child: nameLabel);

       return container;
   }

*/
