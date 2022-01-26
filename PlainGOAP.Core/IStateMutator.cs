namespace PlainGOAP
{
    public interface IStateMutator<T>
    {
        public T CopyAndMutate(T state, IAction<T> action);
    }
}
