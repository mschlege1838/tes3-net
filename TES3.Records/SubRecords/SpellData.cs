namespace TES3.Records.SubRecords
{
    public class SpellData : SubRecord
    {
        public int Type { get; set; }
        public int SpellCost { get; set; }
        public int Flags { get; set; }

        public SpellData(string name, int type, int spellCost, int flags) : base(name)
        {
            Type = type;
            SpellCost = spellCost;
            Flags = flags;
        }

        public override string ToString()
        {
            return $"{Name} typ,cost({Type}, {SpellCost}) flags({Flags:X8})";
        }
    }


}