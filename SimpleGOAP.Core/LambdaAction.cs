using System;

namespace SimpleGOAP
{
    public class LambdaAction<T> : IAction<T>
    {
        private Func<T, T> action;

        public LambdaAction(string title, int actionCost,  Func<T, T> action)
        {
            Title = title;
            Cost = actionCost;
            this.action = action;
        }

        public LambdaAction(string title, int actionCost, Action<T> action)
        {
            Title = title;
            Cost = actionCost;
            this.action = state =>
            {
                action(state);
                return state;
            };
        }

        public LambdaAction(string title, Func<T, T> action)
        {
            Title = title;
            Cost = 1;
            this.action = action;
        }

        public LambdaAction(string title, Action<T> action)
        {
            Title = title;
            Cost = 1;
            this.action = state =>
            {
                action(state);
                return state;
            };
        }

        public string Title { get; }
        public int Cost { get; }
        public T TakeActionOnState(T state) => action(state);
    }
}
