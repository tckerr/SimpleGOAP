namespace PlainGOAP.Tests.Data.Traveler.Actions
{
    public class SellAction : IAction<string, object>
    {
        private string itemName;
        private int amountPerItem;

        public SellAction(string itemName, int amountPerItem)
        {
            this.itemName = itemName;
            this.amountPerItem = amountPerItem;
        }

        public string GetName(State<string, object> state)
        {
            return $"Sell 1 {itemName} for ${amountPerItem} on eBay";
        }

        public int ActionCost => 10;
        public bool CheckPreconditions(State<string, object> state)
        {
            return state.Get<int>(itemName) > 0 && state.Check("myLocation", "Home");
        }

        public void Impact(State<string, object> state)
        {
            state.Set<int>(itemName, f => f - 1);
            state.Set<int>("money", m => m + amountPerItem);
        }
    }
}
