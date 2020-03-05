

using TES3.Core;

namespace TES3.Records.SubRecords
{
    public class PathGridDataSubRecord : SubRecord
    {
        public PathGridDataSubRecord(string name, int gridX, int gridY, short granularity, short pointCount) : base(name)
        {
            GridX = gridX;
            GridY = gridY;
            Granularity = granularity;
            PointCount = pointCount;
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

        public short Granularity
        {
            get;
            set;
        }

        public short PointCount
        {
            get;
            set;
        }

        public override string ToString()
        {
            return $"{Name} ({GridX}, {GridY}) grnlty,pt_ct({Granularity}, {PointCount})";
        }

    }
}
