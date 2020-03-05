
using System;

using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.SCPT.SCDT 
{
	
	[HandlesSubRecord("SCPT", "SCDT")]
    static class Operations
	{
		[HandlesOperation(SubRecordOperationType.Read)]
		internal static object Read(SubRecordReadContext context)
		{
        
            var data = new byte[context.header.Size];
            Array.Copy(context.buf, data, context.header.Size);
            return new GenericSubRecord(context.SubRecordName, data, true);
		}
		
		[HandlesOperation(SubRecordOperationType.Size)]
		internal static object GetSize(SubRecordLengthContext context)
		{
            return ((GenericSubRecord) context.subRecord).Data.Length;
		}
		
		[HandlesOperation(SubRecordOperationType.Write)]
		internal static object Write(SubRecordWriteContext context)
		{
            WriteBytes(context.stream, ((GenericSubRecord) context.subRecord).Data);
			return null;
		}
		
		[HandlesOperation(SubRecordOperationType.AddIndex)]
		internal static object GetAddIndex(SubRecordAddIndexContext context)
		{
			return GetAddIndexOrdered(context.record, "SCDT", Names.SubRecordOrder);
		}
		
	}
	
}
