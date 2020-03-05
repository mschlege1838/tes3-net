
using System;

using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.DIAL.DATA 
{
	
	[HandlesSubRecord("DIAL", "DATA")]
    static class Operations
	{
		[HandlesOperation(SubRecordOperationType.Read)]
		internal static object Read(SubRecordReadContext context)
		{
            switch (context.header.Size)
            {
                case 1:
                    return new ByteSubRecord(context.SubRecordName, context.buf[0]);
                case 4:
                    return new IntSubRecord(context.SubRecordName, BitConverter.ToInt32(context.buf, 0));
                default:
                    throw new InvalidOperationException($"Unrecognized dialogue DATA size: {context.header.Size}");
            }
		}
		
		[HandlesOperation(SubRecordOperationType.Size)]
		internal static object GetSize(SubRecordLengthContext context)
		{
            return context.subRecord is ByteSubRecord ? 1 : 4;
		}
		
		[HandlesOperation(SubRecordOperationType.Write)]
		internal static object Write(SubRecordWriteContext context)
		{
            if (context.subRecord is ByteSubRecord byteRecord)
            {
                context.stream.WriteByte(byteRecord.Data);
            }
            else if (context.subRecord is IntSubRecord intRecord)
            {
                WriteBytes(context.stream, BitConverter.GetBytes(intRecord.Data));
            }
			return null;
		}
		
		[HandlesOperation(SubRecordOperationType.AddIndex)]
		internal static object GetAddIndex(SubRecordAddIndexContext context)
		{
			return GetAddIndexOrdered(context.record, "DATA", Names.SubRecordOrder);
		}
		
	}
	
}
