# What is SimpleGOAP?

SimpleGOAP is a C# implementation of goal oriented action planning. There are some great resources on the topic [for your reading here](https://alumni.media.mit.edu/~jorkin/goap.html). The objectives of this repository are twofold:

1. Provide a simple implementation for anyone to use across a variety of platforms like Unity, Godot, ASP.net, etc.
2. Serve as a reference implementation for GOAP.

# Installation

SimpleGOAP is available on nuget.org through the package ID `SimpleGOAP.Core`. If you want to utilize the `KeyValueState` classes shown in examples below, you'll also need to install the `SimpleGOAP.KeyValueState` package:

1. [SimpleGOAP.Core on nuget.org](https://www.nuget.org/packages/SimpleGOAP.Core/)
1. [SimpleGOAP.KeyValueState on nuget.org](https://www.nuget.org/packages/SimpleGOAP.KeyValueState/)

# Usage

There are 4 steps to using the GOAP planner:

1. **Establishing state**: Define a "state" class that represents the parameters of your world state. Create an object of this type that represents the current world state.
2. **Defining actions**: Create a function which returns a list of actions that can be taken for a given state.
3. **Setting a goal**: Write a function that evaluates whether any permutation of that state adequately satisfies your end goal.
4. **Running the planner**: Pass all of the above into the planner to get a list of actions that can be taken to get from the current world state to a state that meets the goal. 

## Example: Baking potatoes

We are a farmer. Our goal is to harvest and cook 5 baked potatoes. Here are the actions we can take:

1. Harvest potato (+1 raw potato)
2. Chop wood (+1 wood)
3. Make fire (-3 wood, fire = true)
4. Cook potato (-1 raw potato, +1 baked potato)

### Step 1: Defining state

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

In order for the algorithm to function, it needs to be able to copy a state as well as compare two states to see if they are the same. Define two classes for each of theses purposes.

*Copying states is required because the planner must apply actions to a state in order to create possible futures from which a solution can be found. We don't want to modify the object if it is a reference type, and therefore we must generate a copy each time.*

*Equality checks are required because there could be more than one way to reach any given state. If actions arrive at another state which already has a shorter path, that branch will be discarded.*


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
        return new {obj.RawPotatoes, obj.Wood, obj.Fire, obj.BakedPotatoes}.GetHashCode();
    }
}
```

### Step 2: Defining actions

As outlined above, there are 4 actions the user can take: harvest potatoes, chop wood, make fire, and cook potatoes. There are a few properties that define an action:

1. A name.
2. An action cost. The algorithm prioritizes paths with lower costs. The cost can be dynamic depending on the state.
3. An "effect": a function that takes in a state object and returns a modified object. This represents the impact of taking that action.

In code, our actions must implement `IAction<PotatoState>`. You can choose to implement this interface with your own classes, but for simplicity there is an existing implementation -- `LambdaAction<T>` -- which we can use for now. It takes all of the 3 parameters from above in its constructor. 

For now, we'll set all actions costs to 1:

```c#
var harvestPotato = new LambdaAction<PotatoState>(
    "Harvest potato", 1, state => state.RawPotatoes++);
    
var chopWood = new LambdaAction<PotatoState>(
    "Chop wood", 1, state => state.Wood++);
    
var makeFire = new LambdaAction<PotatoState>(
    "Make fire", 1, state =>
        {
            state.Fire = true;
            state.Wood -= 3;
        });
        
var cookPotato = new LambdaAction<PotatoState>(
    "Cook", 1, state =>
        {
            state.RawPotatoes--;
            state.BakedPotatoes++;
        });
```

Now that we have actions defined, let's create our function that takes in a `PotatoState` and returns a list of eligible actions:

```c#
IEnumerable<IAction<PotatoState>> GetActions(PotatoState state)
{
    yield return harvestPotato;
    yield return chopWood;

    if (state.Wood >= 3)
        yield return makeFire;

    if (state.Fire && state.RawPotatoes > 0) 
        yield return cookPotato;
}
```

### Step 3: Setting the goal
Now, we define a function that will tell the engine whether we have reached our goal. In the case of our potato example, we simply want more that 5 baked potatoes:

```c#
Func<PotatoState, bool> goalEvaluator = (state) => state.BakedPotatoes >= 5;
```
We also must define a heuristic function that will tell the engine how close to our goal we are for any given state. The planner will consider lower values to be closer to the goal. In this case, let's use the distance from 5 (our goal) as a heuristic:

```c#
Func<PotatoState,int> heuristicCost = state => 5 - state.BakedPotatoes;
```

*Note: the function above is technically optional; you could always return 0 and the search will still work. However, it's purpose is to suggest possible future paths and therefore can have a drastic effect on performance.*


### Step 4: Running the planner

Finally, instantiate the planner and execute the plan:

```c#
var planner = new Planner<PotatoState>(
    new PotatoStateCopier(),
    new PotatoStateEqualityComparer()
);

var plan = planner.Execute(new PlanParameters<PotatoState>
{
    StartingState = new PotatoState(),
    GetActions = GetActions,
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

#### Review

Our final code looks like so:

```c#
public class PotatoState
{
    public int RawPotatoes = 0;
    public int Wood = 0;
    public bool Fire = false;
    public int BakedPotatoes = 0;
}

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
        return new {obj.RawPotatoes, obj.Wood, obj.Fire, obj.BakedPotatoes}.GetHashCode();
    }
}

public static class PotatoExample {

    public static void Main() {    
        var initialState = new PotatoState();    
        
        var harvestPotato = new LambdaAction<PotatoState>(
            "Harvest potato", 1, state => state.RawPotatoes++);
            
        var chopWood = new LambdaAction<PotatoState>(
            "Chop wood", 1, state => state.Wood++);
            
        var makeFire = new LambdaAction<PotatoState>(
            "Make fire", 1, state =>
                {
                    state.Fire = true;
                    state.Wood -= 3;
                });
                
        var cookPotato = new LambdaAction<PotatoState>(
            "Cook", 1, state =>
                {
                    state.RawPotatoes--;
                    state.BakedPotatoes++;
                });
        
        IEnumerable<IAction<PotatoState>> GetActions(PotatoState state)
        {
            yield return harvestPotato;
            yield return chopWood;
        
            if (state.Wood >= 3)
                yield return makeFire;
        
            if (state.Fire && state.RawPotatoes > 0) 
                yield return cookPotato;
        }
        
        var planner = new Planner<PotatoState>(
            new PotatoStateCopier(),
            new PotatoStateEqualityComparer()
        );
        
        var plan = planner.Execute(new PlanParameters<PotatoState>
        {
            StartingState = initialState,
            GetActions = GetActions,
            HeuristicCost = heuristicCost,
            GoalEvaluator = goalEvaluator
        });
        
        foreach (var step in plan.Steps)
            Console.WriteLine(step.Action.Title);
    }
}
```



# Some comments on implementation

Many implementations of GOAP prefer an expression of actions and state that can be driven from a configuration file. These are more or less static and limit your options. However the approach SimpleGOAP takes is code-first. For example, precondition checks have been eliminated in favor of a user-defined function that takes a state object and returns all possible actions. This allows for more dynamic action lists that morph as state changes.

Note that you could build a more static system on top of SimpleGOAP. In the case of defining actions externally, you could simply have an implementation of `IAction<T>` which returns actions from your master list which pass a precondition check also defined in the config file.
