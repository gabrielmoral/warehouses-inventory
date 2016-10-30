using System.Collections.Generic;

namespace WarehousesInventory
{
    public class FifoPriceStrategy : IPriceStrategy
    {
        public IEnumerable<Batch> Apply(IEnumerable<Batch> batches)
        {
            return batches;
        }
    }
}