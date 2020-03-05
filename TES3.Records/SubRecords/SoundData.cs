namespace TES3.Records.SubRecords
{
    public class SoundData : SubRecord
    {
        public byte Volume { get; set; }
        public byte MinRange { get; set; }
        public byte MaxRange { get; set; }

        public SoundData(string name, byte volume, byte minRange, byte maxRange) : base(name)
        {
            Volume = volume;
            MinRange = minRange;
            MaxRange = maxRange;
        }

        public override string ToString()
        {
            return $"{Name} vol,min,max({Volume}, {MinRange}, {MaxRange})";
        }
    }


}