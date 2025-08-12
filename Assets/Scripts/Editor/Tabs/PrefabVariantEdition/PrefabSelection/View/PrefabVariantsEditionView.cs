using System;
using System.IO;
using AnimatorFactory.Core.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace AnimatorFactory.PrefabVariants
{
    public class PrefabVariantsEditionView : VisualElement
    {
        public event Action<GameObject> PrefabSelected;
        public event Action<string> SpritesSourceFolderChanged;
        public event Action GenerateButtonClicked;

        PrefabField _prefabField;
        FolderField _sourceFolderField;
        Label _selectedItemsLabel;
        Button _generateButton;

        public PrefabVariantsEditionView() => CreateUI();

        public void ShowSelectedItemsLabel(int count)
        {
            _selectedItemsLabel.text = $"Selected {count} variants";
            _selectedItemsLabel.style.display = DisplayStyle.Flex;
            _generateButton.style.display = DisplayStyle.Flex;
        }

        void CreateUI()
        {
            SetStyle();
            AddPrefabSelection();
            AddSourceFolderField();
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

        void AddSourceFolderField()
        {
            _sourceFolderField = new FolderField(
                labelText: "Variants Sprites:",
                initialValue: $"Assets{Path.DirectorySeparatorChar}"
            );
            Add(child: _sourceFolderField);
            _sourceFolderField.DestinationFolderChanged += OnSourceFolderChanged;
        }

        void OnPrefabSelected(ChangeEvent<UnityEngine.Object> evt)
        {
            GameObject value = (GameObject)evt.newValue;
            PrefabSelected?.Invoke(obj: value);
        }

        void OnSourceFolderChanged(string path) => SpritesSourceFolderChanged?.Invoke(obj: path);

        void OnGenerateButtonClicked() => GenerateButtonClicked?.Invoke();
    }
}
