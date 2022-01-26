using System;
using System.Collections.Generic;
using System.Linq;
using Priority_Queue;

namespace PlainGOAP
{
    public class Planner<T> where T: IState
    {
        private readonly IStateCopier<T> stateCopier;
        private readonly IEqualityComparer<T> stateComparer;

        public Planner(IStateCopier<T> stateCopier, IEqualityComparer<T> stateComparer)
        {
            this.stateCopier = stateCopier;
            this.stateComparer = stateComparer;
        }

        public IEnumerable<StateNode<T>> Execute(SearchParameters<T> @params,
            int maxIterations = 10000)
        {
            var heuristicCost = @params.HeuristicCost;
            var evalGoal = @params.GoalEvaluator;
            var start = new StateNode<T>(@params.StartingState, null, null);
            var openSet = new FastPriorityQueue<StateNode<T>>(99999);
            openSet.Enqueue(start, 0);

            var distanceScores = new DefaultDict<int, int>(int.MaxValue)
            {
                [start.GetHash()] = 0
            };

            var iterations = 0;

            while (openSet.Any() && ++iterations < maxIterations)
            {
                var current = openSet.Dequeue();
                if (evalGoal(current.State))
                {
                    // Console.WriteLine($"Path found after {iterations} iterations");
                    return ReconstructPath(current);
                }

                var neighbors = GetNeighbors(current, @params.Actions).ToArray();
                foreach (var neighbor in neighbors)
                {
                    // Console.WriteLine($"[{iterations}] Testing neighbor {PrintPath(neighbor, cameFrom)}");

                    var distScore = distanceScores[current.GetHash()] + neighbor.ActionCost;
                    if (distScore >= distanceScores[neighbor.GetHash()])
                    {
                        // Console.WriteLine($"[{iterations}] {PrintPath(neighbor, cameFrom)} was more expensive than an " +
                        // $"existing path. Ignoring.");
                        continue;
                    }

                    // Console.WriteLine($"[{iterations}] Validated path {PrintPath(neighbor, cameFrom)} was cheaper... adding node and registering scores. ");
                    distanceScores[neighbor.GetHash()] = distScore;
                    var finalScore = distScore + heuristicCost(neighbor.State);
                    if (!openSet.Contains(neighbor))
                        openSet.Enqueue(neighbor, finalScore);
                }
            }

            throw new Exception($"No path found, iterations: {iterations}");
        }

        private IEnumerable<StateNode<T>> ReconstructPath(StateNode<T> current)
        {
            var path = new List<StateNode<T>> { current };
            while (current.Parent != null)
            {
                current = current.Parent;
                path = path.Prepend(current).ToList();
            }
            return path;
        }

        private IEnumerable<StateNode<T>> GetNeighbors(StateNode<T> start,
            IEnumerable<IAction<T>> actions)
        {
            var result = new List<StateNode<T>>();
            var currentState = start.State;

            foreach (var action in actions.Where(a => a.CheckPreconditions(currentState)))
            {
                var newState = stateCopier.CopyWithAddedAction(currentState, action);

                if(newState.GetUniqueHashForState() == currentState.GetUniqueHashForState())
                    continue;

                var node = new StateNode<T>(newState, start, action);
                result.Add(node);
            }

            return result;
        }

        private string PrintPath(StateNode<T> node,
            IReadOnlyDictionary<StateNode<T>, StateNode<T>> cameFrom)
        {
            var str = "";
            while (true)
            {
                var cf = node.SourceAction;
                var name = "Default";
                if (cf != null)
                    name = cf.GetName(node.State);
                str += "> " + name + " ";
                if (!cameFrom.ContainsKey(node))
                    return str;
                node = cameFrom[node];
            }
        }
    }
}
