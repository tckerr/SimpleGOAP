using System;
using SimpleGOAP.KeyValueState;
using SimpleGOAP.Tests.Data.ReadmeExample;
using SimpleGOAP.Tests.Data.Traveler;
using SimpleGOAP.Tests.Data.Wolf;
using Xunit;
using Xunit.Abstractions;

namespace SimpleGOAP.Tests
{
    public class PlannerTests
    {
        private readonly ITestOutputHelper testOutputHelper;

        public PlannerTests(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void TestReadmeExample()
        {
            var subject = new Planner<PotatoState>(
                new PotatoStateCopier(),
                new PotatoStateEqualityComparer()
            );

            Func<PotatoState, bool> goalEvaluator = state => state.BakedPotatoes >= 5;
            Func<PotatoState,int> heuristicCost = state => 5 - state.BakedPotatoes;
            var plan = subject.Execute(new PlanParameters<PotatoState>
            {
                StartingState = new PotatoState(),
                Actions = new[]
                {
                    new LambdaAction<PotatoState>("Harvest potato", 1,
                        state => state.RawPotatoes++),

                    new LambdaAction<PotatoState>("Chop wood", 1,
                        state => state.Wood++),

                    new LambdaAction<PotatoState>("Make fire", 1,
                        state => state.Wood >= 3,
                        state =>
                        {
                            state.Fire = true;
                            state.Wood -= 3;
                        }),

                    new LambdaAction<PotatoState>("Cook", 1,
                        state => state.Fire && state.RawPotatoes > 0,
                        state =>
                        {
                            state.RawPotatoes--;
                            state.BakedPotatoes++;
                        }),
                },
                HeuristicCost = heuristicCost,
                GoalEvaluator = goalEvaluator
            });

            foreach (var step in plan.Steps)
                testOutputHelper.WriteLine(step.Action.Title);
        }

        [Fact]
        public void TestTravelerExample()
        {
            var data = TravelerDataFactory.Create();
            var subject = new KeyValuePlanner();

            var start = DateTime.Now;
            var plan = subject.Execute(data);
            var duration = DateTime.Now - start;

            testOutputHelper.WriteLine($"Plan complete after {duration.TotalMilliseconds}ms:");
            foreach (var step in plan.Steps)
                testOutputHelper.WriteLine($"\t{step.Action.Title}");

            Assert.True(plan.Success);
        }

        [Fact]
        public void TestRiverCrossing()
        {
            var data = RiverCrossingDataFactory.Create();
            var subject = new Planner<RiverCrossingState>(
                new LambdaCopier<RiverCrossingState>(state => new RiverCrossingState
                {
                    Cabbage = state.Cabbage,
                    Goat = state.Goat,
                    Wolf = state.Wolf,
                    Farmer = state.Farmer
                }),
                new RiverCrossingStateComparer()
            );

            var start = DateTime.Now;
            var plan = subject.Execute(data);
            var duration = DateTime.Now - start;

            testOutputHelper.WriteLine($"Plan complete after {duration.TotalMilliseconds}ms:");
            foreach (var step in plan.Steps)
                testOutputHelper.WriteLine($"\t{step.Action.Title}");

            Assert.True(plan.Success);
            Assert.Equal("Move goat left", plan.Steps[0].Action.Title);
            Assert.Equal("Return", plan.Steps[1].Action.Title);
            Assert.Equal("Move wolf left", plan.Steps[2].Action.Title);
            Assert.Equal("Move goat right", plan.Steps[3].Action.Title);
            Assert.Equal("Move cabbage left", plan.Steps[4].Action.Title);
            Assert.Equal("Return", plan.Steps[5].Action.Title);
            Assert.Equal("Move goat left", plan.Steps[6].Action.Title);
        }

        [Fact]
        public void TestKeyValuePlannerFailsWhenNoActions()
        {
            var subject = new KeyValuePlanner();

            var plan = subject.Execute(new PlanParameters<KeyValueState<string, object>>
            {
                Actions = Array.Empty<IAction<KeyValueState<string, object>>>(),
                GoalEvaluator = g => false,
                HeuristicCost = g => 0,
                StartingState = new KeyValueState<string, object>()
            });

            Assert.False(plan.Success);
        }

        [Fact]
        public void TestKeyValuePlannerThrowsWhenArgsNull()
        {
            var subject = new KeyValuePlanner();
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                subject.Execute(new PlanParameters<KeyValueState<string, object>>());
            });
        }
    }
}
