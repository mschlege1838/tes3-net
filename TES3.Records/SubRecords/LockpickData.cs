namespace TES3.Records.SubRecords
{
    public class LockpickData : SubRecord
    {
        public float Weight { get; set; }
        public int Value { get; set; }
        public float Quality { get; set; }
        public int Uses { get; set; }

        public LockpickData(string name, float weight, int value, float quality, int uses) : base(name)
        {
            Weight = weight;
            Value = value;
            Quality = quality;
            Uses = uses;
        }

        public override string ToString()
        {
            return $"{Name} wght,val,qlty,val({Weight}, {Value}, {Quality}, {Uses})";
        }
    }


}