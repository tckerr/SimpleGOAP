using System.Collections.Generic;

namespace SimpleGOAP
{
    /// <summary>The result of the GOAP planner's search.</summary>
    /// <typeparam name="T">The type representing state.</typeparam>
    public class Plan<T>
    {
        /// <summary>Whether the search found a path to the goal.</summary>
        public bool Success { get; set; }

        /// <summary>The steps to take to get from current state to goal state. Will be empty if the search
        /// failed.</summary>
        public List<PlanStep<T>> Steps { get; set; }
    }
}
