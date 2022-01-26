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


        public IEnumerable<StateNode<TKey, TVal>> FindPath2(SearchParameters<TKey, TVal> @params)
        {
            var goalState = @params.GoalState;
            var openSet = new StablePriorityQueue<StateNode<TKey, TVal>>(1000000);
            openSet.Enqueue(new StateNode<TKey, TVal>(@params.StartingState, null, null), 0);
            var closedSet = new HashSet<StateNode<TKey, TVal>>();
            while (!openSet.First().IsComplete(goalState))
            {
                var current = openSet.Dequeue();
                closedSet.Add(current);
                var neighbors = GetNeighbors(current, @params.Actions).ToArray();
                for (var i = 0; i < neighbors.Length; i++)
                {
                    var neighbor = neighbors[i];
                    var cost = current.GCost + neighbor.ActionCost;
                    if (cost < neighbor.GCost && openSet.Contains(neighbor))
                        openSet.Remove(neighbor);
                    if (cost < neighbor.GCost && closedSet.Contains(neighbor))
                        closedSet.Remove(neighbor);
                    if (!openSet.Contains(neighbor) && !closedSet.Contains(neighbor))
                    {
                        neighbor.GCost = cost;
                        openSet.Enqueue(neighbor, neighbor.GCost + heuristicStrategy.Calculate(neighbor, goalState));
                    }
                }
            }
            if(!openSet.Any())
                throw new Exception($"No path found");

            return ReconstructPath(openSet.First());
        }



        public IEnumerable<StateNode<TKey, TVal>> FindPath(SearchParameters<TKey, TVal> @params,
            int maxIterations = 10000)
        {
            var start = new StateNode<TKey, TVal>(@params.StartingState, null, null);
            var openSet = new HashSet<StateNode<TKey, TVal>> { start };

            var finalScores = new DefaultDict<int, int>(int.MaxValue);
            var distanceScores = new DefaultDict<int, int>(int.MaxValue);

            distanceScores[start.GetHash()] = 0;
            finalScores[start.GetHash()] = heuristicStrategy.Calculate(start, @params.GoalState);

            var iterations = 0;

            while (openSet.Any() && ++iterations < maxIterations)
            {
                var current = openSet.OrderBy(n => finalScores[n.GetHash()]).First();
                if (current.IsComplete(@params.GoalState))
                {
                    // Console.WriteLine($"Path found after {iterations} iterations");
                    return ReconstructPath(current);
                }

                openSet.Remove(current);
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
                    finalScores[neighbor.GetHash()] = distScore + heuristicStrategy.Calculate(neighbor, @params.GoalState);
                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
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
