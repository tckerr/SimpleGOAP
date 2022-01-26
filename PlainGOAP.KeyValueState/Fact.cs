namespace PlainGOAP.KeyValueState
{
    public class Fact<TKey, TVal>
    {
        public TKey Key;
        public TVal Value;

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

        public Fact(TKey key, TVal value)
        {
            Key = key;
            Value = value;
        }
    }
}
