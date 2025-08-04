using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace AnimatorFactory
{
    public partial class AnimatorFactoryWindow
    {
        PrefabHierarchyListView _listView;
        AnimatorStatesView _animatorStatesView;
        ObjectField _prefabField;

        void CreateUIElements()
        {
            rootVisualElement.Clear();
            VisualElement container = new()
            {
                style =
                {
                    paddingTop = 10,
                    paddingBottom = 10,
                    paddingLeft = 10,
                    paddingRight = 10,
                    flexGrow = 1
                }
            };
            rootVisualElement.Add(child: container);

            AddPrefabSelectionView(container: container);
            AddHierarchyListView(container: container);
            AddAnimatorStatesView(container: container);
        }

        void AddPrefabSelectionView(VisualElement container)
        {
            _prefabField = new ObjectField(label: Strings.prefabSelectionLabel)
            {
                objectType = typeof(GameObject),
                allowSceneObjects = false
            };
            _prefabField.RegisterValueChangedCallback(callback: OnPrefabSelectionChanged);
            container.Add(child: _prefabField);
        }

        void AddHierarchyListView(VisualElement container)
        {
            _listView = new PrefabHierarchyListView();
            container.Add(child: _listView);
            _listView.AddListener(onSelectItem: OnHierarchyItemSelected);
        }

        void AddAnimatorStatesView(VisualElement container)
        {
            _animatorStatesView = new AnimatorStatesView();
            container.Add(child: _animatorStatesView);
            _animatorStatesView.Hide();
        }

        void OnPrefabSelectionChanged(ChangeEvent<Object> evt)
        {
            GameObject selectedPrefab = evt.newValue as GameObject;

            if (selectedPrefab == null)
            {
                _listView.Reset();
                _animatorStatesView.Hide();
                return;
            }

            List<PrefabHierarchyListItem> hierarchyNodes =
                HierarchyBuilder.BuildHierarchy(selectedPrefab: selectedPrefab);
            _listView.Refresh(hierarchyNodes: hierarchyNodes);
            _animatorStatesView.Hide();
        }

        void OnHierarchyItemSelected(PrefabHierarchyListItem item)
        {
            _animatorStatesView.ShowAnimatorStates(item: item);
        }
    }
}
