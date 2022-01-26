using Priority_Queue;

namespace PlainGOAP
{
    public class StateNode<TKey, TVal> : StablePriorityQueueNode
    {
        public IAction<TKey, TVal> SourceAction;
        public StateNode<TKey, TVal> Parent;
        public IState<TKey, TVal> State;
        public int GCost;

        public StateNode(IState<TKey, TVal> state, StateNode<TKey, TVal> parent, IAction<TKey, TVal> sourceAction)
        {
            SourceAction = sourceAction;
            Parent = parent;
            State = state;
            GCost = (parent?.GCost ?? 0) + 1;
        }

        public int ActionCost => SourceAction?.ActionCost ?? 0;

        public int GetHash() => State.GetUniqueHash();
    }
}
