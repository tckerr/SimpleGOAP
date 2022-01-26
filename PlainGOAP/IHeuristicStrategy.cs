namespace PlainGOAP
{
    public interface IHeuristicStrategy<TKey, TVal>
    {
        int Calculate(StateNode<TKey, TVal> node, State<TKey, TVal> goalState);
    }
}
