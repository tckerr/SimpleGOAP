namespace PlainGOAP.Tests.Data.Traveler.Actions
{
    public class SleepAction : IAction<string, object>
    {
        public string GetName(State<string, object> state) => "Sleep";
        public int ActionCost => 10;

        public bool CheckPreconditions(State<string, object> state)
        {
            return state.Check("myLocation", "Home") && state.Get<int>("fatigue") > 0;
        }

        public void TakeActionOnState(State<string, object> state)
        {
            state.Set("fatigue", 0);
        }
    }
}
