using UnityEngine;

namespace AnimatorFactory
{
    public partial class AnimatorFactoryWindow
    {
        void DrawAnimatorUI(PrefabHierarchyListItem item)
        {
            if (!item.gameObject.HasComponent<Animator>())
            {
                return;
            }

            Animator animator = item.gameObject.GetComponent<Animator>();
            // TODO: Get all states of the animator. Show them as a horizontal, scrollable(if needed) list of buttons.
        }
    }
}
