using System;
using System.Collections.Generic;
using System.Linq;
using PlainGOAP.Util;

namespace PlainGOAP.Engine
{
    public static class AStarSearch
    {
        private static IEnumerable<StateNode<TKey, TVal>> ReconstructPath<TKey, TVal>(
            Dictionary<StateNode<TKey, TVal>, StateNode<TKey, TVal>> cameFrom, StateNode<TKey, TVal> current)
        {
            var path = new List<StateNode<TKey, TVal>> {current};
            while (cameFrom.TryGetValue(current, out var next))
            {
                current = next;
                path = path.Prepend(current).ToList();
            }

            return path;
        }

        private static int HeuristicCost<TKey, TVal>(StateNode<TKey, TVal> node, State<TKey, TVal> goalState)
        {
            return goalState.ListFacts().Count(f => !node.State.Check(f));
        }

        private static IEnumerable<StateNode<TKey, TVal>> GetNeighbors<TKey, TVal>(StateNode<TKey, TVal> start,
            IEnumerable<IAction<TKey, TVal>> actions)
        {
            var result = new List<StateNode<TKey, TVal>>();
            var currentState = start.State;

            foreach (var action in actions.Where(a => a.CheckPreconditions(currentState)))
            {
                var newState = currentState.CopyAddAction(action);

                if (newState.ListFacts().All(f => currentState.Check(f)))
                    continue;

                var node = new StateNode<TKey, TVal>(newState, action);
                result.Add(node);
            }

            return result;
        }

        private static string PrintPath<TKey, TVal>(StateNode<TKey, TVal> node, IReadOnlyDictionary<StateNode<TKey, TVal>, StateNode<TKey, TVal>> cameFrom)
        {
            var str = "";
            while (true)
            {
                var cf = node.CameFrom;
                var name = "Default";
                if (cf != null)
                    name = cf.GetName(node.State);
                str += "> " + name + " ";
                if (!cameFrom.ContainsKey(node))
                    return str;
                node = cameFrom[node];
            }
        }

        public static IEnumerable<StateNode<TKey, TVal>> FindPath<TKey, TVal>(SearchParameters<TKey, TVal> @params)
        {
            var start = new StateNode<TKey, TVal>(@params.StartingState, null);
            var openSet = new HashSet<StateNode<TKey, TVal>> {start};

            var cameFrom = new Dictionary<StateNode<TKey, TVal>, StateNode<TKey, TVal>>();
            var finalScores = new DefaultDict<string, int>(int.MaxValue);
            var distanceScores = new DefaultDict<string, int>(int.MaxValue);

            distanceScores[start.GetHash()] = 0;
            finalScores[start.GetHash()] = HeuristicCost(start, @params.GoalState);

            var iterations = 0;

            while (openSet.Any() && ++iterations < 10000)
            {
                var current = openSet.OrderBy(n => finalScores[n.GetHash()]).First();
                if (current.IsComplete(@params.GoalState))
                {
                    // Console.WriteLine($"Path found after {iterations} iterations");
                    return ReconstructPath(cameFrom, current);
                }

                openSet.Remove(current);
                var neighbors = GetNeighbors(current, @params.Actions).ToArray();
                foreach (var neighbor in neighbors)
                {
                    // Console.WriteLine($"[{iterations}] Testing neighbor {PrintPath(neighbor, cameFrom)}");

                    var distScore = distanceScores[current.GetHash()] + neighbor.Cost;
                    if (distScore >= distanceScores[neighbor.GetHash()])
                    {
                        // Console.WriteLine($"[{iterations}] {PrintPath(neighbor, cameFrom)} was more expensive than an " +
                                          // $"existing path. Ignoring.");
                        continue;
                    }

                    cameFrom[neighbor] = current;
                    // Console.WriteLine($"[{iterations}] Validated path {PrintPath(neighbor, cameFrom)} was cheaper... adding node and registering scores. ");
                    distanceScores[neighbor.GetHash()] = distScore;
                    finalScores[neighbor.GetHash()] = distScore + HeuristicCost(neighbor, @params.GoalState);
                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }

            throw new Exception($"No path found, iterations: {iterations}");
        }
    }
}
