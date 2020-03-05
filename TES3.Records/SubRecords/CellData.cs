using TES3.Core;

namespace TES3.Records.SubRecords
{
    public class CellData : SubRecord
    {
        public int Flags { get; set; }
        public int GridX { get; set; }
        public int GridY { get; set; }

        public CellData(string name, int flags, int gridX, int gridY) : base(name)
        {
            Flags = flags;
            GridX = gridX;
            GridY = gridY;
        }

        

        public override string ToString()
        {
            return $"{Name} ({GridX}, {GridY}) {Flags:X8}";
        }
    }


}