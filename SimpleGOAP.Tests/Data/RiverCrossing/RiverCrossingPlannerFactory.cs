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

            var data = new PlanParameters<RiverCrossingState>
            {
                StartingState = new RiverCrossingState(),
                MaxHeuristicCost = 50,
                HeuristicCost = HeuristicCost,
                GoalEvaluator = state => HeuristicCost(state) == 0,
                Actions = new[]
                {
                    new LambdaAction<RiverCrossingState>(
                        "Move cabbage left",
                        state => !state.Farmer && !state.Cabbage && state.Wolf != state.Goat,
                        state =>
                        {
                            state.Farmer = true;
                            state.Cabbage = true;
                        }
                    ),
                    new LambdaAction<RiverCrossingState>(
                        "Move cabbage right",
                        state => state.Farmer && state.Cabbage,
                        state =>
                        {
                            state.Farmer = false;
                            state.Cabbage = false;
                        }
                    ),
                    new LambdaAction<RiverCrossingState>(
                        "Move wolf left",
                        state => !state.Farmer && !state.Wolf,
                        state =>
                        {
                            state.Farmer = true;
                            state.Wolf = true;
                        }
                    ),
                    new LambdaAction<RiverCrossingState>(
                        "Move wolf right",
                        state => state.Farmer && state.Wolf,
                        state =>
                        {
                            state.Farmer = false;
                            state.Wolf = false;
                        }
                    ),
                    new LambdaAction<RiverCrossingState>(
                        "Move goat left",
                        state => !state.Farmer && !state.Goat,
                        state =>
                        {
                            state.Farmer = true;
                            state.Goat = true;
                        }
                    ),
                    new LambdaAction<RiverCrossingState>(
                        "Move goat right",
                        state => state.Farmer && state.Goat,
                        state =>
                        {
                            state.Farmer = false;
                            state.Goat = false;
                        }
                    ),
                    new LambdaAction<RiverCrossingState>(
                        "Move farmer left",
                        state => !state.Farmer,
                        state => state.Farmer = true
                    ),
                    new LambdaAction<RiverCrossingState>(
                        "Return",
                        state => state.Farmer,
                        state => state.Farmer = false
                    ),
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
