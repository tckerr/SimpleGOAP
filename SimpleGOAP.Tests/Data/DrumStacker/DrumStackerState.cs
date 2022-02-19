using System.Collections.Generic;

namespace SimpleGOAP.Tests.Data.DrumStacker
{
    public enum DrumColor
    {
        Blue,
        Yellow,
        Green,
        Red
    }
    public enum DrumSize
    {
        Large,
        Medium,
        Small,
    }

    public class Drum
    {
        public DrumColor Color;
        public DrumSize Size;
    }

    public class DrumStack
    {
        public DrumColor StackColor;
        public List<Drum> Drums = new List<Drum>
        {
            new Drum(),
            new Drum(),
            new Drum(),
            new Drum(),
        };
    }

    public class DrumStackerState
    {
        public List<DrumStack> Stacks = new List<DrumStack>
        {
            new DrumStack{StackColor = DrumColor.Blue},
            new DrumStack{StackColor = DrumColor.Yellow},
            new DrumStack{StackColor = DrumColor.Green},
            new DrumStack{StackColor = DrumColor.Red},
        };
    }
}
