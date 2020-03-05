
using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.DIAL
{
    [HandlesRecord("DIAL")]
    static class Operations
    {
        [HandlesOperation(RecordOperationType.AddIndex)]
        internal static object GetAddIndex(RecordAddIndexContext context)
        {
            var index = GetAddIndexStrict(context.modFile, RecordNames.DialogueRecordOrder);
            if (index != -1)
            {
                return index + 1;
            }

            index = context.modFile.GetLastIndex("SNDG");
            if (index != -1)
            {
                return index + 1;
            }

            index = GetAddIndexStrict(context.modFile, RecordNames.CellRecordOrder);
            if (index != -1)
            {
                return index + 1;
            }

            index = GetAddIndexUnordered(context.modFile, RecordNames.GameItemRecords);
            return index == -1 ? GetAddIndexFirst(context.modFile, RecordNames.InitialRecordOrder) : index + 1;
        }

        [HandlesOperation(RecordOperationType.Identifier)]
        internal static object GetIdentifier(RecordIdentifierContext context)
        {
			return context.record.GetSubRecord<StringSubRecord>("NAME").Data;
		}
    }
}
