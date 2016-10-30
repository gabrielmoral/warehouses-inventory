using System.Collections.Generic;

namespace WarehousesInventory
{
    public class Inventory
    {
        private Warehouses warehousesBatchs = new Warehouses();

        public PartCost PreviewCheckOut(PartDescription partDescription, int quantity, IPriceStrategy priceStrategy)
        {
            int totalPrice = warehousesBatchs.Filter(partDescription)
                                             .AccumulatePrice(quantity, priceStrategy);

            return new PartCost(totalPrice);
        }

        public void CheckIn(Batch batch, string warehouse)
        {
            warehousesBatchs.Add(warehouse, batch);
        }

        public void CheckOut(PartDescription partDescription, int quantity, IPriceStrategy priceStrategy)
        {
            warehousesBatchs.Filter(partDescription)
                            .ReduceStock(quantity, priceStrategy);
        }

        public int GetQuantity(PartDescription partDescription)
        {
            return warehousesBatchs.Filter(partDescription)
                                   .GetRemainingParts();
        }

        public IList<Batch> GetWarehouseBatchs(string warehouse)
        {
            return warehousesBatchs.Filter(warehouse).ToList();
        }
    }
}