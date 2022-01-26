namespace PlainGOAP.Tests.Data.Traveler.Actions
{
    public class EatAction : IAction<string, object>
    {
        public string GetName(State<string, object> state) => "Eat";
        public int ActionCost => 10;
        public bool CheckPreconditions(State<string, object> state)
        {
            return state.Check("myLocation", "Restaurant") && state.Get<int>("food") > 0;
        }

        public void Impact(State<string, object> state)
        {
            state.Set<int>("food", f => f - 1);
            state.Set("full", true);
        }
    }
}
