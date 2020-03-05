namespace TES3.Records.SubRecords
{
    public class ArmorData : SubRecord
    {
        public int Type { get; set; }
        public float Weight { get; set; }
        public int Value { get; set; }
        public int Health { get; set; }
        public int EnchantPoints { get; set; }
        public int Armor { get; set; }

        public ArmorData(string name, int type, float weight, int value, int health, int enchantPoints,
                int armor) : base(name)
        {
            Type = type;
            Weight = weight;
            Value = value;
            Health = health;
            EnchantPoints = enchantPoints;
            Armor = armor;
        }

        public override string ToString()
        {
            return $"{Name} (typ, wght, val, hlth, ench, ratng)({Type}, {Weight}, {Value}, {Health}, {EnchantPoints}, {Armor})";
        }
    }


}