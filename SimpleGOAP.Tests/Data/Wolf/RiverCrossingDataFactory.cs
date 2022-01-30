using System.Collections.Generic;
using System.Linq;
using SimpleGOAP.KeyValueState;

namespace SimpleGOAP.Tests.Data.Wolf
{
    public class RiverCrossingState
    {
        public bool Wolf;
        public bool Goat;
        public bool Cabbage;
        public bool Farmer;
    }

    public class RiverCrossingStateComparer : IEqualityComparer<RiverCrossingState>
    {
        public bool Equals(RiverCrossingState x, RiverCrossingState y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.Wolf == y.Wolf && x.Goat == y.Goat && x.Cabbage == y.Cabbage && x.Farmer == y.Farmer;
        }

        public int GetHashCode(RiverCrossingState obj)
        {
            unchecked
            {
                var hashCode = obj.Wolf.GetHashCode();
                hashCode = (hashCode * 397) ^ obj.Goat.GetHashCode();
                hashCode = (hashCode * 397) ^ obj.Cabbage.GetHashCode();
                hashCode = (hashCode * 397) ^ obj.Farmer.GetHashCode();
                return hashCode;
            }
        }
    }

    public static class RiverCrossingDataFactory
    {
        public static PlanParameters<RiverCrossingState> Create()
        {
            var currentState = new RiverCrossingState();

            return new PlanParameters<RiverCrossingState>
            {
                StartingState = currentState,
                HeuristicCost = state =>
                {
                    var cost = 0;
                    if (!state.Wolf) cost++;
                    if (!state.Goat) cost++;
                    if (!state.Cabbage) cost++;
                    if (!state.Farmer) cost++;
                    return cost;
                },
                GoalEvaluator = state => state.Wolf
                                         && state.Goat
                                         && state.Cabbage
                                         && state.Farmer,
                Actions = new[]
                {
                    new LambdaAction<RiverCrossingState>(
                        "Move cabbage left",
                        0,
                        state => !state.Farmer && !state.Cabbage && state.Wolf != state.Goat,
                        state =>
                        {
                            state.Farmer = true;
                            state.Cabbage = true;
                        }
                    ),
                    new LambdaAction<RiverCrossingState>(
                        "Move cabbage right",
                        0,
                        state => state.Farmer && state.Cabbage  && state.Wolf != state.Goat,
                        state =>
                        {
                            state.Farmer = false;
                            state.Cabbage = false;
                        }
                    ),
                    new LambdaAction<RiverCrossingState>(
                        "Move wolf left",
                        0,
                        state => !state.Farmer && !state.Wolf  && state.Cabbage != state.Goat,
                        state =>
                        {
                            state.Farmer = true;
                            state.Wolf = true;
                        }
                    ),
                    new LambdaAction<RiverCrossingState>(
                        "Move wolf right",
                        0,
                        state => state.Farmer  && state.Wolf && state.Cabbage != state.Goat,
                        state =>
                        {
                            state.Farmer = false;
                            state.Wolf = false;
                        }
                    ),
                    new LambdaAction<RiverCrossingState>(
                        "Move goat left",
                        0,
                        state => !state.Farmer && !state.Goat,
                        state =>
                        {
                            state.Farmer = true;
                            state.Goat = true;
                        }
                    ),
                    new LambdaAction<RiverCrossingState>(
                        "Move goat right",
                        0,
                        state => state.Farmer && state.Goat ,
                        state =>
                        {
                            state.Farmer = false;
                            state.Goat = false;
                        }
                    ),
                    new LambdaAction<RiverCrossingState>(
                        "Move farmer left",
                        0,
                        state => !state.Farmer,
                        state =>
                        {
                            state.Farmer = true;
                        }
                    ),
                    new LambdaAction<RiverCrossingState>(
                        "Return",
                        0,
                        state => state.Farmer,
                        state =>
                        {
                            state.Farmer = false;
                        }
                    ),
                }
            };
        }
    }
}
