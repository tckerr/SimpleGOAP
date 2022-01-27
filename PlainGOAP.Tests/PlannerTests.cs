using System;
using PlainGOAP.KeyValueState;
using PlainGOAP.Tests.Data.ReadmeExample;
using PlainGOAP.Tests.Data.Traveler;
using Xunit;
using Xunit.Abstractions;

namespace PlainGOAP.Tests
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
        public void TestKeyValuePlanner()
        {
            var data = TravelerDataFactory.Create();
            var subject = new KeyValuePlanner();

            var start = DateTime.Now;
            var plan = subject.Execute(data);
            var duration = DateTime.Now - start;

            testOutputHelper.WriteLine($"Plan complete after {duration.TotalMilliseconds}ms:");
            foreach (var step in plan.Steps)
                testOutputHelper.WriteLine($"\t{step.Action.Title}");
        }

        [Fact]
        public void TestKeyValuePlannerPerformance()
        {
            var data = TravelerDataFactory.Create();
            var subject = new KeyValuePlanner();

            var start = DateTime.Now;
            var iterations = 100;
            for (var i = 0; i < iterations; i++)
                subject.Execute(data);
            var duration = DateTime.Now - start;

            testOutputHelper.WriteLine($"Plan x{iterations} complete after {duration.TotalSeconds}s:");
        }
    }
}
