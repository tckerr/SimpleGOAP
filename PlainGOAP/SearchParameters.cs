using System;
using System.Collections.Generic;

namespace PlainGOAP
{
    public class SearchParameters<T1, T2>
    {
        public IState<T1, T2> StartingState { get; set; }
        public Func<IState<T1, T2>, int> HeuristicCost { get; set; }
        public Func<IState<T1, T2>, bool> GoalEvaluator { get; set; }
        public IReadOnlyList<IAction<T1, T2>> Actions { get; set; }
    }
}
