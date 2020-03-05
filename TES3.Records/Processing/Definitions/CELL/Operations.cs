
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.CELL
{
    [HandlesRecord("CELL")]
    static class Operations
    {
        [HandlesOperation(RecordOperationType.AddIndex)]
        internal static object GetAddIndex(RecordAddIndexContext context)
        {
            var index = GetAddIndexStrict(context.modFile, RecordNames.CellRecordOrder);
            if (index != -1)
            {
                return index + 1;
            }

            index = GetAddIndexUnordered(context.modFile, RecordNames.GameItemRecords);
            return index == -1 ? GetAddIndexFirst(context.modFile, RecordNames.InitialRecordOrder) : -1;
        }

        [HandlesOperation(RecordOperationType.Identifier)]
        internal static object GetIdentifier(RecordIdentifierContext context)
        {
            if (CellUtils.IsInterior(context.record))
            {
                return CellUtils.GetName(context.record);
            }
            else
            {
                return CellUtils.GetGridKey(context.record);
            }
		}
    }
}
