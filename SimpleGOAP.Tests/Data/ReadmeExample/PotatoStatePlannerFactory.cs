using System;

namespace SimpleGOAP.Tests.Data.ReadmeExample
{
    public static class PotatoStatePlannerFactory
    {
        public static (PlanParameters<PotatoState>, Planner<PotatoState>) Create()
        {
            var planner = new Planner<PotatoState>(
                new PotatoStateCopier(),
                new PotatoStateEqualityComparer()
            );

            Func<PotatoState, bool> goalEvaluator = state => state.BakedPotatoes >= 5;
            Func<PotatoState,int> heuristicCost = state => 5 - state.BakedPotatoes;
            var planParameters = new PlanParameters<PotatoState>
            {
                StartingState = new PotatoState(),
                Actions = new[]
                {
                    new LambdaAction<PotatoState>("Harvest potato", 1,
                        state => state.RawPotatoes++),

                    new LambdaAction<PotatoState>("Chop wood", 1,
                        state => state.Wood++),

                    new LambdaAction<PotatoState>("Make fire", 1,
                        state => state.Wood >= 3,
                        state =>
                        {
                            state.Fire = true;
                            state.Wood -= 3;
                        }),

                    new LambdaAction<PotatoState>("Cook", 1,
                        state => state.Fire && state.RawPotatoes > 0,
                        state =>
                        {
                            state.RawPotatoes--;
                            state.BakedPotatoes++;
                        }),
                },
                HeuristicCost = heuristicCost,
                GoalEvaluator = goalEvaluator
            };
            return (planParameters, planner);
        }
    }
}
