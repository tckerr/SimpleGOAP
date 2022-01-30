using System;

namespace SimpleGOAP
{
    public class LambdaCopier<T> : IStateCopier<T>
    {
        private readonly Func<T, T> doCopy;

        public LambdaCopier(Func<T, T> doCopy)
        {
            this.doCopy = doCopy;
        }

        public T Copy(T state) => doCopy(state);
    }
}
