using TES3.Util;

namespace TES3.Core
{
    public class ColorRef : ICopyable<ColorRef>
    {
        

        public ColorRef(byte red, byte green, byte blue, byte alpha)
        {
            Red = red;
            Green = green;
            Blue = blue;
            Alpha = alpha;
        }

        public byte Red
        {
            get;
            set;
        }

        public byte Green
        {
            get;
            set;
        }

        public byte Blue
        {
            get;
            set;
        }

        public byte Alpha
        {
            get;
            set;
        }


        public ColorRef Copy()
        {
            return new ColorRef(Red, Green, Blue, Alpha);
        }


        public override string ToString()
        {
            return $"rgba({Red}, {Green}, {Blue}, {Alpha})";
        }
    }

}