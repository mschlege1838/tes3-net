using TES3.Util;

namespace TES3.Core
{

    public class TES3VertexNormal : ICopyable<TES3VertexNormal>
    {

        public TES3VertexNormal(sbyte x, sbyte y, sbyte z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public sbyte X
        {
            get;
            set;
        }

        public sbyte Y
        {
            get;
            set;
        }

        public sbyte Z
        {
            get;
            set;
        }

        public TES3VertexNormal Copy()
        {
            return new TES3VertexNormal(X, Y, Z);
        }

        public override string ToString()
        {
            return $"({X}, {Y}, {Z})";
        }
    }
}
