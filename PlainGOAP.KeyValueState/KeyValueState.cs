using System;
using System.Collections.Generic;

namespace PlainGOAP.KeyValueState
{
    public class KeyValueState<TKey, TVal>
    {
        public List<Fact<TKey, TVal>> Facts { get; } = new();
        private readonly Dictionary<TKey, int> indices = new();

        public void Set(TKey key, TVal val)
        {
            if (!indices.TryGetValue(key, out var idx))
            {
                indices[key] = indices.Count;
                Facts.Add(new Fact<TKey, TVal>(key, val));
            }
            else
                Facts[idx] = new Fact<TKey, TVal>(key, val);
        }

        public void Set(IEnumerable<(TKey, TVal)> list)
        {
            foreach (var (key, val) in list)
                Set(key, val);
        }

        public void Set<T>(TKey key, Func<T, T> setter) where T : TVal =>
            Set(key, setter((T) Facts[indices[key]].Value));

        public void Set(Fact<TKey, TVal> fact) => Set(fact.Key, fact.Value);

        public T2 Get<T2>(TKey key) where T2 : TVal
        {
            if (!indices.TryGetValue(key, out var idx))
                throw new Exception($"Fact key '{key}' not registered");
            var val = Facts[idx].Value;
            if (val is not T2 tval)
                throw new Exception($"Fact of type {val.GetType().FullName} is not type {typeof(T2).FullName}");
            return tval;
        }

        public bool Check(Fact<TKey, TVal> fact) => Check(fact.Key, fact.Value);
        public bool Check(TKey key, TVal val) => indices.TryGetValue(key, out var idx) && Facts[idx].Value.Equals(val);
    }
}
