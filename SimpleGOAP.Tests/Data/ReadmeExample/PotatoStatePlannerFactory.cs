using System;
using System.Collections.Generic;

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
            Func<PotatoState, int> heuristicCost = state => 5 - state.BakedPotatoes;

            var makeFire = new LambdaAction<PotatoState>("Make fire", 1,
                state =>
                {
                    state.Fire = true;
                    state.Wood -= 3;
                });

            var cookPotato = new LambdaAction<PotatoState>("Cook", 1,
                state =>
                {
                    state.RawPotatoes--;
                    state.BakedPotatoes++;
                });

            var harvestPotato = new LambdaAction<PotatoState>("Harvest potato", 1,
                state => state.RawPotatoes++);

            var chopWood = new LambdaAction<PotatoState>("Chop wood", 1,
                state => state.Wood++);

            IEnumerable<IAction<PotatoState>> GetActions(PotatoState state)
            {
                yield return harvestPotato;
                yield return chopWood;

                if (state.Wood >= 3)
                    yield return makeFire;

                if (state.Fire && state.RawPotatoes > 0)
                    yield return cookPotato;
            }

            var planParameters = new PlanParameters<PotatoState>
            {
                StartingState = new PotatoState(),
                GetActions = GetActions,
                HeuristicCost = heuristicCost,
                GoalEvaluator = goalEvaluator
            };
            return (planParameters, planner);
        }
    }
}
