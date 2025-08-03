using UnityEngine.UIElements;

namespace AnimatorFactory
{
    public partial class AnimatorFactoryWindow
    {
        IMGUIContainer _imguiContainer;
        PrefabHierarchyListView _listView;

        void CreateUIElements()
        {
            rootVisualElement.Clear();

            _imguiContainer = new IMGUIContainer(onGUIHandler: OnIMGUI)
            {
                style = { 
                    flexShrink = 0,
                    paddingTop = 10,
                    paddingBottom = 10,
                    paddingLeft = 10,
                    paddingRight = 10
                }
            };
            rootVisualElement.Add(child: _imguiContainer);

            _listView = new PrefabHierarchyListView();
            rootVisualElement.Add(child: _listView);
        }

        void OnIMGUI()
        {
            if (_serializedObject == null)
            {
                return;
            }
            _serializedObject.Update();
            DrawPrefabSelection();
            _serializedObject.ApplyModifiedProperties();
        }
    }
}
