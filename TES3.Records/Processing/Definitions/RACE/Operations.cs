
using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.RACE
{
    [HandlesRecord("RACE")]
    static class Operations
    {
        [HandlesOperation(RecordOperationType.AddIndex)]
        internal static object GetAddIndex(RecordAddIndexContext context)
        {
            return GetAddIndexOrdered(context.modFile, "RACE", RecordNames.InitialRecordOrder);
        }

        [HandlesOperation(RecordOperationType.Identifier)]
        internal static object GetIdentifier(RecordIdentifierContext context)
        {
			return context.record.GetSubRecord<StringSubRecord>("NAME").Data;
		}
    }
}
