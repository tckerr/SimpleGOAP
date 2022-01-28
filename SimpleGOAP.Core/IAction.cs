namespace SimpleGOAP
{
    /// <summary>An action, defined relative to state (T)</summary>
    /// <typeparam name="T">The type representing state.</typeparam>
    public interface IAction<T>
    {
        /// <summary>The name of the action. This has no impact in the search.</summary>
        string Title { get; }

        /// <summary>The cost of taking this action. The search will prioritize paths which have a lower cost.</summary>
        int Cost { get; }

        /// <summary>Should return true if the action can legally be taken for the input state.</summary>
        bool IsLegalForState(T state);

        /// <summary>A function which should modify the input state and return a new state. You do not need to copy
        /// the state within this function, since that has already been done upstream. So, for reference types
        /// you may just edit them and return the same object. This requires a return type of <c>T</c> in case
        /// your state object is not passed by reference (e.g. a struct).</summary>
        T TakeActionOnState(T state);
    }
}
