namespace PlainGOAP.KeyValueState
{
    public class KeyValueStateMutator<TKey, TVal> : IStateMutator<KeyValueState<TKey, TVal>>
    {
        public KeyValueState<TKey, TVal> CopyAndMutate(KeyValueState<TKey, TVal> state, IAction<KeyValueState<TKey, TVal>> action)
        {
            var newState = new KeyValueState<TKey, TVal>();
            foreach (var fact in state.Facts)
                newState.Set(fact);

            return action.TakeActionOnState(newState);
        }
    }
}
