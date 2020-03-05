
using TES3.Core;
using TES3.Util;

namespace TES3.Records.SubRecords
{
    public class VertexNormalSubRecord : SubRecord
    {

        public const int MAPPING_SIDE_LENGTH = 65;

        TES3VertexNormal[,] normalVectorMapping;

        public VertexNormalSubRecord(string name, TES3VertexNormal[,] mapping) : base(name)
        {
            NormalVectorMapping = mapping;
        }

        public TES3VertexNormal[,] NormalVectorMapping
        {
            get => normalVectorMapping;
            set => normalVectorMapping = Validation.ValidateSquare(value, MAPPING_SIDE_LENGTH, "value", "Normal Vector Mapping");
        }
    }
}
