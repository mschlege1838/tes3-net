using System.IO;


namespace TES3.Records.Processing
{
    class SubRecordReadContext : ISubRecordOperationContext
    {
        internal readonly Stream stream;
        internal SubRecordHeader header;
        internal byte[] buf;

        internal SubRecordReadContext(string recordName, Stream stream)
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
            get => header.Name;
        }

    }
}
