
using System;

using TES3.Core;

namespace TES3.Records.SubRecords
{
    public class LandTextureSubRecord : SubRecord
    {

        LandTextureMap data;

        public LandTextureSubRecord(string name, LandTextureMap data) : base(name)
        {
            Data = data;
        }

        public LandTextureMap Data
        {
            get => data;
            set => data = value ?? throw new ArgumentNullException("value", "Data cannot be null");
        }

    }
}
