using SimpleGOAP.KeyValueState;

namespace SimpleGOAP.Tests.Data.Traveler.Actions
{
    public class SellAction : IAction<KeyValueState<string, object>>
    {
        private string itemName;
        private int amountPerItem;

        public SellAction(string itemName, int amountPerItem)
        {
            this.itemName = itemName;
            this.amountPerItem = amountPerItem;
        }

        public string Title => $"Sell 1 {itemName} for ${amountPerItem} on eBay";

        public int Cost => 10;
        public bool IsLegalForState(KeyValueState<string, object> state)
        {
            return state.Get<int>(itemName) > 0 && state.Check("myLocation", "Home");
        }

        public KeyValueState<string, object> TakeActionOnState(KeyValueState<string, object> state)
        {
            state.Set<int>(itemName, f => f - 1);
            state.Set<int>("money", m => m + amountPerItem);
            return state;
        }
    }
}
