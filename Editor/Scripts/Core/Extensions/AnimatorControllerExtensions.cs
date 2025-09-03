using System.Linq;
using UnityEditor.Animations;

namespace AnimatorFactory
{
    public static class AnimatorControllerExtensions
    {
        public static bool HasState(this AnimatorController self, string stateName)
        {
            if (self == null)
            {
                return false;
            }

            foreach (AnimatorControllerLayer layer in self.layers)
            {
                AnimatorStateMachine stateMachine = layer.stateMachine;

                if (stateMachine.states.Any(childState => childState.state.name.Equals(stateName)))
                {
                    return true;
                }

                if (HasStateInSubStateMachines(stateMachine: stateMachine, stateName: stateName))
                {
                    return true;
                }
            }

            return false;
        }

        static bool HasStateInSubStateMachines(AnimatorStateMachine stateMachine, string stateName)
        {
            foreach (ChildAnimatorStateMachine childStateMachine in stateMachine.stateMachines)
            {
                AnimatorStateMachine subStateMachine = childStateMachine.stateMachine;

                if (subStateMachine.states.Any(childState => childState.state.name.Equals(stateName)))
                {
                    return true;
                }

                if (HasStateInSubStateMachines(stateMachine: subStateMachine, stateName: stateName))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
