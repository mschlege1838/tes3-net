namespace TES3.Records.SubRecords
{
    public class WeaponData : SubRecord
    {
        public float Weight { get; set; }
        public int Value { get; set; }
        public short Type { get; set; }
        public short Health { get; set; }
        public float Speed { get; set; }
        public float Reach { get; set; }
        public short EnchantPoints { get; set; }
        public byte ChopMin { get; set; }
        public byte ChopMax { get; set; }
        public byte SlashMin { get; set; }
        public byte SlashMax { get; set; }
        public byte ThrustMin { get; set; }
        public byte ThrustMax { get; set; }
        public int Flags { get; set; }

        public WeaponData(string name, float weight, int value, short type, short health, float speed,
                float reach, short enchantPoints, byte chopMin, byte chopMax, byte slashMin, byte slashMax,
                byte thrustMin, byte thrustMax, int flags) : base(name)
        {
            Weight = weight;
            Value = value;
            Type = type;
            Health = health;
            Speed = speed;
            Reach = reach;
            EnchantPoints = enchantPoints;
            ChopMin = chopMin;
            ChopMax = chopMax;
            SlashMin = slashMin;
            SlashMax = slashMax;
            ThrustMin = thrustMin;
            ThrustMax = thrustMax;
            Flags = flags;
        }

        public override string ToString()
        {
            return $"{Name} wgt,val,typ,hlth,spd,rch,ench({Weight}, {Value}, {Type}, {Health}, {Speed}, {Reach}, {EnchantPoints}) " +
                $"chp,slsh,thrst({ChopMin} - {ChopMax}, {SlashMin} - {SlashMax}, {ThrustMin} - {ThrustMax}) flags({Flags:X8})";
        }
    }


}