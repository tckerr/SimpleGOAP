using System;
using System.Linq;
using SimpleGOAP.KeyValueState;
using SimpleGOAP.Tests.Data.DrumStacker;
using SimpleGOAP.Tests.Data.ReadmeExample;
using SimpleGOAP.Tests.Data.RiverCrossing;
using SimpleGOAP.Tests.Data.Traveler;
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
        public void TestDrumStacker()
        {
            var (data, subject) = DrumStackerPlannerFactory.Create();

            var start = DateTime.Now;
            var plan = subject.Execute(data);
            var duration = DateTime.Now - start;

            string render(Drum drum)
            {
                return $"[{drum.Color.ToString()[0]}{drum.Size.ToString()[0]}]";
            }

            testOutputHelper.WriteLine($"Plan complete after {duration.TotalMilliseconds}ms:");
            for (var i = 0; i < plan.Steps.Count; i++)
            {
                var step = plan.Steps[i];
                testOutputHelper.WriteLine($"\t{i}: {step.Action.Title}");

                var line1 = step.AfterState.Stacks.Select(stack => stack.Drums.Count > 0 ? render(stack.Drums[0]) : "[  ]");
                var line2 = step.AfterState.Stacks.Select(stack => stack.Drums.Count > 1 ? render(stack.Drums[1]) : "[  ]");
                var line3 = step.AfterState.Stacks.Select(stack => stack.Drums.Count > 2 ? render(stack.Drums[2]) : "[  ]");
                var line4 = step.AfterState.Stacks.Select(stack => stack.Drums.Count > 3 ? render(stack.Drums[3]) : "[  ]");


                testOutputHelper.WriteLine("BLU-YEL-GRE-RED-");
                testOutputHelper.WriteLine(string.Join("", line4));
                testOutputHelper.WriteLine(string.Join("", line3));
                testOutputHelper.WriteLine(string.Join("", line2));
                testOutputHelper.WriteLine(string.Join("", line1));
            }

            Assert.True(plan.Success);
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
