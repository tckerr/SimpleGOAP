using System.Text;

namespace PlainGOAP.Engine
{
    public class StateNode<TKey, TVal>
    {
        public IAction<TKey, TVal> CameFrom;
        public State<TKey, TVal> State;

        public StateNode(State<TKey, TVal> state, IAction<TKey, TVal> cameFrom)
        {
            CameFrom = cameFrom;
            State = state;
        }

        public int Cost => CameFrom?.ActionCost ?? 0;

        public bool IsComplete(State<TKey, TVal> worldState)
        {
            foreach (var fact in worldState.ListFacts())
            {
                if (!State.Check(fact))
                    return false;
            }
            return true;
        }

        public string GetHash()
        {
            var str = new StringBuilder();
            foreach (var fact in State.ListFacts())
            {
                str.Append(fact.Key);
                str.Append("|");
                str.Append(fact.Value);
                str.Append(",");
            }
            return str.ToString();
        }
    }
}