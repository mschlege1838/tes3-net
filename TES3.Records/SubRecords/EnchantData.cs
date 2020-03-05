namespace TES3.Records.SubRecords
{
    public class EnchantData : SubRecord
    {
        public int Type { get; set; }
        public int Cost { get; set; }
        public int Charge { get; set; }
        public int AutoCalc { get; set; }

        public EnchantData(string name, int type, int cost, int charge, int autoCalc) : base(name)
        {
            Type = type;
            Cost = cost;
            Charge = charge;
            AutoCalc = autoCalc;
        }

        public override string ToString()
        {
            return $"{Name} (typ, cst, chrg, auto)({Type}, {Cost}, {Charge}, {AutoCalc})";
        }
    }

}