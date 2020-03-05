
using System;
using TES3.Core;
using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.PGRD
{
    [HandlesRecord("PGRD")]
    static class Operations
    {
        [HandlesOperation(RecordOperationType.AddIndex)]
        internal static object GetAddIndex(RecordAddIndexContext context)
        {
            throw new InvalidOperationException(GetStrictMessage("PGRD"));
        }

        [HandlesOperation(RecordOperationType.Identifier)]
        internal static object GetIdentifier(RecordIdentifierContext context)
        {
            var data = context.record.GetSubRecord<PathGridDataSubRecord>("DATA");

            return new PathGridKey(context.record.GetSubRecord<StringSubRecord>("NAME").Data, new GridKey(data.GridX, data.GridY));
		}
    }
}
