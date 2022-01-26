namespace PlainGOAP
{
    public interface IStateCopier<T> where T : IState
    {
        public T CopyWithAddedAction(T state, IAction<T> action);
    }
}
