
using System;
using TES3.Core;
using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.LAND
{
    [HandlesRecord("LAND")]
    static class Operations
    {
        [HandlesOperation(RecordOperationType.AddIndex)]
        internal static object GetAddIndex(RecordAddIndexContext context)
        {
            throw new InvalidOperationException(GetStrictMessage("LAND"));
        }

        [HandlesOperation(RecordOperationType.Identifier)]
        internal static object GetIdentifier(RecordIdentifierContext context)
        {
            var data = context.record.GetSubRecord<GridSubRecord>("INTV");

            return new LandscapeGridKey(data.GridX, data.GridY);
		}
    }
}
