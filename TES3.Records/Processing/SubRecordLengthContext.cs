namespace TES3.Records.Processing
{
    class SubRecordLengthContext : ISubRecordOperationContext
    {

        internal SubRecord subRecord;

        internal SubRecordLengthContext(string recordName)
        {
            RecordName = recordName;
        }

        public string RecordName
        {
            get;
        }

        public string SubRecordName
        {
            get => subRecord.Name;
        }
    }
}
