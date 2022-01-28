namespace SimpleGOAP
{
    public interface IAction<T>
    {
        string Title { get; }
        int ActionCost { get; }
        bool CheckPreconditions(T state);
        T TakeActionOnState(T state);
    }
}
