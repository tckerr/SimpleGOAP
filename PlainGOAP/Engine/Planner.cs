using System.Collections.Generic;
using System.Linq;

namespace PlainGOAP.Engine
{
    public class Planner<TKey, TVal>
    {
        private readonly IAction<TKey, TVal>[] actions;
        private readonly State<TKey, TVal> worldState;

        public Planner(IAction<TKey, TVal>[] actions, State<TKey, TVal> worldState)
        {
            this.actions = actions;
            this.worldState = worldState;
        }

        public IEnumerable<IAction<TKey, TVal>> FindPlan(State<TKey, TVal> goalState)
        {
            var path = AStarSearch<TKey, TVal>.FindPath(worldState, goalState, actions);
            return path.Select(p => p.CameFrom).Skip(1);
        }
    }
}
