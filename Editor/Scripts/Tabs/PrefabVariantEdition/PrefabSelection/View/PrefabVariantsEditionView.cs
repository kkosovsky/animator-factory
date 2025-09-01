using System;
using System.IO;
using AnimatorFactory.Core.UI;
using AnimatorFactory.PrefabHierarchy;
using UnityEngine;
using UnityEngine.UIElements;

namespace AnimatorFactory.PrefabVariants
{
    public class PrefabVariantsEditionView : VisualElement
    {
        public event Action<GameObject> PrefabSelected;

        PrefabField _prefabField;
        FolderField _sourceFolderField;
        Label _selectedItemsLabel;
        PrefabHierarchyView _hierarchyView;
        Label _hierarchyLabel;

        public PrefabHierarchyView HierarchyView => _hierarchyView;
        
        public PrefabVariantsEditionView() => CreateUI();

        public void ShowSelectedItemsLabel(int count)
        {
            _selectedItemsLabel.text = $"Selected {count} variants";
            _selectedItemsLabel.style.display = DisplayStyle.Flex;
        }

        void CreateUI()
        {
            SetStyle();
            AddPrefabSelection();
            AddHierarchySection();
            AddSelectedItemsLabel();
        }

        void AddSelectedItemsLabel()
        {
            _selectedItemsLabel = new Label
            {
                style =
                {
                    display = DisplayStyle.None
                }
            };

            Add(child: _selectedItemsLabel);
        }



        void SetStyle()
        {
            style.flexGrow = 1;
            style.backgroundColor = new Color(r: 0.3f, g: 0.3f, b: 0.3f);
        }

        void AddPrefabSelection()
        {
            _prefabField = PrefabField.Make(
                label: "Prefab Selection: ",
                OnPrefabSelectionChanged: OnPrefabSelected
            );
            Add(child: _prefabField);
        }

        void OnPrefabSelected(ChangeEvent<UnityEngine.Object> evt)
        {
            GameObject value = (GameObject)evt.newValue;
            PrefabSelected?.Invoke(obj: value);
        }

        void AddHierarchySection()
        {
            _hierarchyLabel = new Label(text: "Hierarchy:")
            {
                style =
                {
                    unityFontStyleAndWeight = FontStyle.Bold,
                    marginTop = 10,
                    marginBottom = 5,
                    display = DisplayStyle.None
                }
            };
            Add(child: _hierarchyLabel);

            _hierarchyView = new PrefabHierarchyView
            {
                style =
                {
                    display = DisplayStyle.None,
                    height = 200,
                    marginBottom = 10
                }
            };
            Add(child: _hierarchyView);
        }

        public void ShowHierarchy()
        {
            _hierarchyLabel.style.display = DisplayStyle.Flex;
            _hierarchyView.style.display = DisplayStyle.Flex;
        }

        public void HideHierarchy()
        {
            _hierarchyLabel.style.display = DisplayStyle.None;
            _hierarchyView.style.display = DisplayStyle.None;
        }
    }
}
