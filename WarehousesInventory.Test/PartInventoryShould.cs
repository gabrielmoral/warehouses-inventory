using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace WarehousesInventory.Test
{
    [TestClass]
    public class PartInventoryShould
    {
        [TestMethod]
        public void Check_In_Parts()
        {
            const string partType = "Part A";
            const string warehouse = "Dublin";
            var partDescription = new PartDescription(partType, warehouse);

            var inventory = new Inventory();
            var batch = new Batch(partType, partPrice: 100, quantity: 1);

            inventory.CheckIn(batch, warehouse);

            PartCost cost = inventory.PreviewCheckOut(partDescription,
                                                      quantity: 1,
                                                      priceStrategy: new FifoPriceStrategy());
            cost.Total.Should().Be(100);
        }

        [TestMethod]
        public void Return_Zero_Cost_If_Part_Do_Not_Exists()
        {
            var partDescription = new PartDescription("Part A", "Dublin");

            var inventory = new Inventory();

            PartCost cost = inventory.PreviewCheckOut(partDescription,
                                                      quantity: 2,
                                                      priceStrategy: new FifoPriceStrategy());
            cost.Total.Should().Be(0);
        }

        [TestMethod]
        public void Return_Cost_Having_One_Batch_And_Enough_Quantity_Of_That_Part()
        {
            const string partType = "Part A";
            const string warehouse = "Dublin";
            var partDescription = new PartDescription(partType, warehouse);

            var inventory = new Inventory();
            var batch = new Batch(partType, partPrice: 100, quantity: 2);

            inventory.CheckIn(batch, warehouse);

            PartCost cost = inventory.PreviewCheckOut(partDescription,
                                                      quantity: 2,
                                                      priceStrategy: new FifoPriceStrategy());
            cost.Total.Should().Be(200);
        }

        [TestMethod]
        public void Return_Cost_Having_Several_Batches_Of_That_Part_With_FIFO_Strategy()
        {
            const string partType = "Part A";
            const string warehouse = "Dublin";
            var partDescription = new PartDescription(partType, warehouse);

            var inventory = new Inventory();
            var batch1 = new Batch(partType, partPrice: 100, quantity: 2);
            var batch2 = new Batch(partType, partPrice: 150, quantity: 4);

            inventory.CheckIn(batch1, warehouse);
            inventory.CheckIn(batch2, warehouse);

            PartCost cost = inventory.PreviewCheckOut(partDescription,
                                                     quantity: 3,
                                                     priceStrategy: new FifoPriceStrategy());
            cost.Total.Should().Be(350);
        }

        [TestMethod]
        public void Return_Cost_Having_Several_Batches_Of_That_Part_With_LIFO_Strategy()
        {
            const string partType = "Part A";
            const string warehouse = "Dublin";
            var partDescription = new PartDescription(partType, warehouse);

            var inventory = new Inventory();
            var batch1 = new Batch(partType, partPrice: 100, quantity: 2);
            var batch2 = new Batch(partType, partPrice: 150, quantity: 4);

            inventory.CheckIn(batch1, warehouse);
            inventory.CheckIn(batch2, warehouse);

            PartCost cost = inventory.PreviewCheckOut(partDescription,
                                                      quantity: 5,
                                                      priceStrategy: new LifoPriceStrategy());
            cost.Total.Should().Be(700);
        }

        [TestMethod]
        public void Check_Out_Parts()
        {
            const string partType = "Part A";
            const string warehouse = "Dublin";
            var partDescription = new PartDescription(partType, warehouse);

            var inventory = new Inventory();
            var batch1 = new Batch(partType, partPrice: 100, quantity: 2);
            var batch2 = new Batch(partType, partPrice: 100, quantity: 4);

            inventory.CheckIn(batch1, warehouse);
            inventory.CheckIn(batch2, warehouse);

            inventory.CheckOut(partDescription,
                               quantity: 5,
                               priceStrategy: new LifoPriceStrategy());

            inventory.GetQuantity(partDescription).Should().Be(1);
        }

        [TestMethod]
        public void Extract_Batches_Per_Warehouse()
        {
            const string warehouse = "Dublin";

            var inventory = new Inventory();
            var batch1 = new Batch("Part A", partPrice: 100, quantity: 2);
            var batch2 = new Batch("Part B", partPrice: 135, quantity: 4);
            var batch3 = new Batch("Part A", partPrice: 200, quantity: 4);

            inventory.CheckIn(batch1, warehouse);
            inventory.CheckIn(batch2, warehouse);
            inventory.CheckIn(batch3, "Cork");

            IList<Batch> dublinBatches = inventory.GetWarehouseBatches(warehouse);

            var partABatch = dublinBatches.First();
            var partBBatch = dublinBatches.Last();

            partABatch.GetTotalBatchPrice().Should().Be(200);
            partABatch.Quantity.Should().Be(2);
            partBBatch.GetTotalBatchPrice().Should().Be(540);
            partBBatch.Quantity.Should().Be(4);

            dublinBatches.Should().HaveCount(2);
        }
    }
}