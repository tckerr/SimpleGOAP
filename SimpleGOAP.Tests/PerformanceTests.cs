using System;
using SimpleGOAP.KeyValueState;
using SimpleGOAP.Tests.Data.ReadmeExample;
using SimpleGOAP.Tests.Data.Traveler;
using Xunit;
using Xunit.Abstractions;

namespace SimpleGOAP.Tests
{
    public class PerformanceTests
    {
        private readonly ITestOutputHelper testOutputHelper;

        public PerformanceTests(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
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
