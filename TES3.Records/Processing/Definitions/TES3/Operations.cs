
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.TES3
{
    [HandlesRecord("TES3")]
    static class Operations
    {
        [HandlesOperation(RecordOperationType.AddIndex)]
        internal static object GetAddIndex(RecordAddIndexContext context)
        {
            return GetAddIndexOrdered(context.modFile, "TES3", RecordNames.InitialRecordOrder);
        }

        [HandlesOperation(RecordOperationType.Identifier)]
        internal static object GetIdentifier(RecordIdentifierContext context)
        {
            return Record.TES3HeaderIdentifier;
		}
    }
}
