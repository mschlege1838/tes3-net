using TES3.Util;

namespace TES3.Core
{
    public class TES3VertexColor : ICopyable<TES3VertexColor>
    {

        public TES3VertexColor(byte red, byte green, byte blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
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

        public TES3VertexColor Copy()
        {
            return new TES3VertexColor(Red, Green, Blue);
        }

        public override string ToString()
        {
            return $"rgb({Red}, {Green}, {Blue})";
        }
    }
}
