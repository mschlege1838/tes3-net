
using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.NPC_.DODT 
{
	
	[HandlesSubRecord("NPC_", "DODT")]
    static class Operations
	{
		[HandlesOperation(SubRecordOperationType.Read)]
		internal static object Read(SubRecordReadContext context)
		{
        
            var pos = 0;
            return new PositionSubRecord(context.SubRecordName, ReadPositionRef(context.buf, ref pos));
		}
		
		[HandlesOperation(SubRecordOperationType.Size)]
		internal static object GetSize(SubRecordLengthContext context)
		{
            return 24;
		}
		
		[HandlesOperation(SubRecordOperationType.Write)]
		internal static object Write(SubRecordWriteContext context)
		{
            WritePositionRef(context.stream, ((PositionSubRecord) context.subRecord).Data);
			return null;
		}
		
		[HandlesOperation(SubRecordOperationType.AddIndex)]
		internal static object GetAddIndex(SubRecordAddIndexContext context)
		{
            return Names.GetAddIndexForTravelDestination(context);
        }
		
	}
	
}
