namespace WarehousesInventory
{
    public class PartCost
    {
        private const int ZERO_COST = 0;

        public int Total { get; set; } = ZERO_COST;

        public PartCost()
        {
        }

        public PartCost(int cost)
        {
            this.Total = cost;
        }
    }
}