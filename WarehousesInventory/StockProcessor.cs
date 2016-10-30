using System;
using System.Collections.Generic;
using System.Linq;

namespace WarehousesInventory
{
    internal class StockProcessor
    {
        private int remainingQuantity;

        public StockProcessor(int quantity)
        {
            this.remainingQuantity = quantity;
        }

        public void Process(IEnumerable<Batch> batchs, Action<int, Batch> action)
        {
            batchs.ToList().ForEach(batch =>
            {
                if (QuantityIsProcessed()) return;

                int quantityToProcess = GetQuantityToProcess(batch);

                action(quantityToProcess, batch);

                ReduceRemainingQuantity(quantityToProcess);
            });
        }

        private bool QuantityIsProcessed()
        {
            return remainingQuantity == 0;
        }

        private int GetQuantityToProcess(Batch batch)
        {
            bool enoughQuantityInBatch = remainingQuantity <= batch.Quantity;

            return enoughQuantityInBatch ? remainingQuantity : batch.Quantity;
        }

        private void ReduceRemainingQuantity(int quantityToProccess)
        {
            remainingQuantity -= quantityToProccess;
        }
    }
}