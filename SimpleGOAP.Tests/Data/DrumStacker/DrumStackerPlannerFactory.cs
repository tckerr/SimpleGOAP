using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleGOAP.Tests.Data.DrumStacker
{
    public static class DrumStackerPlannerFactory
    {
        public static (PlanParameters<DrumStackerState>, Planner<DrumStackerState>) Create()
        {
            var allSizes = (DrumSize[]) Enum.GetValues(typeof(DrumSize));

            int HeuristicCost(DrumStackerState state)
            {
                var count = 0;
                foreach (var stack in state.Stacks)
                {
                    for (var i = 0; i < stack.Drums.Count; i++)
                    {
                        var drum = stack.Drums[i];
                        if (drum.Color != stack.StackColor)
                            count++;

                        if (i < 3 && drum.Size != allSizes[i])
                            count++;
                    }
                }

                return count * 300;
            }

            IEnumerable<IAction<DrumStackerState>> GetActions(DrumStackerState state)
            {
                for (var i = 0; i < state.Stacks.Count; i++)
                {
                    var stack = state.Stacks[i];

                    if (stack.Drums.Count == 0)
                        continue;

                    var drumToMove = stack.Drums[stack.Drums.Count - 1];

                    for (var j = 0; j < state.Stacks.Count; j++)
                    {
                        if (i == j || state.Stacks[j].Drums.Count >= 4)
                            continue;

                        var size = drumToMove.Size;
                        var color = drumToMove.Color;
                        var targetIdx = j;
                        yield return new LambdaAction<DrumStackerState>(
                            $"Move {size} {color} to stack {j}",
                            s =>
                            {
                                var currentStack = s.Stacks
                                    .First(st =>
                                        st.Drums.Any(d => d.Color == color && d.Size == size));

                                var me = currentStack.Drums.First(d =>
                                    d.Color == color && d.Size == size);

                                currentStack.Drums.Remove(me);
                                s.Stacks[targetIdx].Drums.Add(me);
                            });
                    }
                }
            }

            var initialState = new DrumStackerState
            {
                Stacks = new List<DrumStack>
                {
                    new DrumStack
                    {
                        StackColor = DrumColor.Blue,
                        Drums = new List<Drum>
                        {
                            new Drum {Color = DrumColor.Blue, Size = DrumSize.Large},
                            new Drum {Color = DrumColor.Red, Size = DrumSize.Large},
                            new Drum {Color = DrumColor.Red, Size = DrumSize.Small},
                        }
                    },
                    new DrumStack
                    {
                        StackColor = DrumColor.Yellow,
                        Drums = new List<Drum>
                        {
                            new Drum {Color = DrumColor.Yellow, Size = DrumSize.Medium},
                            new Drum {Color = DrumColor.Green, Size = DrumSize.Small},
                            new Drum {Color = DrumColor.Blue, Size = DrumSize.Small},
                        }
                    },
                    new DrumStack
                    {
                        StackColor = DrumColor.Green,
                        Drums = new List<Drum>
                        {
                            new Drum {Color = DrumColor.Yellow, Size = DrumSize.Large},
                            new Drum {Color = DrumColor.Green, Size = DrumSize.Medium},
                            new Drum {Color = DrumColor.Blue, Size = DrumSize.Medium},
                        }
                    },
                    new DrumStack
                    {
                        StackColor = DrumColor.Red,
                        Drums = new List<Drum>
                        {
                            new Drum {Color = DrumColor.Red, Size = DrumSize.Medium},
                            new Drum {Color = DrumColor.Yellow, Size = DrumSize.Small},
                            new Drum {Color = DrumColor.Green, Size = DrumSize.Large},
                        }
                    },
                }
            };

            var data = new PlanParameters<DrumStackerState>
            {
                StartingState = initialState,
                HeuristicCost = HeuristicCost,
                GoalEvaluator = state => HeuristicCost(state) == 0,
                GetActions = GetActions
            };

            var planner = new Planner<DrumStackerState>(
                new DrumStackerStateCopier(),
                new DrumStackerStateComparer()
            );

            return (data, planner);
        }
    }
}
