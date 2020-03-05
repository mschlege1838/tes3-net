using System;

namespace TES3.GameItem
{

    [AttributeUsage(AttributeTargets.Class)]
    class TargetRecord : Attribute
    {
        internal TargetRecord(string recordName)
        {
            RecordName = recordName ?? throw new ArgumentNullException("recordName");
        }

        internal string RecordName
        {
            get;
        }
    }
}
