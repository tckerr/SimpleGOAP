using System.Collections.Generic;

namespace SimpleGOAP.Tests.Data.RiverCrossing
{
    public static class RiverCrossingPlannerFactory
    {
        public static (PlanParameters<RiverCrossingState>, Planner<RiverCrossingState>) Create()
        {
            int HeuristicCost(RiverCrossingState state1)
            {
                var cost = 0;
                if (!state1.Wolf) cost++;
                if (!state1.Goat) cost++;
                if (!state1.Cabbage) cost++;
                if (!state1.Farmer) cost++;

                if (state1.Wolf == state1.Goat && state1.Wolf != state1.Farmer) cost += 999;

                if (state1.Goat == state1.Cabbage && state1.Goat != state1.Farmer) cost += 999;

                return cost;
            }

            var returnFarmer = new LambdaAction<RiverCrossingState>(
                "Return",
                state => state.Farmer = false
            );

            var moveCabbageLeft = new LambdaAction<RiverCrossingState>(
                "Move cabbage left",
                state =>
                {
                    state.Farmer = true;
                    state.Cabbage = true;
                }
            );

            var moveCabbageRight = new LambdaAction<RiverCrossingState>(
                "Move cabbage right",
                state =>
                {
                    state.Farmer = false;
                    state.Cabbage = false;
                }
            );

            var moveWolfLeft = new LambdaAction<RiverCrossingState>(
                "Move wolf left",
                state =>
                {
                    state.Farmer = true;
                    state.Wolf = true;
                }
            );

            var moveWolfRight = new LambdaAction<RiverCrossingState>(
                "Move wolf right",
                state =>
                {
                    state.Farmer = false;
                    state.Wolf = false;
                }
            );

            var moveGoatLeft = new LambdaAction<RiverCrossingState>(
                "Move goat left",
                state =>
                {
                    state.Farmer = true;
                    state.Goat = true;
                }
            );

            var moveGoatRight = new LambdaAction<RiverCrossingState>(
                "Move goat right",
                state =>
                {
                    state.Farmer = false;
                    state.Goat = false;
                }
            );

            var moveFarmerLeft = new LambdaAction<RiverCrossingState>(
                "Move farmer left",
                state => state.Farmer = true
            );

            var data = new PlanParameters<RiverCrossingState>
            {
                StartingState = new RiverCrossingState(),
                MaxHeuristicCost = 50,
                HeuristicCost = HeuristicCost,
                GoalEvaluator = state => HeuristicCost(state) == 0,
                GetActions = s =>
                {
                    var actions = new List<IAction<RiverCrossingState>>();

                    if(s.Farmer)
                        actions.Add(returnFarmer);

                    if(!s.Farmer)
                        actions.Add(moveFarmerLeft);

                    if(!s.Farmer && !s.Cabbage && s.Wolf != s.Goat)
                        actions.Add(moveCabbageLeft);

                    if(s.Farmer && s.Cabbage)
                        actions.Add(moveCabbageRight);

                    if(!s.Farmer && !s.Wolf)
                        actions.Add(moveWolfLeft);

                    if(s.Farmer && s.Wolf)
                        actions.Add(moveWolfRight);

                    if(!s.Farmer && !s.Goat)
                        actions.Add(moveGoatLeft);

                    if(s.Farmer && s.Goat)
                        actions.Add(moveGoatRight);

                    return actions;
                }
            };

            var planner = new Planner<RiverCrossingState>(
                new LambdaCopier<RiverCrossingState>(state => new RiverCrossingState
                {
                    Cabbage = state.Cabbage,
                    Goat = state.Goat,
                    Wolf = state.Wolf,
                    Farmer = state.Farmer
                }),
                new RiverCrossingStateComparer()
            );

            return (data, planner);
        }
    }
}
