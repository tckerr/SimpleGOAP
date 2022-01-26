namespace PlainGOAP
{
    public interface IState<TKey, TVal>
    {
        int GetUniqueHash();
        IState<TKey, TVal> CopyAddAction(IAction<TKey, TVal> action);
    }
}
