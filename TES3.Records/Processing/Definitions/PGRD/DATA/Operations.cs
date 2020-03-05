
using System;

using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.PGRD.DATA 
{
	
	[HandlesSubRecord("PGRD", "DATA")]
    static class Operations
	{
		[HandlesOperation(SubRecordOperationType.Read)]
		internal static object Read(SubRecordReadContext context)
		{
        
            var pos = 0;

            var gridX = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var gridY = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var granularity = BitConverter.ToInt16(context.buf, pos); pos += 2;
            var pointCount = BitConverter.ToInt16(context.buf, pos); pos += 2;


            return new PathGridDataSubRecord(context.SubRecordName, gridX, gridY, granularity, pointCount);
		}
		
		[HandlesOperation(SubRecordOperationType.Size)]
		internal static object GetSize(SubRecordLengthContext context)
		{
            return 12;
		}
		
		[HandlesOperation(SubRecordOperationType.Write)]
		internal static object Write(SubRecordWriteContext context)
		{
        
            var pathGridData = (PathGridDataSubRecord) context.subRecord;

            WriteBytes(context.stream, BitConverter.GetBytes(pathGridData.GridX));
            WriteBytes(context.stream, BitConverter.GetBytes(pathGridData.GridY));
            WriteBytes(context.stream, BitConverter.GetBytes(pathGridData.Granularity));
            WriteBytes(context.stream, BitConverter.GetBytes(pathGridData.PointCount));
			return null;
		}
		
		[HandlesOperation(SubRecordOperationType.AddIndex)]
		internal static object GetAddIndex(SubRecordAddIndexContext context)
		{
			return GetAddIndexOrdered(context.record, "DATA", Names.SubRecordOrder);
		}
		
	}
	
}
