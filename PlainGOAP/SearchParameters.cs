using System;
using System.Collections.Generic;

namespace PlainGOAP
{
    public class SearchParameters<T> where T : IState
    {
        public T StartingState { get; set; }
        public Func<T, int> HeuristicCost { get; set; }
        public Func<T, bool> GoalEvaluator { get; set; }
        public IReadOnlyList<IAction<T>> Actions { get; set; }
    }
}
