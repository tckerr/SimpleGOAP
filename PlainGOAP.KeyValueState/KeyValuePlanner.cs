namespace PlainGOAP.KeyValueState
{
    public class KeyValuePlanner : Planner<KeyValueState<string, object>>
    {
        public KeyValuePlanner() : base(new KeyValueStateMutator<string, object>(), new KeyValueStateComparer<string, object>())
        {
        }
    }
}
