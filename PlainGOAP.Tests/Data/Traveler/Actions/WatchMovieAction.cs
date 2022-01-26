﻿using PlainGOAP.StateManagement;

namespace PlainGOAP.Tests.Data.Traveler.Actions
{
    public class WatchMovieAction : IAction<KeyValueState<string, object>>
    {
        public string GetName(KeyValueState<string, object> state) => "Watch movie for $20";
        public int ActionCost => 10;

        public bool CheckPreconditions(KeyValueState<string, object> state)
        {
            return state.Check("myLocation", "Theater") && state.Get<int>("money") >= 20;
        }

        public void TakeActionOnState(KeyValueState<string, object> state)
        {
            state.Set("money", state.Get<int>("money") - 20);
            state.Set("fun", state.Get<int>("fun") + 1);
        }
    }
}
