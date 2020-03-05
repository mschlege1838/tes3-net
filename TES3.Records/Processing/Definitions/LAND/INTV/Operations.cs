
using System;

using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.LAND.INTV 
{
	
	[HandlesSubRecord("LAND", "INTV")]
    static class Operations
	{
		[HandlesOperation(SubRecordOperationType.Read)]
		internal static object Read(SubRecordReadContext context)
		{
        
            var pos = 0;

            var gridX = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var gridY = BitConverter.ToInt32(context.buf, pos); pos += 4;

            return new GridSubRecord(context.SubRecordName, gridX, gridY);
		}
		
		[HandlesOperation(SubRecordOperationType.Size)]
		internal static object GetSize(SubRecordLengthContext context)
		{
            return 8;
		}
		
		[HandlesOperation(SubRecordOperationType.Write)]
		internal static object Write(SubRecordWriteContext context)
		{
        
            var gridData = (GridSubRecord) context.subRecord;

            WriteBytes(context.stream, BitConverter.GetBytes(gridData.GridX));
            WriteBytes(context.stream, BitConverter.GetBytes(gridData.GridY));
			return null;
		}
		
		[HandlesOperation(SubRecordOperationType.AddIndex)]
		internal static object GetAddIndex(SubRecordAddIndexContext context)
		{
			return GetAddIndexOrdered(context.record, "INTV", Names.SubRecordOrder);
		}
		
	}
	
}
