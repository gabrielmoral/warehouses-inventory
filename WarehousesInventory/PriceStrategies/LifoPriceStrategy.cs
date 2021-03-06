﻿using System.Collections.Generic;
using System.Linq;

namespace WarehousesInventory
{
    public class LifoPriceStrategy : IPriceStrategy
    {
        public IEnumerable<Batch> Apply(IEnumerable<Batch> batches)
        {
            return batches.Reverse();
        }
    }
}