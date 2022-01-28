using SimpleGOAP.KeyValueState;

namespace SimpleGOAP.Tests.Data.Traveler.Actions
{
    public class EatAction : IAction<KeyValueState<string, object>>
    {
        public string Title => $"Eat food";
        public int ActionCost => 10;
        public bool CheckPreconditions(KeyValueState<string, object> state)
        {
            return state.Check("myLocation", "Restaurant") && state.Get<int>("food") > 0;
        }

        public KeyValueState<string, object> TakeActionOnState(KeyValueState<string, object> state)
        {
            state.Set("food", state.Get<int>("food") - 1);
            state.Set("full", true);
            return state;
        }
    }
}
