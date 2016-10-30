using System.Collections.Generic;

namespace WarehousesInventory
{
    public interface IPriceStrategy
    {
        IEnumerable<Batch> Apply(IEnumerable<Batch> batches);
    }
}