
using System;
using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.INFO
{
    [HandlesRecord("INFO")]
    static class Operations
    {
        [HandlesOperation(RecordOperationType.AddIndex)]
        internal static object GetAddIndex(RecordAddIndexContext context)
        {
            throw new InvalidOperationException(GetStrictMessage("INFO"));
        }

        [HandlesOperation(RecordOperationType.Identifier)]
        internal static object GetIdentifier(RecordIdentifierContext context)
        {
			return context.record.GetSubRecord<StringSubRecord>("INAM").Data;
		}
    }
}
