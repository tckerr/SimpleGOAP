using Priority_Queue;

namespace PlainGOAP
{
    public class StateNode<T> : StablePriorityQueueNode where T : IState
    {
        public IAction<T> SourceAction;
        public StateNode<T> Parent;
        public T State;
        public int GCost;

        public StateNode(T state, StateNode<T> parent, IAction<T> sourceAction)
        {
            SourceAction = sourceAction;
            Parent = parent;
            State = state;
            GCost = (parent?.GCost ?? 0) + 1;
        }

        public int ActionCost => SourceAction?.ActionCost ?? 0;

        public int GetHash() => State.GetUniqueHashForState();
    }
}
