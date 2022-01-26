using System;

namespace PlainGOAP.Tests.Data.Traveler.Actions
{
    public class PurchaseAction : IAction<string, object>
    {
        private readonly string itemName;
        private readonly string storeName;
        private readonly int cost;
        private readonly int amountPerPurchase;
        private readonly int? max;

        public PurchaseAction(string itemName, string storeName, int cost, int amountPerPurchase, int? max = null)
        {
            this.max = max;
            this.itemName = itemName;
            this.storeName = storeName;
            this.cost = cost;
            this.amountPerPurchase = amountPerPurchase;
        }

        public string GetName(State<string, object> state) => $"Purchase {itemName} x{amountPerPurchase} for ${cost}";
        public int ActionCost => 10;

        public bool CheckPreconditions(State<string, object> state)
        {
            return state.Get<int>("money") >= cost
                   && state.Check("myLocation", storeName)
                   && (max == null || state.Get<int>(itemName) < max);
        }

        public void TakeActionOnState(State<string, object> state)
        {
            state.Set(itemName, Math.Min(max ?? int.MaxValue, state.Get<int>(itemName) + amountPerPurchase));
            state.Set("money", state.Get<int>("money") - cost);
        }
    }
}
