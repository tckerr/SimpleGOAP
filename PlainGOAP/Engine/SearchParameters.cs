using System.Collections.Generic;

namespace PlainGOAP.Engine
{
    public class SearchParameters<T1, T2>
    {
        public State<T1, T2> StartingState { get; set; }
        public State<T1, T2> GoalState { get; set; }
        public IReadOnlyList<IAction<T1, T2>> Actions { get; set; }
    }
}