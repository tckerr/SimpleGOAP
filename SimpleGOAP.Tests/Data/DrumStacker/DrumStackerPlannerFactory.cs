using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleGOAP.Tests.Data.DrumStacker
{
    public static class DrumStackerPlannerFactory
    {
        public static (PlanParameters<DrumStackerState>, Planner<DrumStackerState>) Create()
        {
            var allColors = (DrumColor[]) Enum.GetValues(typeof(DrumColor));
            var allSizes = (DrumSize[]) Enum.GetValues(typeof(DrumSize));

            int HeuristicCost(DrumStackerState state1)
            {
                var count = 0;
                foreach (var (stack, color) in state1.Stacks.Zip(allColors, (stack, color) => (stack, color)))
                {
                    for (var i = 0; i < stack.Drums.Count; i++)
                    {
                        var drum = stack.Drums[i];
                        if (drum.Color != color)
                            count++;

                        if (i < 3 && drum.Size != allSizes[i])
                            count++;
                    }
                }
                return count * 300;
            }

            var data = new PlanParameters<DrumStackerState>
            {
                StartingState = new DrumStackerState
                {
                    Stacks = new List<DrumStack>
                    {
                        new DrumStack
                        {
                            Drums = new List<Drum>
                            {
                                new Drum{Color = DrumColor.Blue, Size = DrumSize.Large},
                                new Drum{Color = DrumColor.Red, Size = DrumSize.Large},
                                new Drum{Color = DrumColor.Red, Size = DrumSize.Small},
                            }
                        },
                        new DrumStack
                        {
                            Drums = new List<Drum>
                            {
                                new Drum{Color = DrumColor.Yellow, Size = DrumSize.Medium},
                                new Drum{Color = DrumColor.Green, Size = DrumSize.Small},
                                new Drum{Color = DrumColor.Blue, Size = DrumSize.Small},
                            }
                        },
                        new DrumStack
                        {
                            Drums = new List<Drum>
                            {
                                new Drum{Color = DrumColor.Yellow, Size = DrumSize.Large},
                                new Drum{Color = DrumColor.Green, Size = DrumSize.Medium},
                                new Drum{Color = DrumColor.Blue, Size = DrumSize.Medium},
                            }
                        },
                        new DrumStack
                        {
                            Drums = new List<Drum>
                            {
                                new Drum{Color = DrumColor.Red, Size = DrumSize.Medium},
                                new Drum{Color = DrumColor.Yellow, Size = DrumSize.Small},
                                new Drum{Color = DrumColor.Green, Size = DrumSize.Large},
                            }
                        },
                    }
                },
                HeuristicCost = HeuristicCost,
                UseFastQueue = false,
                GoalEvaluator = state => HeuristicCost(state) == 0,
                Actions = allColors
                    .SelectMany(color => allSizes.Select(size => (color, size)))
                    .SelectMany(tuple => Enumerable.Range(0, 4).Select(stack => (tuple.color, tuple.size, stack)))
                    .Select(tuple =>
                    {
                        var (color, size, stack) = tuple;
                        return new LambdaAction<DrumStackerState>(
                            $"Move {size} {color} drum to stack {stack}",
                            1,
                            state =>
                            {
                                var newStack = state.Stacks[stack].Drums;
                                if (newStack.Count == 4)
                                    return false;

                                if (newStack.Any(d => d.Color == color && d.Size == size))
                                    return false;

                                var currentStack = state.Stacks
                                    .First(s => s.Drums.Any(d => d.Color == color && d.Size == size));

                                var me = currentStack.Drums.First(d => d.Color == color && d.Size == size);

                                return currentStack.Drums.IndexOf(me) == currentStack.Drums.Count - 1;
                            },
                            state =>
                            {
                                var currentStack = state.Stacks
                                    .First(s => s.Drums.Any(d => d.Color == color && d.Size == size));

                                var me = currentStack.Drums.First(d => d.Color == color && d.Size == size);

                                currentStack.Drums.Remove(me);
                                state.Stacks[stack].Drums.Add(me);
                            }
                            );

                    })
                    .ToList()
            };

            var planner = new Planner<DrumStackerState>(
                new LambdaCopier<DrumStackerState>(state => new DrumStackerState
                {
                    Stacks = state.Stacks.Select(s => new DrumStack
                    {
                        Drums = s.Drums.Select(d => new Drum
                        {
                             Color = d.Color,
                             Size = d.Size
                        }).ToList()
                    }).ToList()
                }),
                new DrumStackerStateComparer()
            );

            return (data, planner);
        }
    }
}
