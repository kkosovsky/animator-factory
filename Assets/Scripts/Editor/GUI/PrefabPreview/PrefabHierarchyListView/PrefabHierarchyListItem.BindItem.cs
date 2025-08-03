using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace AnimatorFactory
{
    public partial class PrefabHierarchyListView
    {
        void BindItem(VisualElement element, int index)
        {
            if (index >= _hierarchyNodes.Count)
            {
                return;
            }

            HierarchyNode node = _hierarchyNodes[index: index];

            VisualElement indentSpace = element.Q<VisualElement>(name: "indent-space");
            indentSpace.style.width = node.depth * 20;

            Image icon = element.Q<Image>(name: "game-object-icon");
            Texture2D gameObjectIcon = AssetPreview.GetMiniThumbnail(obj: node.gameObject);
            if (gameObjectIcon != null)
            {
                icon.image = gameObjectIcon;
            }

            Label nameLabel = element.Q<Label>(name: "name-label");
            nameLabel.text = node.name;

            VisualElement iconsContainer = element.Q<VisualElement>(name: "icons-container");
            iconsContainer.Clear();

            if (node.gameObject.GetComponent<SpriteRenderer>() != null)
            {
                Custom.AddComponentIcon<SpriteRenderer>(
                    container: iconsContainer,
                    tooltip: Strings.hasSpriteRendererComponent
                );
            }

            if (node.gameObject.GetComponent<Animator>() != null)
            {
                Custom.AddComponentIcon<Animator>(container: iconsContainer, tooltip: Strings.hasAnimatorComponent);
            }
        }
    }
}
