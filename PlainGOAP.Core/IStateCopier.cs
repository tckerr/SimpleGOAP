namespace PlainGOAP
{
    public interface IStateCopier<T>
    {
        public T Copy(T state);
    }
}
