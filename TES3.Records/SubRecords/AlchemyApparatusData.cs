namespace TES3.Records.SubRecords
{
    public class AlchemyApparatusData : SubRecord
    {
        public int Type { get; set; }
        public float Quality { get; set; }
        public float Weight { get; set; }
        public int Value { get; set; }

        public AlchemyApparatusData(string name, int type, float quality, float weight, int value) : base(name)
        {
            Type = type;
            Quality = quality;
            Weight = weight;
            Value = value;
        }

        public override string ToString()
        {
            return $"{Name} (type, qual, wght, val)({Type}, {Quality}, {Weight}, {Value})";
        }
    }


}