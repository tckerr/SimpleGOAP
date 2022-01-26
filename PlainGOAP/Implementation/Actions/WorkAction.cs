using PlainGOAP.Engine;

namespace PlainGOAP.Implementation.Actions
{
    public class WorkAction : IAction<string, object>
    {
        private readonly string workLocation;
        private readonly int amountEarned;

        public WorkAction(string workLocation, int amountEarned)
        {
            this.workLocation = workLocation;
            this.amountEarned = amountEarned;
        }

        public string GetName(State<string, object> state) => $"Earn ${amountEarned} at {workLocation}";
        public int ActionCost => 10;

        public bool CheckPreconditions(State<string, object> state)
        {
            return state.Check("myLocation", workLocation) && state.Get<int>("fatigue") < 3;
        }

        public void Impact(State<string, object> state)
        {
            state.Set("money", state.Get<int>("money") + amountEarned);
            state.Set<int>("fatigue", f => f + 1);
        }
    }
}
