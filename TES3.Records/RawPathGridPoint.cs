
using TES3.Core;

namespace TES3.Records
{
    public class RawPathGridPoint : PathGridPoint
    {
        public RawPathGridPoint(int x, int y, int z, bool generated, byte connectionCount, short unknown) : base(x, y, z, generated)
        {
            ConnectionCount = connectionCount;
            Unknown = unknown;
        }

        public byte ConnectionCount
        {
            get;
            set;
        }

        public short Unknown
        {
            get;
            set;
        }

    }
}
