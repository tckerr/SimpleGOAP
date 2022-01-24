using PlainGOAP.Engine;
using PlainGOAP.Implementation;

namespace PlainGOAP
{
    public static class M
    {
        public static void Main()
        {
            var state = new State<string, object>();
            state.Set("atStore", false);
            state.Set("hasFood", false);
            state.Set("full", false);

            var goal = new State<string, object>();
            goal.Set("full", true);
            goal.Set("hasMoney", true);

            var actions = new IAction<string, object>[]
            {
                new LambdaAction<string, object>
                {
                    Name = "WalkToStore",
                    Prerequisites = Array.Empty<Fact<string, object>>(),
                    Effects = new []{new Fact<string, object>("atStore", true)}
                },
                new LambdaAction<string, object>
                {
                    Name = "EarnMoney",
                    Prerequisites = Array.Empty<Fact<string, object>>(),
                    Effects = new []{new Fact<string, object>("hasMoney", true)}
                },
                new LambdaAction<string, object>
                {
                    Name = "BuyFood",
                    Prerequisites = new []
                    {
                        new Fact<string, object>("atStore", true),
                        new Fact<string, object>("hasMoney", true)
                    },
                    Effects = new []
                    {
                        new Fact<string, object>("hasFood", true),
                        new Fact<string, object>("hasMoney", false),
                    }
                },
                new LambdaAction<string, object>
                {
                    Name = "Eat",
                    Prerequisites = new []{new Fact<string, object>("hasFood", true)},
                    Effects = new []{new Fact<string, object>("full", true)}
                }
            };


        }
    }
}


