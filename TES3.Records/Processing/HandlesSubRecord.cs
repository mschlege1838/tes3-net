using System;
using System.Collections.Generic;
using System.Text;

namespace TES3.Records.Processing
{

    [AttributeUsage(AttributeTargets.Class)]
    class HandlesSubRecord : Attribute
    {

        internal HandlesSubRecord(string recordName, string subRecordName)
        {
            RecordName = recordName ?? throw new ArgumentNullException("recordName");
            SubRecordName = subRecordName ?? throw new ArgumentNullException("subRecordName");
        }

        internal string RecordName
        {
            get;
        }

        internal string SubRecordName
        {
            get;
        }

    }
}
