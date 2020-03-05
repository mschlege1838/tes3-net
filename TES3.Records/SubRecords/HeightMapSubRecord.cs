using TES3.Util;

namespace TES3.Records.SubRecords
{
    public class HeightMapSubRecord : SubRecord
    {
        public const int MAPPING_SIDE_LENGTH = 65;

        sbyte[,] slopeMapping;

        public HeightMapSubRecord(string name, float baseHeight, sbyte[,] slopeMapping, byte[] unknown) : base(name)
        {
            BaseHeight = baseHeight;
            SlopeMapping = slopeMapping;
            Unknown = unknown;
        }

        public float BaseHeight
        {
            get;
            set;
        }

        public sbyte[,] SlopeMapping
        {
            get => slopeMapping;
            set => slopeMapping = Validation.ValidateSquare(value, MAPPING_SIDE_LENGTH, "value", "Slope Mapping");
        }

        public byte[] Unknown
        {
            get;
            set;
        }

    }
}
