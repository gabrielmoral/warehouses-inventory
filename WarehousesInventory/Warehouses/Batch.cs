namespace WarehousesInventory
{
    public class Batch
    {
        private readonly int partPrice;

        public string PartType { get; private set; }
        public int Quantity { get; private set; }

        public Batch(string partType, int partPrice, int quantity)
        {
            this.partPrice = partPrice;
            this.PartType = partType;
            this.Quantity = quantity;
        }

        public int GetTotalBatchPrice()
        {
            return Quantity * partPrice;
        }

        internal int GetPriceFor(int quantity)
        {
            return quantity * partPrice;
        }

        internal void ReduceQuantity(int quantityToProccess)
        {
            Quantity -= quantityToProccess;
        }

        internal bool IsEmpty()
        {
            return Quantity == 0;
        }
    }
}