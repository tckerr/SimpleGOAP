using System.Linq;

namespace PlainGOAP
{
    public class FactCountHeuristic<TKey, TVal> : IHeuristicStrategy<TKey, TVal>
    {
        public int Calculate(StateNode<TKey, TVal> node, State<TKey, TVal> goalState)
        {
            return goalState.ListFacts().Count(fact => !node.State.Check(fact));
        }
    }
}
