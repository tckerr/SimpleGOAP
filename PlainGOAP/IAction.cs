namespace PlainGOAP
{
    public interface IAction<TKey, TVal>
    {
        string GetName(IState<TKey,TVal> state);
        int ActionCost { get; }
        bool CheckPreconditions(IState<TKey,TVal> state);
        void TakeActionOnState(IState<TKey,TVal> state);
    }
}
