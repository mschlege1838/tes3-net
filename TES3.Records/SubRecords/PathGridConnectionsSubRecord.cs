using System;
using System.Collections.Generic;
using System.Text;

namespace TES3.Records.SubRecords
{
    public class PathGridConnectionSubRecord : SubRecord
    {
        IList<int> connections;

        public PathGridConnectionSubRecord(string name, IList<int> connections) : base(name)
        {
            Connections = connections;
        }

        public IList<int> Connections
        {
            get => connections;
            set => connections = value ?? throw new ArgumentNullException("value", "Connections cannot be null.");
        }

        public override string ToString()
        {
            return $"{Name} ({Connections.Count} connections)";
        }
    }
}
