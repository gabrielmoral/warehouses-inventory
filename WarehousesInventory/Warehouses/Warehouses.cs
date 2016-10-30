using System.Collections.Generic;

namespace WarehousesInventory
{
    public class Warehouses
    {
        private Dictionary<string, WarehouseBatches> batches = new Dictionary<string, WarehouseBatches>();

        public void Add(string warehouse, Batch batch)
        {
            if (!batches.ContainsKey(warehouse))
            {
                batches.Add(warehouse, new WarehouseBatches());
            }

            batches[warehouse].Add(batch);
        }

        public WarehouseBatches Filter(string warehouse)
        {
            if (!batches.ContainsKey(warehouse))
            {
                return new WarehouseBatches();
            }

            return batches[warehouse];
        }

        public WarehouseBatches Filter(PartDescription partDescription)
        {
            if (!Exists(partDescription))
            {
                return new WarehouseBatches();
            }

            return batches[partDescription.Warehouse].FindBatchesBy(partDescription.PartType);
        }

        private bool Exists(PartDescription partDescription)
        {
            if (!batches.ContainsKey(partDescription.Warehouse)) return false;

            return batches[partDescription.Warehouse].Exists(partDescription.PartType);
        }
    }
}