namespace TES3.Records.SubRecords
{
    public class ClothingData : SubRecord
    {
        public int Type { get; set; }
        public float Weight { get; set; }
        public short Value { get; set; }
        public short EnchantPoints { get; set; }

        public ClothingData(string name, int type, float weight, short value, short enchantPoints) : base(name)
        {
            Type = type;
            Weight = weight;
            Value = value;
            EnchantPoints = enchantPoints;
        }

        public override string ToString()
        {
            return $"{Name} (typ, wght, val, ench)({Type}, {Weight}, {Value}, {EnchantPoints})";
        }
    }


}