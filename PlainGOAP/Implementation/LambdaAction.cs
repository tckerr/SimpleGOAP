using System;
using System.Linq;
using PlainGOAP.Engine;

namespace PlainGOAP.Implementation
{
    public class LambdaAction<TKey, TVal> : IAction<TKey, TVal>
    {
        public string Name { get; set; }
        public int Cost => 10;
        public Fact<TKey, TVal>[] Prerequisites { get; set; } = Array.Empty<Fact<TKey, TVal>>();
        public Fact<TKey, TVal>[] Effects { get; set; } = Array.Empty<Fact<TKey, TVal>>();

        public bool CheckPreconditions(State<TKey, TVal> state) => Prerequisites.All(state.Check);
        public void Impact(State<TKey, TVal> state)
        {
            foreach (var effect in Effects)
            {
                state.Set(effect);
            }
        }
    }
}
