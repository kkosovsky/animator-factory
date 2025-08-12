using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace AnimatorFactory.Core.UI
{
    public class FolderField : VisualElement
    {
        /// <summary>
        /// Fired when destination folder is changed by user.
        /// </summary>
        public event Action<string> DestinationFolderChanged;

        TextField _destinationFolderField;
        Button _browseFolderButton;

        public FolderField() => CreateGUI();

        void CreateGUI()
        {
            VisualElement folderRow = new()
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    alignItems = Align.Center,
                    marginBottom = 10,
                    width = Length.Percent(value: 100)
                }
            };

            Label folderLabel = new(text: Strings.destination)
            {
                style =
                {
                    fontSize = 11,
                    color = Color.gray,
                    marginRight = 5,
                    width = 80
                }
            };
            folderRow.Add(child: folderLabel);

            _destinationFolderField = new TextField
            {
                value = Strings.assetsPath,
                style =
                {
                    width = 200,
                    marginRight = 5
                }
            };
            _destinationFolderField.RegisterValueChangedCallback(callback: OnDestinationFolderChanged);
            folderRow.Add(child: _destinationFolderField);

            _browseFolderButton = new Button
            {
                text = Strings.browse,
                style =
                {
                    width = 70,
                    height = 20
                }
            };
            _browseFolderButton.clicked += OnBrowseFolderClicked;
            folderRow.Add(child: _browseFolderButton);

            Add(child: folderRow);
        }

        void OnDestinationFolderChanged(ChangeEvent<string> evt)
        {
            string newValue = evt.newValue;
            if (string.IsNullOrEmpty(value: newValue))
            {
                return;
            }

            if (!newValue.EndsWith(value: Path.DirectorySeparatorChar))
            {
                _destinationFolderField.value = $"{newValue}{Path.DirectorySeparatorChar}";
            }

            DestinationFolderChanged?.Invoke(obj: newValue);
        }

        void OnBrowseFolderClicked()
        {
            string newValue = _destinationFolderField.value;
            string currentFolder = newValue;
            if (string.IsNullOrEmpty(value: currentFolder))
            {
                currentFolder = Strings.assetsPath;
            }

            string selectedFolder = EditorUtility.SaveFolderPanel(
                title: Strings.selectDestinationFolder,
                folder: currentFolder,
                defaultName: string.Empty
            );

            if (string.IsNullOrEmpty(value: selectedFolder))
            {
                return;
            }

            string relativePath = FileUtil.GetProjectRelativePath(path: selectedFolder);
            if (string.IsNullOrEmpty(value: relativePath))
            {
                return;
            }

            if (!newValue.EndsWith(value: Path.DirectorySeparatorChar))
            {
                _destinationFolderField.value = $"{relativePath}{Path.DirectorySeparatorChar}";
            }

            DestinationFolderChanged?.Invoke(obj: _destinationFolderField.value);
        }

        static class Strings
        {
            internal const string selectDestinationFolder = "Select Destination Folder";
            internal const string destination = "Destination:";
            internal const string browse = "Browse...";
            internal static readonly string assetsPath = $"Assets{Path.DirectorySeparatorChar}";
        }
    }
}
