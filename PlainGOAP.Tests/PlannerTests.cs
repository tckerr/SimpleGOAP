using System;
using System.Linq;
using PlainGOAP.Engine;
using PlainGOAP.Tests.Data;
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
            var data = new Traveler().Create();
            var start = DateTime.Now;

            var plan = AStarSearch.FindPath(data).ToArray();

            testOutputHelper.WriteLine($"Plan complete after {(DateTime.Now - start).TotalMilliseconds}ms");

            for (var i = 1; i < plan.Length; i++)
            {
                var state = plan[i];
                testOutputHelper.WriteLine(state.CameFrom?.GetName(plan[i-1].State) ?? "null");
            }
        }
    }
}
