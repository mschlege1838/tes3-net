namespace TES3.Records.SubRecords
{
    public class MagicEffectData : SubRecord
    {
        public MagicEffectData(string name, int spellSchool, float baseCost, int flags, int red,
                int blue, int green, float speedX, float sizeX, float sizeCap) : base(name)
        {
            SpellSchool = spellSchool;
            BaseCost = baseCost;
            Flags = flags;
            Red = red;
            Blue = blue;
            Green = green;
            SpeedX = speedX;
            SizeX = sizeX;
            SizeCap = sizeCap;
        }

        public int SpellSchool
        {
            get;
            set;
        }

        public float BaseCost
        {
            get;
            set;
        }

        public int Flags
        {
            get;
            set;
        }

        public int Red
        {
            get;
            set;
        }

        public int Blue
        {
            get;
            set;
        }

        public int Green
        {
            get;
            set;
        }

        public float SpeedX
        {
            get;
            set;
        }

        public float SizeX
        {
            get;
            set;
        }

        public float SizeCap
        {
            get;
            set;
        }

        public override string ToString()
        {
            return $"{Name} school,cost({SpellSchool}, {BaseCost}) rgb({Red}, {Green}, {Blue}) speedx,sizex,sizecap({SpeedX}, {SizeX}, {SizeCap}) flags({Flags:X8})";
        }
    }


}