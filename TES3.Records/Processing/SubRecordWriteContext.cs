using System.IO;


namespace TES3.Records.Processing
{
    class SubRecordWriteContext : ISubRecordOperationContext
    {

        internal readonly Stream stream;
        internal SubRecord subRecord;

        internal SubRecordWriteContext(string recordName, Stream stream)
        {
            RecordName = recordName;
            this.stream = stream;
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
