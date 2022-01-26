namespace PlainGOAP
{
    public interface IAction<T>
    {
        string GetName(T state);
        int ActionCost { get; }
        bool CheckPreconditions(T state);
        T TakeActionOnState(T state);
    }
}
