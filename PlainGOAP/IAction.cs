namespace PlainGOAP
{
    public interface IAction<in T> where T : IState
    {
        string GetName(T state);
        int ActionCost { get; }
        bool CheckPreconditions(T state);
        void TakeActionOnState(T state);
    }
}
