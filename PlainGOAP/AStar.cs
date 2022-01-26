using System;
using System.Collections.Generic;
using System.Linq;
using Priority_Queue;

namespace PlainGOAP
{
    public class AStarSearch<TKey, TVal>
    {
        private readonly IHeuristicStrategy<TKey, TVal> heuristicStrategy;

        public AStarSearch(IHeuristicStrategy<TKey, TVal> heuristicStrategy)
        {
            this.heuristicStrategy = heuristicStrategy;
        }

        public IEnumerable<StateNode<TKey, TVal>> FindPath(SearchParameters<TKey, TVal> @params,
            int maxIterations = 10000)
        {
            var start = new StateNode<TKey, TVal>(@params.StartingState, null, null);
            var openSet = new FastPriorityQueue<StateNode<TKey, TVal>>(99999);
            openSet.Enqueue(start, 0);

            var distanceScores = new DefaultDict<int, int>(int.MaxValue);

            distanceScores[start.GetHash()] = 0;

            var iterations = 0;

            while (openSet.Any() && ++iterations < maxIterations)
            {
                var current = openSet.Dequeue();
                if (current.IsComplete(@params.GoalState))
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
                    var finalScore = distScore + heuristicStrategy.Calculate(neighbor, @params.GoalState);
                    if (!openSet.Contains(neighbor))
                        openSet.Enqueue(neighbor, finalScore);
                }
            }

            throw new Exception($"No path found, iterations: {iterations}");
        }

        private IEnumerable<StateNode<TKey, TVal>> ReconstructPath(StateNode<TKey, TVal> current)
        {
            var path = new List<StateNode<TKey, TVal>> { current };
            while (current.Parent != null)
            {
                current = current.Parent;
                path = path.Prepend(current).ToList();
            }
            return path;
        }

        private IEnumerable<StateNode<TKey, TVal>> GetNeighbors(StateNode<TKey, TVal> start,
            IEnumerable<IAction<TKey, TVal>> actions)
        {
            var result = new List<StateNode<TKey, TVal>>();
            var currentState = start.State;

            foreach (var action in actions.Where(a => a.CheckPreconditions(currentState)))
            {
                var newState = currentState.CopyAddAction(action);

                if (newState.ListFacts().All(f => currentState.Check(f)))
                    continue;

                var node = new StateNode<TKey, TVal>(newState, start, action);
                result.Add(node);
            }

            return result;
        }

        private string PrintPath(StateNode<TKey, TVal> node,
            IReadOnlyDictionary<StateNode<TKey, TVal>, StateNode<TKey, TVal>> cameFrom)
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
