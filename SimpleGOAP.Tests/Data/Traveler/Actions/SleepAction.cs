using SimpleGOAP.KeyValueState;

namespace SimpleGOAP.Tests.Data.Traveler.Actions
{
    public class SleepAction : IAction<KeyValueState<string, object>>
    {
        public string Title => "Sleep";
        public int GetCost(KeyValueState<string, object> state) => 10;

        public bool IsLegalForState(KeyValueState<string, object> state)
        {
            return state.Check("myLocation", "Home") && state.Get<int>("fatigue") > 0;
        }

        public KeyValueState<string, object> TakeActionOnState(KeyValueState<string, object> state)
        {
            state.Set("fatigue", 0);
            return state;
        }
    }
}
