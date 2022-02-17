using System;
using SimpleGOAP.KeyValueState;
using SimpleGOAP.Tests.Data.ReadmeExample;
using SimpleGOAP.Tests.Data.RiverCrossing;
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
            var (args, planner) = PotatoStatePlannerFactory.Create();
            foreach (var step in planner.Execute(args).Steps)
                testOutputHelper.WriteLine(step.Action.Title);
        }

        [Fact]
        public void TestTravelerExample()
        {
            var (data, subject) = TravelerDataFactory.Create();

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
            // https://en.wikipedia.org/wiki/Wolf,_goat_and_cabbage_problem
            var (data, subject) = RiverCrossingPlannerFactory.Create();

            var start = DateTime.Now;
            var plan = subject.Execute(data);
            var duration = DateTime.Now - start;

            testOutputHelper.WriteLine($"Plan complete after {duration.TotalMilliseconds}ms:");
            foreach (var step in plan.Steps)
                testOutputHelper.WriteLine($"\t{step.Action.Title}");

            Assert.True(plan.Success);
            Assert.Equal("Move goat left", plan.Steps[0].Action.Title);
            Assert.Equal("Return", plan.Steps[1].Action.Title);
            Assert.Equal("Move cabbage left", plan.Steps[2].Action.Title);
            Assert.Equal("Move goat right", plan.Steps[3].Action.Title);
            Assert.Equal("Move wolf left", plan.Steps[4].Action.Title);
            Assert.Equal("Return", plan.Steps[5].Action.Title);
            Assert.Equal("Move goat left", plan.Steps[6].Action.Title);
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
