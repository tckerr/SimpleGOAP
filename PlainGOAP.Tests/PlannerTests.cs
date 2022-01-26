using System;
using PlainGOAP.KeyValueState;
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
        public void TestKeyValuePlanner()
        {
            var data = TravelerDataFactory.Create();
            var subject = new KeyValuePlanner();

            var start = DateTime.Now;
            var plan = subject.Execute(data);
            var duration = DateTime.Now - start;

            testOutputHelper.WriteLine($"Plan complete after {duration.TotalMilliseconds}ms:");
            foreach (var step in plan.Steps)
                testOutputHelper.WriteLine($"\t{step.Action.GetName(step.BeforeState)}");
        }
    }
}
