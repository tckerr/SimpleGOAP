namespace PlainGOAP.KeyValueState
{
    public class Fact<TKey, TVal>
    {
        public readonly TKey Key;
        public readonly TVal Value;

        public Fact(TKey key, TVal value)
        {
            Key = key;
            Value = value;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + Key.GetHashCode();
                hash = hash * 23 + Value.GetHashCode();
                return hash;
            }
        }
    }
}
