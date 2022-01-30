using System.Collections.Generic;

namespace SimpleGOAP.Tests.Data.RiverCrossing
{
    public class RiverCrossingStateComparer : IEqualityComparer<RiverCrossingState>
    {
        public bool Equals(RiverCrossingState x, RiverCrossingState y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.Wolf == y.Wolf && x.Goat == y.Goat && x.Cabbage == y.Cabbage && x.Farmer == y.Farmer;
        }

        public int GetHashCode(RiverCrossingState obj)
        {
            unchecked
            {
                var hashCode = obj.Wolf.GetHashCode();
                hashCode = (hashCode * 397) ^ obj.Goat.GetHashCode();
                hashCode = (hashCode * 397) ^ obj.Cabbage.GetHashCode();
                hashCode = (hashCode * 397) ^ obj.Farmer.GetHashCode();
                return hashCode;
            }
        }
    }
}
