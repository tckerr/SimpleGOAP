using System.Collections.Generic;

namespace SimpleGOAP.Tests.Data.DrumStacker
{
    public class DrumStackerStateComparer : IEqualityComparer<DrumStackerState>
    {
        public bool Equals(DrumStackerState x, DrumStackerState y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;

            if (x.Stacks.Count != y.Stacks.Count)
                return false;

            for (var i = 0; i < x.Stacks.Count; i++)
            {
                var xStack = x.Stacks[i];
                var yStack = y.Stacks[i];
                if (yStack.Drums.Count != xStack.Drums.Count)
                    return false;

                for (var j = 0; j < xStack.Drums.Count; j++)
                {
                    var xDrum = xStack.Drums[j];
                    var yDrum = yStack.Drums[j];
                    if (xDrum.Color != yDrum.Color)
                        return false;
                    if (xDrum.Size != yDrum.Size)
                        return false;
                }
            }
            return true;
        }

        public int GetHashCode(DrumStackerState obj)
        {
            int Hash(IReadOnlyList<Drum> drums, int idx)
            {
                if (idx >= drums.Count)
                    return 1;
                var drum = drums[idx];
                return drum == null ? 0 : new {drum.Color, drum.Size}.GetHashCode();
            }

            return new
            {
                a1 = Hash(obj.Stacks[0].Drums, 0),
                a2 = Hash(obj.Stacks[0].Drums, 1),
                a3 = Hash(obj.Stacks[0].Drums, 2),
                a4 = Hash(obj.Stacks[0].Drums, 3),
                b1 = Hash(obj.Stacks[1].Drums, 0),
                b2 = Hash(obj.Stacks[1].Drums, 1),
                b3 = Hash(obj.Stacks[1].Drums, 2),
                b4 = Hash(obj.Stacks[1].Drums, 3),
                c1 = Hash(obj.Stacks[2].Drums, 0),
                c2 = Hash(obj.Stacks[2].Drums, 1),
                c3 = Hash(obj.Stacks[2].Drums, 2),
                c4 = Hash(obj.Stacks[2].Drums, 3),
                d1 = Hash(obj.Stacks[3].Drums, 0),
                d2 = Hash(obj.Stacks[3].Drums, 1),
                d3 = Hash(obj.Stacks[3].Drums, 2),
                d4 = Hash(obj.Stacks[3].Drums, 3),
            }.GetHashCode();
        }
    }
}
