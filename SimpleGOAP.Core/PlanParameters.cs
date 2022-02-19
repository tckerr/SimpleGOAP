using System;
using System.Collections.Generic;

namespace SimpleGOAP
{
    /// <summary>Search parameters for the planner.</summary>
    /// <typeparam name="T">The type representing state.</typeparam>
    public class PlanParameters<T>
    {
        /// <summary>The initial world state.</summary>
        public T StartingState { get; set; }

        /// <summary>A heuristic function that will tell the engine how close to our goal we are for any given state.
        /// The planner will consider lower values to be closer to the goal.<br/><br/>Note that this is technically
        /// optional; you could return 0 and the search will still work. However, it's purpose is to suggest possible
        /// future paths and therefore can have a drastic effect on performance.</summary>
        public Func<T, int> HeuristicCost { get; set; }

        /// <summary> A function that should return true if for a given state T we have reached our goal. </summary>
        public Func<T, bool> GoalEvaluator { get; set; }

        /// <summary> A list of actions that can be taken to achieve the goal.</summary>
        public Func<T, IEnumerable<IAction<T>>> GetActions { get; set; }

        /// <summary>The maximum number of possible actions to check before exiting.</summary>
        public int MaxIterations { get; set; } = int.MaxValue;

        /// <summary>Prefer using a faster queue, with the downside of having a fixed queue size.</summary>
        public bool UseFastQueue { get; set; } = true;

        /// <summary>The max queue size to use. Only relevant if <c>UseFastQueue</c> is set to true.</summary>
        public int QueueMaxSize { get; set; } = 100001;

        /// <summary>If an action's heuristic cost exceeds this threshold, it will be ignored as
        /// a possible future path. Ignored if null.</summary>
        public int? MaxHeuristicCost { get; set; }
    }
}
