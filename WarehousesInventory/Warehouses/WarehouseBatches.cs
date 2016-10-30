using System.Collections.Generic;
using System.Linq;

namespace WarehousesInventory
{
    public class WarehouseBatches
    {
        private IList<Batch> batches;

        public WarehouseBatches()
        {
            this.batches = new List<Batch>();
        }

        private WarehouseBatches(IList<Batch> batches)
        {
            this.batches = batches;
        }

        public void Add(Batch batch)
        {
            this.batches.Add(batch);
        }

        public WarehouseBatches FindBatchesBy(string partType)
        {
            return new WarehouseBatches(this.batches.Where(b => b.PartType == partType).ToList());
        }

        public bool Exists(string partType)
        {
            return this.batches.Any(b => b.PartType == partType);
        }

        internal void ReduceStock(int quantity, IPriceStrategy priceStrategy)
        {
            IEnumerable<Batch> strategiedBatches = priceStrategy.Apply(this.batches);

            var stockProcessor = new StockProcessor(quantity);

            stockProcessor.Process(strategiedBatches, (q, batch) =>
            {
                batch.ReduceQuantity(q);

                if (batch.IsEmpty())
                {
                    this.batches.Remove(batch);
                }
            });
        }

        internal int AccumulatePrice(int quantity, IPriceStrategy priceStrategy)
        {
            IEnumerable<Batch> strategiedBatches = priceStrategy.Apply(this.batches);

            int totalPrice = 0;
            var stockProcessor = new StockProcessor(quantity);

            stockProcessor.Process(strategiedBatches, (q, batch) => totalPrice += batch.GetPriceFor(q));

            return totalPrice;
        }

        internal int GetRemainingParts()
        {
            return this.batches.Sum(b => b.Quantity);
        }

        internal IList<Batch> ToList()
        {
            return this.batches;
        }
    }
}