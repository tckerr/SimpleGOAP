using System;

namespace SimpleGOAP
{
    public class LambdaAction<T> : IAction<T>
    {
        private Func<T, T> action;
        private readonly Func<int> getCost;

        public LambdaAction(string title, int actionCost, Func<T, T> action)
        {
            Title = title;
            getCost = () => actionCost;
            this.action = action;
        }

        public LambdaAction(string title, int actionCost, Action<T> action)
        {
            Title = title;
            getCost = () => actionCost;
            this.action = state =>
            {
                action(state);
                return state;
            };
        }

        public LambdaAction(string title, Func<int> getCost, Func<T, T> action)
        {
            Title = title;
            this.getCost = getCost;
            this.action = action;
        }

        public LambdaAction(string title, Func<int> getCost, Action<T> action)
        {
            Title = title;
            this.getCost = getCost;
            this.action = state =>
            {
                action(state);
                return state;
            };
        }

        public LambdaAction(string title, Func<T, T> action)
        {
            Title = title;
            getCost = () => 1;
            this.action = action;
        }

        public LambdaAction(string title, Action<T> action)
        {
            Title = title;
            getCost = () => 1;
            this.action = state =>
            {
                action(state);
                return state;
            };
        }

        public string Title { get; }

        public int GetCost(T state) => getCost();

        public T TakeActionOnState(T state) => action(state);
    }
}
