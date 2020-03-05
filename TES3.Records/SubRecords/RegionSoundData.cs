
using System;

namespace TES3.Records.SubRecords
{
    public class RegionSoundData : SubRecord
    {
        string soundName;
        

        public RegionSoundData(string name, string soundName, byte chance) : base(name)
        {
            SoundName = soundName;
            Chance = chance;
        }

        public byte Chance
        {
            get;
            set;
        }

        public string SoundName
        {
            get => soundName;
            set => soundName = value ?? throw new ArgumentNullException("soundNameData", "Sound Name cannot be null");
        }

        public override string ToString()
        {
            return $"{Name} {SoundName}({Chance})";
        }
    }


}