
using System;

using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.CELL.INTV 
{
	
	[HandlesSubRecord("CELL", "INTV")]
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
            return 4;
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
            throw new InvalidOperationException(GetStrictMessage("INTV"));
		}
		
	}
	
}
