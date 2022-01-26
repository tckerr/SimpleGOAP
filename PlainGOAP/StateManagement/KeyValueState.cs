using System;
using System.Collections.Generic;
using System.Linq;

namespace PlainGOAP.StateManagement
{
    public class KeyValueStateCopier<TKey, TVal> : IStateCopier<KeyValueState<TKey, TVal>>
    {
        public KeyValueState<TKey, TVal> CopyWithAddedAction(KeyValueState<TKey, TVal> state, IAction<KeyValueState<TKey, TVal>> action)
        {
            var newState = new KeyValueState<TKey, TVal>();
            foreach (var fact in state.ListFacts())
                newState.Set(fact);

            action.TakeActionOnState(newState);

            return newState;
        }
    }

    public class KeyValueState<TKey, TVal> : IState
    {
        private readonly Dictionary<TKey, TVal> facts = new();
        private Fact<TKey, TVal>[] factList = Array.Empty<Fact<TKey, TVal>>();

        public void Set(TKey key, TVal val)
        {
            facts[key] = val;
            factList = facts.Select(f => new Fact<TKey, TVal>(f.Key, f.Value)).ToArray();
        }

        public void Set<T>(TKey key, Func<T, T> setter) where T : TVal
        {
            Set(key, setter((T)facts[key]));
        }

        public void Set(Fact<TKey, TVal> fact)
        {
            Set(fact.Key, fact.Value);
        }

        public T2 Get<T2>(TKey key) where T2 : TVal
        {
            if (!facts.TryGetValue(key, out var val))
                throw new Exception($"Fact key '{key}' not registered");
            if (!(val is T2 tval))
                throw new Exception($"Fact of type {val.GetType().FullName} is not type {typeof(T2).FullName}");
            return tval;
        }

        public Fact<TKey, TVal>[] ListFacts() => factList;

        public bool Check(Fact<TKey, TVal> fact) => Check(fact.Key, fact.Value);
        public bool Check(TKey key, TVal val)
        {
            return facts.TryGetValue(key, out var v) && v.Equals(val);
        }

        public int GetUniqueHashForState()
        {
            var array = ListFacts();
            if (array != null)
                unchecked
                {
                    var hash = 17;

                    foreach (var item in array)
                        hash = hash * 23 + (item?.GetHashCode() ?? 0);

                    return hash;
                }
            return 0;
        }
    }
}
