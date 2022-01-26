using System.Collections.Generic;

namespace PlainGOAP.KeyValueState
{
    public class KeyValueStateComparer<TKey, TVal> : IEqualityComparer<KeyValueState<TKey, TVal>>
    {
        public bool Equals(KeyValueState<TKey, TVal> x, KeyValueState<TKey, TVal> y)
        {
            if (x == null || y == null || x.Facts.Count != y.Facts.Count)
                return false;
            return GetHashCode(x) == GetHashCode(y);
        }

        public int GetHashCode(KeyValueState<TKey, TVal> state)
        {
            var facts = state.Facts;
            if (facts == null)
                return 0;
            unchecked
            {
                var hash = 17;
                foreach (var fact in facts)
                    hash = hash * 23 + (fact.GetHashCode());
                return hash;
            }
        }
    }
}
