namespace SimpleGOAP
{
    /// <summary>A step in the plan which indicates an action taken, as well as the before and after states.</summary>
    /// <typeparam name="T">The type representing state.</typeparam>
    public class PlanStep<T>
    {
        /// <summary>The position of this step in the list.</summary>
        public int Index { get; set; }

        /// <summary>The action taken.</summary>
        public IAction<T> Action { get; set; }

        /// <summary>The state prior to action being taken.</summary>
        public T BeforeState {get;set;}

        /// <summary>The state after the action is taken.</summary>
        public T AfterState {get;set;}
    }
}
