using System.Linq;

namespace SimpleGOAP.Tests.Data.DrumStacker
{
    public class DrumStackerStateCopier : IStateCopier<DrumStackerState>
    {
        public DrumStackerState Copy(DrumStackerState state) =>
            new DrumStackerState
            {
                Stacks = state.Stacks.Select(s => new DrumStack
                {
                    StackColor = s.StackColor,
                    Drums = s.Drums.Select(d => new Drum
                    {
                        Color = d.Color,
                        Size = d.Size
                    }).ToList()
                }).ToList()
            };
    }
}
