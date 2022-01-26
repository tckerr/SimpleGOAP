using System;
using System.Collections.Generic;
using System.Linq;

namespace PlainGOAP
{
    public class State<TKey, TVal> : IState<TKey, TVal>
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

        public T Get<T>(TKey key) where T : TVal
        {
            if (!facts.TryGetValue(key, out var val))
                throw new Exception($"Fact key '{key}' not registered");
            if (!(val is T tval))
                throw new Exception($"Fact of type {val.GetType().FullName} is not type {typeof(T).FullName}");
            return tval;
        }

        public Fact<TKey, TVal>[] ListFacts() => factList;

        public bool Check(Fact<TKey, TVal> fact) => Check(fact.Key, fact.Value);
        public bool Check(TKey key, TVal val)
        {
            return facts.TryGetValue(key, out var v) && v.Equals(val);
        }

        public State<TKey,TVal> CopyAddAction(IAction<TKey,TVal> action)
        {
            var state = new State<TKey, TVal>();
            foreach (var fact in ListFacts())
                state.Set(fact);

            action.TakeActionOnState(state);

            return state;
        }

        public int GetUniqueHash() => ArrayComparer.GetHashCode(ListFacts());
    }
}
