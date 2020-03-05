
namespace TES3.Records.SubRecords
{
    public class GridSubRecord : SubRecord
    {

        public GridSubRecord(string name, int gridX, int gridY) : base(name)
        {
            GridX = gridX;
            GridY = gridY;
        }

        public int GridX
        {
            get;
            set;
        }

        public int GridY
        {
            get;
            set;
        }

        public override string ToString()
        {
            return $"{Name} ({GridX}, {GridY})";
        }

    }
}
