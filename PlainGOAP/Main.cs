using System;
using System.Linq;
using PlainGOAP.Engine;
using PlainGOAP.Implementation;

namespace PlainGOAP
{
    public static class M
    {
        public static void Main()
        {
            var currentState = new State<string, object>();
            currentState.Set("myLocation", "home");
            currentState.Set("hasFood", false);
            currentState.Set("full", false);
            currentState.Set("money", 0);

            var goalState = new State<string, object>();
            goalState.Set("full", true);
            goalState.Set("money", 100);
            goalState.Set("myLocation", "home");

            var actions = new IAction<string, object>[]
            {
                new LambdaAction<string, object>("GoToRestaurant", _ => true, state => state.Set("myLocation", "restaurant")),
                new LambdaAction<string, object>("GoToWork", _ => true, state => state.Set("myLocation", "work")),
                new LambdaAction<string, object>("GoHome", _ => true, state => state.Set("myLocation", "home")),
                new LambdaAction<string, object>(
                    "DoWork",
                    state => state.Check("myLocation", "work"),
                    state => state.Set("money", state.Get<int>("money") + 10)
                ),
                new LambdaAction<string, object>(
                    "OrderFood",
                    state => state.Check("myLocation", "restaurant") && state.Get<int>("money") >= 20,
                    state =>
                    {
                        state.Set("money", state.Get<int>("money") - 20);
                        state.Set("hasFood", true);
                    }),
                new LambdaAction<string, object>(
                    "Eat",
                    state => state.Check("myLocation", "restaurant") && state.Check("hasFood", true),
                    state =>
                    {
                        state.Set("hasFood", false);
                        state.Set("full", true);
                    }),
            };

            var planner = new Planner<string, object>(actions, currentState);
            var plan = planner.FindPlan(goalState).ToArray();

            Console.WriteLine("Plan complete");
            foreach (var action in plan)
                Console.WriteLine(action?.Name ?? "null");
        }
    }
}


