using System;

namespace SimpleGOAP
{
    public class LambdaAction<T> : IAction<T>
    {
        private Func<T, bool> preconditionCheck;
        private Func<T, T> action;

        public LambdaAction(string title, int actionCost, Func<T, bool> preconditionCheck, Func<T, T> action)
        {
            this.Title = title;
            ActionCost = actionCost;
            this.preconditionCheck = preconditionCheck;
            this.action = action;
        }

        public LambdaAction(string title, int actionCost,  Func<T, T> action)
        {
            this.Title = title;
            ActionCost = actionCost;
            preconditionCheck = _ => true;
            this.action = action;
        }

        public LambdaAction(string title, int actionCost, Action<T> action)
        {
            this.Title = title;
            ActionCost = actionCost;
            preconditionCheck = _ => true;
            this.action = state =>
            {
                action(state);
                return state;
            };
        }

        public LambdaAction(string title, int actionCost, Func<T, bool> preconditionCheck, Action<T> action)
        {
            this.Title = title;
            ActionCost = actionCost;
            this.preconditionCheck = preconditionCheck;
            this.action = state =>
            {
                action(state);
                return state;
            };
        }


        public string Title { get; }
        public int ActionCost { get; }
        public bool CheckPreconditions(T state) => preconditionCheck(state);
        public T TakeActionOnState(T state) => action(state);
    }
}
