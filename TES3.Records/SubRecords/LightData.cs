
using System;
using TES3.Core;

namespace TES3.Records.SubRecords
{
    public class LightData : SubRecord
    {
        public float Weight { get; set; }
        public int Value { get; set; }
        public int Time { get; set; }
        public int Radius { get; set; }
        ColorRef color;
        public int Flags { get; set; }

        public LightData(string name, float weight, int value, int time, int radius, ColorRef color, int flags) : base(name)
        {
            Weight = weight;
            Value = value;
            Time = time;
            Radius = radius;
            Color = color;
            Flags = flags;
        }

        public ColorRef Color
        {
            get => color;
            set => color = value ?? throw new ArgumentNullException("color", "Color cannot be null.");
        }

        public override string ToString()
        {
            return $"{Name} wght,val,rad,time({Weight}, {Value}, {Radius}, {Time}) {Color} flags({Flags:X8})";
        }

    }


}