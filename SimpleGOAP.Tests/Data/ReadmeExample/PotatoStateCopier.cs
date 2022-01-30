namespace SimpleGOAP.Tests.Data.ReadmeExample
{
    public class PotatoStateCopier : IStateCopier<PotatoState>
    {
        public PotatoState Copy(PotatoState state)
        {
            return new PotatoState
            {
                RawPotatoes = state.RawPotatoes,
                Wood = state.Wood,
                Fire = state.Fire,
                BakedPotatoes = state.BakedPotatoes
            };
        }
    }
}