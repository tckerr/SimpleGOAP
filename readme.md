# Usage

## Example: Baking potatoes

Our goal is to cook 5 Baked Potatoes. We start with 0. Here are the actions we can take:

1. Harvest potato +1 raw potato)
2. Chop wood (+1 wood)
3. Make fire (-3 wood, fire = true)
4. Cook potato (-1 raw potato, +1 baked potato)

### Defining state

Let's start by creating our state class:

```c#
public class PotatoState
{
    public int RawPotatoes = 0;
    public int Wood = 0;
    public bool Fire = false;
    public int BakedPotatoes = 0;
}
```

In order for the algorithm to function, it needs to be able to compare two states to see if they are the same, and it also needs to be able to copy a state. Define two classes for each of theses purposes:

```c#
public class PotatoStateCopier : IStateCopier<PotatoState>
{
    public PotatoState Copy(PotatoState state)
    {
        return new PotatoState
        {
            Potatoes = state.RawPotatoes,
            Wood = state.Wood,
            Fire = state.Fire,
            BakedPotatoes = state.BakedPotatoes
        };
    }
}

public class PotatoStateEqualityComparer : IEqualityComparer<PotatoState>
{
    public bool Equals(PotatoState x, PotatoState y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.GetType() != y.GetType()) return false;
        return x.RawPotatoes == y.RawPotatoes && x.Wood == y.Wood && x.Fire == y.Fire && x.BakedPotatoes == y.BakedPotatoes;
    }

    public int GetHashCode(PotatoState obj)
    {
        return HashCode.Combine(obj.RawPotatoes, obj.Wood, obj.Fire, obj.BakedPotatoes);
    }
}
```

### Setting the goal
Next, let's define a function that will tell the engine whether we have reached out goal:

```c#
Func<PotatoState, bool> goalEvaluator = (state) => state.BakedPotatoes >= 5;
```
Let's also define a function that will tell the engine how close to our goal we are for any given state. Lower values are better:

```c#
Func<PotatoState,int> heuristicCost = state => 5 - state.BakedPotatoes;
```

### Creating actions

As outlined above, there are 4 actions the user can take: harvest potatoes, chop wood, make fire, and cook potatoes. In code, our actions must implement `IAction<PotatoState>`. Let's use the built-in `LambdaAction<T>`. Some actions require precondition checks, while others do not:

```c#
var actionsList = new[]
{
    new LambdaAction<PotatoState>("Harvest potato", 1, state => state.RawPotatoes++),
    new LambdaAction<PotatoState>("Chop wood", 1, state => state.Wood++),
    new LambdaAction<PotatoState>("Make fire", 1,
        state => state.Wood >= 3,
        state =>
        {
            state.Fire = true;
            state.Wood -= 3;
        }),
    new LambdaAction<PotatoState>("Cook", 1,
        state => state.Fire && state.Potatoes > 0,
        state =>
        {
            state.RawPotatoes--;
            state.BakedPotatoes++;
        }),
};
```

### Running the planner

Finally, instantiate an instance of the planner and execute the plan:

```c#
var planner = new Planner<PotatoState>(
    new PotatoStateCopier(),
    new PotatoStateEqualityComparer()
);

var plan = planner.Execute(new PlanParameters<PotatoState>
{
    StartingState = new PotatoState(),
    Actions = actionList,
    HeuristicCost = heuristicCost,
    GoalEvaluator = goalEvaluator
});

foreach (var step in plan.Steps)
    Console.WriteLine(step.Action.Title);
```

The output:

```
Chop wood
Chop wood
Chop wood
Make fire
Harvest potato
Cook
Harvest potato
Cook
Harvest potato
Cook
Harvest potato
Cook
Harvest potato
Cook
```
