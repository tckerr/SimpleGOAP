using System.Collections.Generic;

namespace PlainGOAP
{
    public class DefaultDict<TKey, TValue> : Dictionary<TKey, TValue>
    {
        private readonly TValue @default;

        public DefaultDict(TValue defaultValue, IEqualityComparer<TKey> comp) : base(comp)
        {
            @default = defaultValue;
        }

        public DefaultDict(TValue defaultValue)
        {
            @default = defaultValue;
        }

        public new TValue this[TKey key]
        {
            get => TryGetValue(key, out var t) ? t : base[key] = @default;
            set => base[key] = value;
        }
    }
}
