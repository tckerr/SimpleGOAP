using System;
using System.Collections.Generic;
using System.Linq;
using PlainGOAP.StateManagement;

namespace PlainGOAP.Tests.Data.Traveler.Actions
{
    public class DriveAction : IAction<KeyValueState<string, object>>
    {
        private const double GasPerDistance = 10;
        private readonly string location;
        private readonly List<(string, int, int)> locations;

        public DriveAction(string location, List<(string, int, int)> locations)
        {
            this.location = location;
            this.locations = locations;
        }

        public string GetName(KeyValueState<string, object> state) => $"Drive to {location}, using {GetGasCost(state)} gas. {state.Get<int>("gas")-GetGasCost(state)} left";
        public int ActionCost => 10;

        private int GetGasCost(KeyValueState<string, object> state)
        {
            var currentLocation = state.Get<string>("myLocation");
            var (_, currentX, currentY) = locations.First(l => l.Item1 == currentLocation);
            var (_, targetX, targetY) = locations.First(l => l.Item1 == location);
            var distance = Math.Sqrt(Math.Pow(targetX - currentX, 2) + Math.Pow(targetY - currentY, 2));
            return Convert.ToInt32(distance * GasPerDistance);
        }

        public bool CheckPreconditions(KeyValueState<string, object> state)
        {
            return state.Get<int>("gas") >= GetGasCost(state);
        }

        public void TakeActionOnState(KeyValueState<string, object> state)
        {
            var gasCost = GetGasCost(state);
            state.Set("gas", state.Get<int>("gas") - gasCost);
            state.Set("myLocation", location);
        }
    }
}
