using PlainGOAP.StateManagement;

namespace PlainGOAP.Tests.Data.Traveler.Actions
{
    public class SleepAction : IAction<KeyValueState<string, object>>
    {
        public string GetName(KeyValueState<string, object> state) => "Sleep";
        public int ActionCost => 10;

        public bool CheckPreconditions(KeyValueState<string, object> state)
        {
            return state.Check("myLocation", "Home") && state.Get<int>("fatigue") > 0;
        }

        public void TakeActionOnState(KeyValueState<string, object> state)
        {
            state.Set("fatigue", 0);
        }
    }
}
