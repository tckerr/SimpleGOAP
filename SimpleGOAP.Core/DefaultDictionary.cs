using System.Collections.Generic;

namespace SimpleGOAP
{
    internal class DefaultDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        private readonly TValue defaultValue;

        internal DefaultDictionary(TValue defaultValue, IEqualityComparer<TKey> comp) : base(comp)
        {
            this.defaultValue = defaultValue;
        }

        internal new TValue this[TKey key]
        {
            get => TryGetValue(key, out var t) ? t : base[key] = defaultValue;
            set => base[key] = value;
        }
    }
}
