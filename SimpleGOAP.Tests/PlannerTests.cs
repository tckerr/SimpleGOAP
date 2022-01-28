using System;
using SimpleGOAP.KeyValueState;
using SimpleGOAP.Tests.Data.ReadmeExample;
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
