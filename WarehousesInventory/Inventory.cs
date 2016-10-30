using System.Collections.Generic;

namespace WarehousesInventory
{
    public class Inventory
    {
        private Warehouses warehousesBatches = new Warehouses();

        public PartCost PreviewCheckOut(PartDescription partDescription, int quantity, IPriceStrategy priceStrategy)
        {
            int totalPrice = warehousesBatches.Filter(partDescription)
                                             .AccumulatePrice(quantity, priceStrategy);

            return new PartCost(totalPrice);
        }

        public void CheckIn(Batch batch, string warehouse)
        {
            warehousesBatches.Add(warehouse, batch);
        }

        public void CheckOut(PartDescription partDescription, int quantity, IPriceStrategy priceStrategy)
        {
            warehousesBatches.Filter(partDescription)
                            .ReduceStock(quantity, priceStrategy);
        }

        public int GetQuantity(PartDescription partDescription)
        {
            return warehousesBatches.Filter(partDescription)
                                   .GetRemainingParts();
        }

        public IList<Batch> GetWarehouseBatches(string warehouse)
        {
            return warehousesBatches.Filter(warehouse).ToList();
        }
    }
}