
using System;

using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.CELL.UNAM 
{
	
	[HandlesSubRecord("CELL", "UNAM")]
    static class Operations
	{
		[HandlesOperation(SubRecordOperationType.Read)]
		internal static object Read(SubRecordReadContext context)
		{
            return new ByteSubRecord(context.SubRecordName, context.buf[0]);
		}
		
		[HandlesOperation(SubRecordOperationType.Size)]
		internal static object GetSize(SubRecordLengthContext context)
		{
            return 1;
		}
		
		[HandlesOperation(SubRecordOperationType.Write)]
		internal static object Write(SubRecordWriteContext context)
		{
            context.stream.WriteByte(((ByteSubRecord) context.subRecord).Data);
			return null;
		}
		
		[HandlesOperation(SubRecordOperationType.AddIndex)]
		internal static object GetAddIndex(SubRecordAddIndexContext context)
		{
            throw new InvalidOperationException(GetStrictMessage("UNAM"));
		}
		
	}
	
}
