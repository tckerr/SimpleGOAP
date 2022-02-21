using Priority_Queue;

namespace SimpleGOAP
{
    internal class StateNode<T> : StablePriorityQueueNode
    {
        internal IAction<T> SourceAction;
        internal StateNode<T> Parent;
        internal T ResultingState;

        internal StateNode(T state, StateNode<T> parent, IAction<T> sourceAction)
        {
            SourceAction = sourceAction;
            Parent = parent;
            ResultingState = state;
        }

        internal int GetActionCost(T state) => SourceAction?.GetCost(state) ?? 0;
    }
}
