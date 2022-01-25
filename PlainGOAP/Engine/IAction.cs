namespace PlainGOAP.Engine
{
    public interface IAction<TKey, TVal>
    {
        string Name { get; }
        int Cost { get; }
        Fact<TKey, TVal>[] Prerequisites { get; }
        Fact<TKey, TVal>[] Effects { get; }
        bool CheckPreconditions(State<TKey,TVal> state);

        void Impact(State<TKey,TVal> state);
    }
}
