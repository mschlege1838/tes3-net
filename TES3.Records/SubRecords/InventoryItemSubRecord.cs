namespace TES3.Records.SubRecords
{
    public class InventoryItemSubRecord : SubRecord
    {
        public int Count { get; set; }
        public string ItemName { get; set; }

        public InventoryItemSubRecord(string name, int count, string itemName) : base(name)
        {
            Count = count;
            ItemName = itemName;
        }

        public override string ToString()
        {
            return $"{Name} itm,ct({ItemName}, {Count})";
        }
    }


}