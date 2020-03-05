namespace TES3.Records.SubRecords
{
    public class PotionData : SubRecord
    {
        public float Weight { get; set; }
        public int Value { get; set; }
        public int AutoCalc { get; set; }

        public PotionData(string name, float weight, int value, int autoCalc) : base(name)
        {
            Weight = weight;
            Value = value;
            AutoCalc = autoCalc;
        }

        public override string ToString()
        {
            return $"{Name} wght,val,autocalc({Weight}, {Value}, {AutoCalc})";
        }
    }

}