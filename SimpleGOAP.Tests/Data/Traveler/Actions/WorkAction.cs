using SimpleGOAP.KeyValueState;

namespace SimpleGOAP.Tests.Data.Traveler.Actions
{
    public class WorkAction : IAction<KeyValueState<string, object>>
    {
        private readonly string workLocation;
        private readonly int amountEarned;

        public WorkAction(string workLocation, int amountEarned)
        {
            this.workLocation = workLocation;
            this.amountEarned = amountEarned;
        }

        public string Title => $"Earn ${amountEarned} at {workLocation}";
        public int Cost => 10;

        public bool IsLegalForState(KeyValueState<string, object> state)
        {
            return state.Check("myLocation", workLocation) && state.Get<int>("fatigue") < 3;
        }

        public KeyValueState<string, object> TakeActionOnState(KeyValueState<string, object> state)
        {
            state.Set("money", state.Get<int>("money") + amountEarned);
            state.Set<int>("fatigue", f => f + 1);
            return state;
        }
    }
}
