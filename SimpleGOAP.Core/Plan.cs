using System.Collections.Generic;

namespace SimpleGOAP
{
    public class Plan<T>
    {
        public List<PlanStep<T>> Steps { get; set; }
    }
}