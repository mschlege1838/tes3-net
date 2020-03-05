
using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.GLOB.FNAM 
{
	
	[HandlesSubRecord("GLOB", "FNAM")]
    static class Operations
	{
		[HandlesOperation(SubRecordOperationType.Read)]
		internal static object Read(SubRecordReadContext context)
		{
            return new StringSubRecord(context.SubRecordName, GetString(context.buf, 0, context.header.Size));
		}
		
		[HandlesOperation(SubRecordOperationType.Size)]
		internal static object GetSize(SubRecordLengthContext context)
		{
            return 1;
		}
		
		[HandlesOperation(SubRecordOperationType.Write)]
		internal static object Write(SubRecordWriteContext context)
		{
            WriteBytes(context.stream, GetBytes(((StringSubRecord) context.subRecord).Data, 1, "GLOB.FNAM"));
			return null;
		}
		
		[HandlesOperation(SubRecordOperationType.AddIndex)]
		internal static object GetAddIndex(SubRecordAddIndexContext context)
		{
			return GetAddIndexOrdered(context.record, "FNAM", Names.SubRecordOrder);
		}
		
	}
	
}
