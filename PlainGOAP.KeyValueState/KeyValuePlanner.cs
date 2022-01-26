namespace PlainGOAP.KeyValueState
{
    public class KeyValuePlanner : Planner<KeyValueState<string, object>>
    {
        public KeyValuePlanner() : base(new KeyValueStateCopier<string, object>(), new KeyValueStateComparer<string, object>())
        {
        }
    }
}
