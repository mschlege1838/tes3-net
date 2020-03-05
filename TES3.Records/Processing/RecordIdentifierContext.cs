using System;
using System.Collections.Generic;
using System.Text;

namespace TES3.Records.Processing
{
    class RecordIdentifierContext : IRecordOperationContext
    {
        internal readonly Record record;

        internal RecordIdentifierContext(Record record)
        {
            this.record = record;
        }

        public string RecordName
        {
            get => record.Name;
        }
    }
}
