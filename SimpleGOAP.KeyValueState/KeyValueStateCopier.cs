namespace SimpleGOAP.KeyValueState
{
    public class KeyValueStateCopier<TKey, TVal> : IStateCopier<KeyValueState<TKey, TVal>>
    {
        public KeyValueState<TKey, TVal> Copy(KeyValueState<TKey, TVal> state)
        {
            var newState = new KeyValueState<TKey, TVal>();
            foreach (var fact in state.Facts)
                newState.Set(fact);

            return newState;
        }
    }
}
