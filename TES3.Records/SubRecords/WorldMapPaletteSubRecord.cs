
using TES3.Util;

namespace TES3.Records.SubRecords
{
    public class WorldMapPaletteSubRecord : SubRecord
    {
        public const int PALETTE_SIDE_LENGTH = 9;

        byte[,] palette;

        public WorldMapPaletteSubRecord(string name, byte[,] palette) : base(name)
        {
            Palette = palette;
        }

        public byte[,] Palette
        {
            get => palette;
            set => palette = Validation.ValidateSquare(value, PALETTE_SIDE_LENGTH, "value", "Palette");
        }
    }
}
