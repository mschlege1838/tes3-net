using System;

namespace TES3.Records.Processing
{

    [AttributeUsage(AttributeTargets.Class)]
    class HandlesRecord : Attribute
    {

        internal HandlesRecord(string recordName)
        {
            RecordName = recordName ?? throw new ArgumentNullException("recordName");
        }

        internal string RecordName
        {
            get;
        }

    }
}
