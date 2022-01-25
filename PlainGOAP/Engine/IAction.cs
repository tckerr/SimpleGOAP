namespace PlainGOAP.Engine
{
    public interface IAction<TKey, TVal>
    {
        string Name { get; }
        int Cost { get; }
        bool CheckPreconditions(State<TKey,TVal> state);
        void Impact(State<TKey,TVal> state);
    }
}
