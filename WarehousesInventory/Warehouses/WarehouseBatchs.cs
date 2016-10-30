using System.Collections.Generic;
using System.Linq;

namespace WarehousesInventory
{
    public class WarehouseBatchs
    {
        private IList<Batch> batchs;

        public WarehouseBatchs()
        {
            this.batchs = new List<Batch>();
        }

        private WarehouseBatchs(IList<Batch> batchs)
        {
            this.batchs = batchs;
        }

        public void Add(Batch batch)
        {
            this.batchs.Add(batch);
        }

        public WarehouseBatchs FindBatchsBy(string partType)
        {
            return new WarehouseBatchs(this.batchs.Where(b => b.PartType == partType).ToList());
        }

        public bool Exists(string partType)
        {
            return this.batchs.Any(b => b.PartType == partType);
        }

        internal void ReduceStock(int quantity, IPriceStrategy priceStrategy)
        {
            IEnumerable<Batch> strategiedBatchs = priceStrategy.Apply(this.batchs);

            var stockProcessor = new StockProcessor(quantity);

            stockProcessor.Process(strategiedBatchs, (q, batch) =>
            {
                batch.ReduceQuantity(q);

                if (batch.IsEmpty())
                {
                    this.batchs.Remove(batch);
                }
            });
        }

        internal int AccumulatePrice(int quantity, IPriceStrategy priceStrategy)
        {
            IEnumerable<Batch> strategiedBatchs = priceStrategy.Apply(this.batchs);

            int totalPrice = 0;
            var stockProcessor = new StockProcessor(quantity);

            stockProcessor.Process(strategiedBatchs, (q, batch) => totalPrice += batch.GetPriceFor(q));

            return totalPrice;
        }

        internal int GetRemainingParts()
        {
            return this.batchs.Sum(b => b.Quantity);
        }

        internal IList<Batch> ToList()
        {
            return this.batchs;
        }
    }
}