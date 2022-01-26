using System.Linq;
using Priority_Queue;

namespace PlainGOAP
{
    public class StateNode<TKey, TVal> : StablePriorityQueueNode
    {
        public IAction<TKey, TVal> SourceAction;
        public StateNode<TKey, TVal> Parent;
        public State<TKey, TVal> State;
        public int GCost;

        public StateNode(State<TKey, TVal> state, StateNode<TKey, TVal> parent, IAction<TKey, TVal> sourceAction)
        {
            SourceAction = sourceAction;
            Parent = parent;
            State = state;
            GCost = (parent?.GCost ?? 0) + 1;
        }

        public int ActionCost => SourceAction?.ActionCost ?? 0;

        public bool IsComplete(State<TKey, TVal> worldState) => worldState.ListFacts().All(fact => State.Check(fact));

        public int GetHash() => ArrayComparer.GetHashCode(State.ListFacts());
    }
}
