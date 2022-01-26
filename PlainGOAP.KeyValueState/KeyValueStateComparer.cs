using System.Collections.Generic;

namespace PlainGOAP.KeyValueState
{
    public class KeyValueStateComparer<TKey, TVal> : IEqualityComparer<KeyValueState<TKey, TVal>>
    {
        public bool Equals(KeyValueState<TKey, TVal> x, KeyValueState<TKey, TVal> y)
        {
            if (x == null || y == null || x.Facts.Length != y.Facts.Length)
                return false;
            return GetHashCode(x) == GetHashCode(y);
        }

        public int GetHashCode(KeyValueState<TKey, TVal> state)
        {
            var array = state.Facts;
            if (array == null)
                return 0;
            unchecked
            {
                var hash = 17;
                foreach (var item in array)
                    hash = hash * 23 + (item?.GetHashCode() ?? 0);
                return hash;
            }
        }
    }
}
