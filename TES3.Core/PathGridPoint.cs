
using TES3.Util;

namespace TES3.Core
{
    public class PathGridPoint : ICopyable<PathGridPoint>
    {

        public PathGridPoint(int x, int y, int z, bool generated)
        {
            X = x;
            Y = y;
            Z = z;
            Generated = generated;
        }
        
        public int X
        {
            get;
            set;
        }

        public int Y
        {
            get;
            set;
        }

        public int Z
        {
            get;
            set;
        }

        public bool Generated
        {
            get;
            set;
        }

        public override string ToString()
        {
            return $"({X}, {Y}, {Z}/{Generated})";
        }

        public PathGridPoint Copy()
        {
            return new PathGridPoint(X, Y, Z, Generated);
        }
    }

}
