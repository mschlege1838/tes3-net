namespace TES3.Records.SubRecords
{
    public class WeatherData : SubRecord
    {

        public WeatherData(string name, byte clear, byte cloudy, byte foggy, byte overcast, byte rain,
                byte thunder, byte ash, byte blight) : base(name)
        {
            Clear = clear;
            Cloudy = cloudy;
            Foggy = foggy;
            Overcast = overcast;
            Rain = rain;
            Thunder = thunder;
            Ash = ash;
            Blight = blight;
        }

        

        public bool IsExpansion
        {
            get;
            private set;
        }

        public byte Clear
        {
            get;
            set;
        }

        public byte Cloudy
        {
            get;
            set;
        }

        public byte Foggy
        {
            get;
            set;
        }

        public byte Overcast
        {
            get;
            set;
        }

        public byte Rain
        {
            get;
            set;
        }

        public byte Thunder
        {
            get;
            set;
        }

        public byte Ash
        {
            get;
            set;
        }

        public byte Blight
        {
            get;
            set;
        }

        public override string ToString()
        {
            return $"{Name} clear,cldy,fog,ovrcst,rain,thund,ash,blgt({Clear}, {Cloudy}, {Foggy}, {Overcast}, {Rain}, {Thunder}, {Ash}, {Blight})";
        }
    }

    public class ExpansionWeatherData : WeatherData
    {
        public ExpansionWeatherData(string name, byte clear, byte cloudy, byte foggy, byte overcast, byte rain,
                byte thunder, byte ash, byte blight, byte snow, byte blizzard) : base(name, clear, cloudy, foggy, overcast, rain, thunder, ash, blight)
        {
            Snow = snow;
            Blizzard = blizzard;
        }

        public byte Snow
        {
            get;
            set;
        }

        public byte Blizzard
        {
            get;
            set;
        }

        public override string ToString()
        {
            return $"{Name} clear,cldy,fog,ovrcst,rain,thund,ash,blgt,snw,bliz({Clear}, {Cloudy}, {Foggy}, {Overcast}, {Rain}, {Thunder}, {Ash}, {Blight}, {Snow}, {Blizzard})";
        }
    }


}