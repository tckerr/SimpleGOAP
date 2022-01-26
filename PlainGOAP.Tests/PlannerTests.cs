using System;
using System.Linq;
using PlainGOAP.StateManagement;
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
        public void TestTravelerData()
        {
            var data = TravelerDataFactory.Create();
            var subject = new Planner<KeyValueState<string, object>>(
                new KeyValueStateCopier<string, object>());

            var start = DateTime.Now;
            var plan = subject.Execute(data, int.MaxValue).ToArray();
            testOutputHelper.WriteLine($"Plan complete after {(DateTime.Now - start).TotalMilliseconds}ms");

            for (var i = 1; i < plan.Length; i++)
            {
                var state = plan[i];
                testOutputHelper.WriteLine(state.SourceAction?.GetName(plan[i-1].State) ?? "null");
            }
        }
    }
}
