using PlainGOAP.Engine;

namespace PlainGOAP.Implementation
{
    public class LambdaAction<TKey, TValue> : IAction<TKey, TValue>
    {
        public string Name { get; set; }
        public int Cost { get; set; }
        public Fact<TKey, TValue>[] Prerequisites { get; set; } = Array.Empty<Fact<TKey, TValue>>();
        public Fact<TKey, TValue>[] Effects { get; set; } = Array.Empty<Fact<TKey, TValue>>();
        public Action ActionHandler { get; set; } = () => { };

        public void Execute() => ActionHandler();
    }
}
