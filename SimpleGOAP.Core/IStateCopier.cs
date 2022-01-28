namespace SimpleGOAP
{
    /// <summary>A class which is responsible for making copies of your state <c>T</c>.</summary>
    /// <typeparam name="T">The type representing state.</typeparam>
    public interface IStateCopier<T>
    {
        /// <summary>Given a state <c>T</c>, this function makes a copy of that state and returns it.</summary>
        public T Copy(T state);
    }
}
