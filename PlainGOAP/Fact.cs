namespace PlainGOAP
{
    public struct Fact<TKey, TVal>
    {
        public TKey Key;
        public TVal Value;

        public Fact(TKey key, TVal value)
        {
            Key = key;
            Value = value;
        }
    }
}