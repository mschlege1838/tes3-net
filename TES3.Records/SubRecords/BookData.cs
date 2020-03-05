namespace TES3.Records.SubRecords
{
    public class BookData : SubRecord
    {
        public float Weight { get; set; }
        public int Value { get; set; }
        public int Scroll { get; set; }
        public int SkillId { get; set; }
        public int EnchantPoints { get; set; }

        public BookData(string name, float weight, int value, int scroll, int skillId, int enchantPoints) : base(name)
        {
            Weight = weight;
            Value = value;
            Scroll = scroll;
            SkillId = skillId;
            EnchantPoints = enchantPoints;
        }

        public override string ToString()
        {
            return $"{Name} (wght, val, scrl, skl, ench)({Weight}, {Value}, {Scroll}, {SkillId}, {EnchantPoints})";
        }
    }


}