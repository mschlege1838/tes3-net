
using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.RACE.NPCS 
{
	
	[HandlesSubRecord("RACE", "NPCS")]
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
            return 32;
		}
		
		[HandlesOperation(SubRecordOperationType.Write)]
		internal static object Write(SubRecordWriteContext context)
		{
            WriteBytes(context.stream, GetBytes(((StringSubRecord) context.subRecord).Data, Constants.RACE_NPCS_LENGTH, "RACE.NPCS"));
			return null;
		}
		
		[HandlesOperation(SubRecordOperationType.AddIndex)]
		internal static object GetAddIndex(SubRecordAddIndexContext context)
		{
			return GetAddIndexOrdered(context.record, "NPCS", Names.SubRecordOrder);
		}
		
	}
	
}
