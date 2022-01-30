using System;
using System.Collections.Generic;
using System.Linq;
using Priority_Queue;

namespace SimpleGOAP
{
    /// <summary>The GOAP planner which runs search on possible futures to find a path to a goal state.</summary>
    /// <typeparam name="T">The type representing state.</typeparam>
    public class Planner<T>
    {
        private readonly IStateCopier<T> stateCopier;
        private readonly IEqualityComparer<T> stateComparer;

        public Planner(IStateCopier<T> stateCopier, IEqualityComparer<T> stateComparer)
        {
            this.stateCopier = stateCopier;
            this.stateComparer = stateComparer;
        }

        /// <summary>Execute the plan.</summary>
        public Plan<T> Execute(PlanParameters<T> @params)
        {
            if (@params.GoalEvaluator == null)
                throw new ArgumentOutOfRangeException(nameof(@params.GoalEvaluator));
            if (@params.HeuristicCost == null)
                throw new ArgumentOutOfRangeException(nameof(@params.HeuristicCost));
            if (@params.Actions == null)
                throw new ArgumentOutOfRangeException(nameof(@params.Actions));
            if (@params.StartingState == null)
                throw new ArgumentOutOfRangeException(nameof(@params.StartingState));

            /*
             * AStar:
             *  g score: current distance from the start measured by sum of action costs
             *  h score: heuristic of how close the node's state is to goal state, supplied by caller
             *  f score: sum of (g, h), used as priority of the node
             */
            var heuristicCost = @params.HeuristicCost;
            var evalGoal = @params.GoalEvaluator;
            var maxHScore = @params.MaxHeuristicCost;

            var start = new StateNode<T>(@params.StartingState, null, null);
            var openSet = CreateQueue(@params);
            openSet.Enqueue(start, 0);

            var distanceScores = new DefaultDict<T, int>(int.MaxValue, stateComparer)
            {
                [start.State] = 0
            };

            var iterations = 0;
            while (openSet.Any() && ++iterations < @params.MaxIterations)
            {
                var current = openSet.Dequeue();
                if (evalGoal(current.State))
                    return ReconstructPath(current, @params.StartingState);

                foreach (var neighbor in GetNeighbors(current, @params.Actions))
                {
                    var distScore = distanceScores[current.State] + neighbor.ActionCost;
                    if (distScore >= distanceScores[neighbor.State])
                        continue;

                    distanceScores[neighbor.State] = distScore;
                    var hCost = heuristicCost(neighbor.State);
                    if(hCost > maxHScore)
                        continue;
                    var finalScore = distScore + hCost;
                    if (!openSet.Contains(neighbor))
                        openSet.Enqueue(neighbor, finalScore);
                }
            }

            return new Plan<T>
            {
                Success = false,
                Steps = new List<PlanStep<T>>()
            };
        }

        private static IPriorityQueue<StateNode<T>, float> CreateQueue(PlanParameters<T> args)
        {
            if (args.UseFastQueue)
                return new FastPriorityQueue<StateNode<T>>(args.QueueMaxSize);
            return new SimplePriorityQueue<StateNode<T>>();
        }

        private static Plan<T> ReconstructPath(StateNode<T> final, T startingState)
        {
            var current = final;
            var path = new List<StateNode<T>>();
            while (current.Parent != null)
            {
                path.Add(current);
                current = current.Parent;
            }

            path.Reverse();
            return new Plan<T>
            {
                Success = true,
                Steps = path.Select((step, i) => new PlanStep<T>
                {
                    Index = i,
                    Action = step.SourceAction,
                    AfterState = step.State,
                    BeforeState = step.Parent == null ? startingState : step.Parent.State
                }).ToList()
            };
        }

        private IEnumerable<StateNode<T>> GetNeighbors(StateNode<T> start,
            IEnumerable<IAction<T>> actions)
        {
            var currentState = start.State;

            foreach (var action in actions.Where(a => a.IsLegalForState(currentState)))
            {
                var newState = action.TakeActionOnState(stateCopier.Copy(currentState));

                // sometimes actions have no effect on state, in which case we don't want to entertain them as nodes
                // assuming that additional actions to get to the same state is always worse
                if(!stateComparer.Equals(currentState, newState))
                    yield return new StateNode<T>(newState, start, action);
            }
        }
    }
}
