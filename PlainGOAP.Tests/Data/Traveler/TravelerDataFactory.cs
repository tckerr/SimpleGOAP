using System.Collections.Generic;
using System.Linq;
using PlainGOAP.Tests.Data.Traveler.Actions;

namespace PlainGOAP.Tests.Data.Traveler
{
    public static class TravelerDataFactory
    {
        public static SearchParameters<string, object> Create()
        {
            const int COST_OF_TOY = 10;
            const int SELL_VALUE_OF_TOY = 40;
            const int COST_OF_GAS = 50;
            const int COST_OF_FOOD = 30;
            const int GAS_TANK_CAPACITY = 40;
            const int WAGE = 20;

            var currentState = new State<string, object>();
            currentState.Set("myLocation", "Home");
            currentState.Set("food", 0);
            currentState.Set("full", false);
            currentState.Set("money", 0);
            currentState.Set("gas", 40);
            currentState.Set("fun", 0);
            currentState.Set("fatigue", 0);
            currentState.Set("toy", 0);

            var locations = new List<(string, int, int)>
            {
                ("Restaurant", 2, 2),
                ("Work", 1, 0),
                ("Gas Station", 1, 1),
                ("Home", 0, 0),
                ("Theater", 2, 0),
            };

            var actions = new IAction<string, object>[]
            {
                new DriveAction("Restaurant", locations),
                new DriveAction("Work", locations),
                new DriveAction("Home", locations),
                new DriveAction("Gas Station", locations),
                new DriveAction("Theater", locations),
                new PurchaseAction("gas", "Gas Station", COST_OF_GAS, GAS_TANK_CAPACITY, GAS_TANK_CAPACITY),
                new PurchaseAction("toy", "Gas Station", COST_OF_TOY, 3, 6),
                new PurchaseAction("food", "Restaurant", COST_OF_FOOD, 1),
                new SellAction("toy", SELL_VALUE_OF_TOY),
                new WorkAction("Work", WAGE),
                new WatchMovieAction(),
                new SleepAction(),
                new EatAction()
            };

            int HeuristicCost(State<string, object> state) =>
                new[]
                {
                    state.Check("full", true) ? 0 : 1,
                    state.Check("myLocation", "Home") ? 0 : 1,
                    state.Get<int>("fun") >= 2 ? 0 : 1,
                    state.Get<int>("fatigue") <= 0 ? 0 : 1,
                }.Sum();

            return new SearchParameters<string, object>
            {
                Actions = actions,
                StartingState = currentState,
                HeuristicCost = HeuristicCost,
                GoalEvaluator = s => HeuristicCost(s) <= 0,
            };
        }
    }
}
