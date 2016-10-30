using System.Collections.Generic;

namespace WarehousesInventory
{
    public class Warehouses
    {
        private Dictionary<string, WarehouseBatchs> batchs = new Dictionary<string, WarehouseBatchs>();

        public void Add(string warehouse, Batch batch)
        {
            if (!batchs.ContainsKey(warehouse))
            {
                batchs.Add(warehouse, new WarehouseBatchs());
            }

            batchs[warehouse].Add(batch);
        }

        public WarehouseBatchs Filter(string warehouse)
        {
            if (!batchs.ContainsKey(warehouse))
            {
                return new WarehouseBatchs();
            }

            return batchs[warehouse];
        }

        public WarehouseBatchs Filter(PartDescription partDescription)
        {
            if (!Exists(partDescription))
            {
                return new WarehouseBatchs();
            }

            return batchs[partDescription.Warehouse].FindBatchsBy(partDescription.PartType);
        }

        private bool Exists(PartDescription partDescription)
        {
            if (!batchs.ContainsKey(partDescription.Warehouse)) return false;

            return batchs[partDescription.Warehouse].Exists(partDescription.PartType);
        }
    }
}