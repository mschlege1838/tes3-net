
using System;

using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.CREA.XSCL 
{
	
	[HandlesSubRecord("CREA", "XSCL")]
    static class Operations
	{
		[HandlesOperation(SubRecordOperationType.Read)]
		internal static object Read(SubRecordReadContext context)
		{
            return new FloatSubRecord(context.SubRecordName, BitConverter.ToSingle(context.buf, 0));
		}
		
		[HandlesOperation(SubRecordOperationType.Size)]
		internal static object GetSize(SubRecordLengthContext context)
		{
            return 4;
		}
		
		[HandlesOperation(SubRecordOperationType.Write)]
		internal static object Write(SubRecordWriteContext context)
		{
            WriteBytes(context.stream, BitConverter.GetBytes(((FloatSubRecord) context.subRecord).Data));
			return null;
		}
		
		[HandlesOperation(SubRecordOperationType.AddIndex)]
		internal static object GetAddIndex(SubRecordAddIndexContext context)
		{
            return GetAddIndexOrdered(context.record, "XSCL", Names.InitalSubRecordOrder);
		}
		
	}
	
}
