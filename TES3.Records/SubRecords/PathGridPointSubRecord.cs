using System;
using System.Collections.Generic;

using TES3.Core;

namespace TES3.Records.SubRecords
{
    public class PathGridPointSubRecord : SubRecord
    {

        IList<RawPathGridPoint> points;

        public PathGridPointSubRecord(string name, IList<RawPathGridPoint> points) : base(name)
        {
            Points = points;
        }

        public IList<RawPathGridPoint> Points
        {
            get => points;
            set => points = value ?? throw new ArgumentNullException("value", "Points cannot be null.");
        }

        public override string ToString()
        {
            return $"{Name} ({Points.Count} points)";
        }
    }
}
