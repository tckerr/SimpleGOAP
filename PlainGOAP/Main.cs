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
            currentState.Set("gasInCar", 100);

            var goalState = new State<string, object>();
            goalState.Set("full", true);
            goalState.Set("hasMoney", true);
            goalState.Set("myLocation", "home");
            // goalState.Set("gasInCar", 100);

            var actions = new IAction<string, object>[]
            {
                new LambdaAction<string, object>
                {
                    Name = "GoToRestaurant",
                    Prerequisites = new Fact<string, object>[]
                    {
                        // new ("gasInCar", 20)
                    },
                    Effects = new Fact<string, object>[]
                    {
                        new("myLocation", "restaurant"),
                    }
                },
                new LambdaAction<string, object>
                {
                    Name = "GoHome",
                    Prerequisites = Array.Empty<Fact<string, object>>(),
                    Effects = new Fact<string, object>[]
                    {
                        new("myLocation", "home")
                    }
                },
                new LambdaAction<string, object>
                {
                    Name = "GoToWork",
                    Prerequisites = Array.Empty<Fact<string, object>>(),
                    Effects = new Fact<string, object>[]
                    {
                        new("myLocation", "work")
                    }
                },
                new LambdaAction<string, object>
                {
                    Name = "EarnMoney",
                    Prerequisites = new Fact<string, object>[]
                    {
                        new("myLocation", "work")
                    },
                    Effects = new Fact<string, object>[]
                    {
                        new("hasMoney", true)
                    }
                },
                new LambdaAction<string, object>
                {
                    Name = "OrderFood",
                    Prerequisites = new Fact<string, object>[]
                    {
                        new("myLocation", "restaurant"),
                        new("hasMoney", true)
                    },
                    Effects = new Fact<string, object>[]
                    {
                        new("hasFood", true),
                        new("hasMoney", false),
                    }
                },
                new LambdaAction<string, object>
                {
                    Name = "Eat",
                    Prerequisites = new Fact<string, object>[]
                    {
                        new ("hasFood", true),
                        new ("myLocation", "restaurant")
                    },
                    Effects = new Fact<string, object> []
                    {
                        new("full", true),
                        new("hasFood", false)
                    }
                }
            };

            var planner = new Planner<string, object>(actions, currentState);
            var plan = planner.FindPlan(goalState).ToArray();

            Console.WriteLine("Plan complete");
            foreach (var action in plan)
                Console.WriteLine(action?.Name ?? "null");
        }
    }
}


