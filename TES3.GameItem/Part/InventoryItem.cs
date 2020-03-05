using TES3.Util;

namespace TES3.GameItem.Part
{
    public class InventoryItem : ICopyable<InventoryItem>
    {
        public InventoryItem(string name, int count)
        {
            Name = name;
            Count = count;
        }

        public string Name
        {
            get;
            set;
        }

        public int Count
        {
            get;
            set;
        }

        public InventoryItem Copy()
        {
            return new InventoryItem(Name, Count);
        }
    }


}
