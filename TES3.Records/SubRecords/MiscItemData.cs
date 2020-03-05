namespace TES3.Records.SubRecords
{
    public class MiscItemData : SubRecord
    {
        public float Weight { get; set; }
        public int Value { get; set; }
        public int Unknown { get; set; }

        public MiscItemData(string name, float weight, int value, int unknown) : base(name)
        {
            Weight = weight;
            Value = value;
            Unknown = unknown;
        }

        public override string ToString()
        {
            return $"{Name} wgt,val({Weight}, {Value})";
        }
    }


}