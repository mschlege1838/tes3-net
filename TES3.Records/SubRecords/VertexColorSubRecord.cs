
using TES3.Core;
using TES3.Util;

namespace TES3.Records.SubRecords
{
    public class VertexColorSubRecord : SubRecord
    {

        public const int COLOR_MAPPING_SIDE_LENGTH = 65;

        TES3VertexColor[,] colorMapping;

        public VertexColorSubRecord(string name, TES3VertexColor[,] colorMapping) : base(name)
        {
            ColorMapping = colorMapping;
        }

        public TES3VertexColor[,] ColorMapping
        {
            get => colorMapping;
            set => colorMapping = Validation.ValidateSquare(value, COLOR_MAPPING_SIDE_LENGTH, "value", "Color Mapping");
        }
    }
}
