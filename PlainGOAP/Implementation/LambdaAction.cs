using System;
using PlainGOAP.Engine;

namespace PlainGOAP.Implementation
{
    public class LambdaAction<TKey, TVal> : IAction<TKey, TVal>
    {
        public LambdaAction(string name, Func<State<TKey, TVal>, bool> isEligible, Action<State<TKey, TVal>> impactState)
        {
            Name = name;
            IsEligible = isEligible;
            ImpactState = impactState;
        }

        public string Name { get; set; }
        public int Cost => 10;
        // public Fact<TKey, TVal>[] Prerequisites { get; set; } = Array.Empty<Fact<TKey, TVal>>();
        // public Fact<TKey, TVal>[] Effects { get; set; } = Array.Empty<Fact<TKey, TVal>>();


        public Func<State<TKey, TVal>, bool> IsEligible { get; }
        public Action<State<TKey, TVal>> ImpactState { get; }

        public bool CheckPreconditions(State<TKey, TVal> state) => IsEligible(state);
        public void Impact(State<TKey, TVal> state) => ImpactState(state);
    }
}
