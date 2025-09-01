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
        public event Action GenerateButtonClicked;

        PrefabField _prefabField;
        FolderField _sourceFolderField;
        Label _selectedItemsLabel;
        PrefabHierarchyView _hierarchyView;
        Label _hierarchyLabel;
        Button _generateButton;

        public PrefabHierarchyView HierarchyView => _hierarchyView;
        
        public PrefabVariantsEditionView() => CreateUI();

        public void ShowSelectedItemsLabel(int count)
        {
            _selectedItemsLabel.text = $"Selected {count} variants";
            _selectedItemsLabel.style.display = DisplayStyle.Flex;
        }

        public void ShowGenerateButton()
        {
            _generateButton.style.display = DisplayStyle.Flex;
        }
        
        public void HideGenerateButton()
        {
            _generateButton.style.display = DisplayStyle.None;
        }

        void CreateUI()
        {
            SetStyle();
            AddPrefabSelection();
            AddHierarchySection();
            AddSelectedItemsLabel();
            AddGenerateButton();
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

        void AddGenerateButton()
        {
            _generateButton = new Button(clickEvent: OnGenerateButtonClicked)
            {
                text = "Generate",
                style =
                {
                    display = DisplayStyle.None,
                    height = 24.0f
                }
            };

            Add(child: _generateButton);
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

        void OnGenerateButtonClicked() => GenerateButtonClicked?.Invoke();

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
