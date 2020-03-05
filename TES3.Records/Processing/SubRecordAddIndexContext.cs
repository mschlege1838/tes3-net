

namespace TES3.Records.Processing
{
    class SubRecordAddIndexContext : ISubRecordOperationContext
    {
        internal readonly Record record;
        internal readonly object additionalContext;
        
        internal SubRecordAddIndexContext(Record record, string subRecordName, object additionalContext)
        {
            this.record = record;
            this.additionalContext = additionalContext;
            SubRecordName = subRecordName;
        }

        public string RecordName
        {
            get => record.Name;
        }

        public string SubRecordName
        {
            get;
        }
    }
}
