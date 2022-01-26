using System;
using System.Collections.Generic;

namespace PlainGOAP
{
    public class PlanParameters<T>
    {
        public T StartingState { get; set; }
        public Func<T, int> HeuristicCost { get; set; }
        public Func<T, bool> GoalEvaluator { get; set; }
        public IReadOnlyList<IAction<T>> Actions { get; set; }

        public int MaxIterations { get; set; } = int.MaxValue;
        public bool UseFastQueue { get; set; } = true;
        public int QueueMaxSize { get; set; } = 100000;
    }
}
