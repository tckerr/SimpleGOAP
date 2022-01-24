namespace PlainGOAP.Engine
{
    public class Node<TKey, TVal>
    {
        public State<TKey, TVal> CurrentState;
        public State<TKey, TVal> GoalState;
        public IAction<TKey, TVal>? Action;
    }


    public class Planner<TKey, TVal>
    {
        private readonly List<IAction<TKey, TVal>> actions;
        private readonly State<TKey, TVal> worldState;

        public Planner(List<IAction<TKey, TVal>> actions, State<TKey, TVal> worldState)
        {
            this.actions = actions;
            this.worldState = worldState;
        }

        public List<IAction<TKey, TVal>> FindPlan(State<TKey, TVal> goalState)
        {
            throw new NotImplementedException();
        }
    }
}
