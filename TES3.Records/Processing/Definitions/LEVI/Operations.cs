
using TES3.Records.SubRecords;

namespace TES3.Records.Processing.Definitions.LEVI
{
    [HandlesRecord("LEVI")]
    static class Operations
    {
        [HandlesOperation(RecordOperationType.AddIndex)]
        internal static object GetAddIndex(RecordAddIndexContext context)
        {
            return RecordNames.GetAddIndexGameItem(context);
        }

        [HandlesOperation(RecordOperationType.Identifier)]
        internal static object GetIdentifier(RecordIdentifierContext context)
        {
			return context.record.GetSubRecord<StringSubRecord>("NAME").Data;
		}
    }
}
