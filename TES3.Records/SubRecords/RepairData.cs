namespace TES3.Records.SubRecords
{
    public class RepairData : SubRecord
    {
        public float Weight { get; set; }
        public int Value { get; set; }
        public int Uses { get; set; }
        public float Quality { get; set; }

        public RepairData(string name, float weight, int value, int uses, float quality) : base(name)
        {
            Weight = weight;
            Value = value;
            Uses = uses;
            Quality = quality;
        }

        public override string ToString()
        {
            return $"{Name} wght,val,use,qlty({Weight}, {Value}, {Uses}, {Quality})";
        }
    }


}