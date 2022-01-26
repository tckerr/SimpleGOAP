using PlainGOAP.StateManagement;

namespace PlainGOAP.Tests.Data.Traveler.Actions
{
    public class EatAction : IAction<KeyValueState<string, object>>
    {
        public string GetName(KeyValueState<string, object> state) => "Eat";
        public int ActionCost => 10;
        public bool CheckPreconditions(KeyValueState<string, object> state)
        {
            return state.Check("myLocation", "Restaurant") && state.Get<int>("food") > 0;
        }

        public void TakeActionOnState(KeyValueState<string, object> state)
        {
            state.Set("food", state.Get<int>("food") - 1);
            state.Set("full", true);
        }
    }
}
