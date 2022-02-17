using System.Collections.Generic;

namespace SimpleGOAP.Tests.Data.ReadmeExample
{
    public class PotatoStateEqualityComparer : IEqualityComparer<PotatoState>
    {
        public bool Equals(PotatoState x, PotatoState y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.RawPotatoes == y.RawPotatoes && x.Wood == y.Wood && x.Fire == y.Fire && x.BakedPotatoes == y.BakedPotatoes;
        }

        public int GetHashCode(PotatoState obj)
        {
            return new {obj.RawPotatoes, obj.Wood, obj.Fire, obj.BakedPotatoes}.GetHashCode();
        }
    }
}