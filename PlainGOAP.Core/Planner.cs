using System;
using System.Collections.Generic;
using System.Linq;
using Priority_Queue;

namespace PlainGOAP
{
    public class Planner<T>
    {
        private readonly IStateMutator<T> stateMutator;
        private readonly IEqualityComparer<T> stateComparer;

        public Planner(IStateMutator<T> stateMutator, IEqualityComparer<T> stateComparer)
        {
            this.stateMutator = stateMutator;
            this.stateComparer = stateComparer;
        }

        public Plan<T> Execute(PlanParameters<T> @params)
        {
            var heuristicCost = @params.HeuristicCost;
            var evalGoal = @params.GoalEvaluator;
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

                var neighbors = GetNeighbors(current, @params.Actions).ToArray();
                foreach (var neighbor in neighbors)
                {
                    var distScore = distanceScores[current.State] + neighbor.ActionCost;
                    if (distScore >= distanceScores[neighbor.State])
                        continue;

                    distanceScores[neighbor.State] = distScore;
                    var finalScore = distScore + heuristicCost(neighbor.State);
                    if (!openSet.Contains(neighbor))
                        openSet.Enqueue(neighbor, finalScore);
                }
            }

            throw new Exception($"No path found, iterations: {iterations}");
        }

        private IPriorityQueue<StateNode<T>, float> CreateQueue(PlanParameters<T> args)
        {
            if (args.UseFastQueue)
                return new FastPriorityQueue<StateNode<T>>(args.QueueMaxSize);
            return new SimplePriorityQueue<StateNode<T>>();
        }

        private Plan<T> ReconstructPath(StateNode<T> final, T startingState)
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
            var result = new List<StateNode<T>>();
            var currentState = start.State;

            foreach (var action in actions.Where(a => a.CheckPreconditions(currentState)))
            {
                var newState = stateMutator.CopyAndMutate(currentState, action);

                if(stateComparer.Equals(currentState, newState))
                    continue;

                var node = new StateNode<T>(newState, start, action);
                result.Add(node);
            }

            return result;
        }
    }
}
