namespace WarehousesInventory
{
    public class PartDescription
    {
        public string PartType { get; private set; }
        public string Warehouse { get; private set; }

        public PartDescription(string partType, string warehouse)
        {
            PartType = partType;
            Warehouse = warehouse;
        }
    }
}