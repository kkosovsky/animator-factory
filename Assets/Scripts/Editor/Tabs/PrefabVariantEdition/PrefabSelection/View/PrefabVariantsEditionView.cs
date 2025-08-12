using System;
using AnimatorFactory.Core.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace AnimatorFactory.PrefabVariants
{
    public class PrefabVariantsEditionView : VisualElement
    {
        public event Action<GameObject> PrefabSelected;
        public event Action<string> DestinationChanged;

        PrefabField _prefabField;
        FolderField _sourceFolderField;

        public PrefabVariantsEditionView() => CreateUI();

        void CreateUI()
        {
            SetStyle();
            AddPrefabSelection();
            AddSourceFolderField();
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
            _sourceFolderField = new FolderField(labelText: "Variants Sprites:");
            Add(child: _sourceFolderField);
            _sourceFolderField.DestinationFolderChanged += OnSourceFolderChanged;
        }

        void OnPrefabSelected(ChangeEvent<UnityEngine.Object> evt)
        {
            GameObject value = (GameObject)evt.newValue;
            PrefabSelected?.Invoke(obj: value);
        }

        void OnSourceFolderChanged(string path) => DestinationChanged?.Invoke(obj: path);
    }
}
