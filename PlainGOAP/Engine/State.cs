namespace PlainGOAP.Engine
{
    public class Fact<TKey, TVal>
    {
        public TKey Key;
        public TVal Value;

        public Fact(TKey key, TVal value)
        {
            Key = key;
            Value = value;
        }
    }

    public class State<TKey, TVal>
    {
        private readonly Dictionary<TKey, TVal> facts = new Dictionary<TKey, TVal>();

        public void Set(TKey key, TVal val)
        {
            facts[key] = val;
        }

        public TVal Get(TKey key) => Get<TVal>(key);

        public T Get<T>(TKey key) where T : TVal
        {
            if (!facts.TryGetValue(key, out var val))
                throw new Exception($"Fact key '{key}' not registered");
            if (!(val is T tval))
                throw new Exception($"Fact of type {val.GetType().FullName} is not type {typeof(T).FullName}");
            return tval;
        }

        public IEnumerable<Fact<TKey, TVal>> ListFacts()
        {
            return facts.Select(f => new Fact<TKey, TVal>(f.Key, f.Value));
        }

        public bool Check(Fact<TKey, TVal> fact) => Check(fact.Key, fact.Value);
        public bool Check(TKey key, TVal val)
        {
            return facts.TryGetValue(key, out var v) && v.Equals(val);
        }
    }
}
