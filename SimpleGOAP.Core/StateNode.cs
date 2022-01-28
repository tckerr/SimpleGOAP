using Priority_Queue;

namespace SimpleGOAP
{
    internal class StateNode<T> : StablePriorityQueueNode
    {
        internal IAction<T> SourceAction;
        internal StateNode<T> Parent;
        internal T State;

        internal StateNode(T state, StateNode<T> parent, IAction<T> sourceAction)
        {
            SourceAction = sourceAction;
            Parent = parent;
            State = state;
        }

        internal int ActionCost => SourceAction?.ActionCost ?? 0;
    }
}
