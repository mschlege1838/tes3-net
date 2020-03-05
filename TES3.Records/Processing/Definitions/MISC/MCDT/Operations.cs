
using System;

using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.MISC.MCDT 
{
	
	[HandlesSubRecord("MISC", "MCDT")]
    static class Operations
	{
		[HandlesOperation(SubRecordOperationType.Read)]
		internal static object Read(SubRecordReadContext context)
		{
        
            var pos = 0;

            var weight = BitConverter.ToSingle(context.buf, pos); pos += 4;
            var value = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var unknown = BitConverter.ToInt32(context.buf, pos); pos += 4;

            return new MiscItemData(context.SubRecordName, weight, value, unknown);
		}
		
		[HandlesOperation(SubRecordOperationType.Size)]
		internal static object GetSize(SubRecordLengthContext context)
		{
            return 12;
		}
		
		[HandlesOperation(SubRecordOperationType.Write)]
		internal static object Write(SubRecordWriteContext context)
		{
        
            var miscItemData = (MiscItemData) context.subRecord;

            WriteBytes(context.stream, BitConverter.GetBytes(miscItemData.Weight));
            WriteBytes(context.stream, BitConverter.GetBytes(miscItemData.Value));
            WriteBytes(context.stream, BitConverter.GetBytes(miscItemData.Unknown));
			return null;
		}
		
		[HandlesOperation(SubRecordOperationType.AddIndex)]
		internal static object GetAddIndex(SubRecordAddIndexContext context)
		{
			return GetAddIndexOrdered(context.record, "MCDT", Names.SubRecordOrder);
		}
		
	}
	
}
