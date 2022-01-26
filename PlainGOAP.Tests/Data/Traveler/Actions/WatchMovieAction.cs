namespace PlainGOAP.Tests.Data.Traveler.Actions
{
    public class WatchMovieAction : IAction<string, object>
    {
        public string GetName(State<string, object> state) => "Watch movie for $20";
        public int ActionCost => 10;

        public bool CheckPreconditions(State<string, object> state)
        {
            return state.Check("myLocation", "Theater") && state.Get<int>("money") >= 20;
        }

        public void Impact(State<string, object> state)
        {
            state.Set("money", state.Get<int>("money") - 20);
            state.Set("fun", state.Get<int>("fun") + 1);
        }
    }
}
