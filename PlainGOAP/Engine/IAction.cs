namespace PlainGOAP.Engine
{
    public interface IAction<TKey, TVal>
    {
        string GetName(State<TKey,TVal> state);
        int ActionCost { get; }
        bool CheckPreconditions(State<TKey,TVal> state);
        void Impact(State<TKey,TVal> state);
    }
}
