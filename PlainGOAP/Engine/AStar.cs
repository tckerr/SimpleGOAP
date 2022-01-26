using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlainGOAP.Util;

namespace PlainGOAP.Engine
{

    public class StateNode<TKey, TVal>
    {
        public IAction<TKey, TVal> CameFrom;
        public State<TKey, TVal> State;

        public StateNode(State<TKey, TVal> state, IAction<TKey, TVal> cameFrom)
        {
            CameFrom = cameFrom;
            State = state;
        }

        public int Cost => CameFrom?.ActionCost ?? 0;

        public bool IsComplete(State<TKey, TVal> worldState)
        {
            foreach (var fact in worldState.ListFacts())
            {
                if (!State.Check(fact))
                    return false;
            }
            return true;
        }

        public string GetHash()
        {
            var str = new StringBuilder();
            foreach (var fact in State.ListFacts())
            {
                str.Append(fact.Key);
                str.Append("|");
                str.Append(fact.Value);
                str.Append(",");
            }
            return str.ToString();
        }
    }

    public static class AStarSearch<TKey, TVal>
    {
        private static IEnumerable<StateNode<TKey, TVal>> ReconstructPath(
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

        private static int HeuristicCost(StateNode<TKey, TVal> node, State<TKey, TVal> goalState)
        {
            return goalState.ListFacts().Count(f => !node.State.Check(f));
        }

        private static IEnumerable<StateNode<TKey, TVal>> GetNeighbors(StateNode<TKey, TVal> start,
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

        private static string PrintPath (StateNode<TKey, TVal> node, Dictionary<StateNode<TKey, TVal>, StateNode<TKey, TVal>> cameFrom)
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

        public static IEnumerable<StateNode<TKey, TVal>> FindPath(State<TKey, TVal> startingState,
            State<TKey, TVal> goalState, IAction<TKey, TVal>[] actions)
        {
            var start = new StateNode<TKey, TVal>(startingState, null);
            var openSet = new HashSet<StateNode<TKey, TVal>> {start};

            var cameFrom = new Dictionary<StateNode<TKey, TVal>, StateNode<TKey, TVal>>();
            var finalScores = new DefaultDict<string, int>(int.MaxValue);
            var distanceScores = new DefaultDict<string, int>(int.MaxValue);

            distanceScores[start.GetHash()] = 0;
            finalScores[start.GetHash()] = HeuristicCost(start, goalState);

            var iterations = 0;

            while (openSet.Any() && ++iterations < 10000)
            {
                var current = openSet.OrderBy(n => finalScores[n.GetHash()]).First();
                if (current.IsComplete(goalState))
                {
                    // Console.WriteLine($"Path found after {iterations} iterations");
                    return ReconstructPath(cameFrom, current);
                }

                openSet.Remove(current);
                var neighbors = GetNeighbors(current, actions).ToArray();
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
                    finalScores[neighbor.GetHash()] = distScore + HeuristicCost(neighbor, goalState);
                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }

            throw new Exception($"No path found, iterations: {iterations}");
        }
    }
}
