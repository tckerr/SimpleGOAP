using System.Collections.Generic;

namespace PlainGOAP
{
    public class Plan<T>
    {
        public List<PlanStep<T>> Steps { get; set; }
    }
}