
using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.CREA.AI_A 
{
	
	[HandlesSubRecord("CREA", "AI_A")]
    static class Operations
	{
		[HandlesOperation(SubRecordOperationType.Read)]
		internal static object Read(SubRecordReadContext context)
		{
        
            var pos = 0;

            var name = GetString(context.buf, pos, 32); pos += 32;
            var unknown = context.buf[pos++];

            return new AIActivateData(context.SubRecordName, name, unknown);
		}
		
		[HandlesOperation(SubRecordOperationType.Size)]
		internal static object GetSize(SubRecordLengthContext context)
		{
            return 33;
		}
		
		[HandlesOperation(SubRecordOperationType.Write)]
		internal static object Write(SubRecordWriteContext context)
		{
        
            var aiData = (AIActivateData) context.subRecord;

            WriteBytes(context.stream, GetBytes(aiData.TargetName, Constants.AI_X_ID_LENGTH, "CREA.AI_A.TargetName"));
            context.stream.WriteByte(aiData.Unknown);
			return null;
		}
		
		[HandlesOperation(SubRecordOperationType.AddIndex)]
		internal static object GetAddIndex(SubRecordAddIndexContext context)
		{
            return Names.GetAddIndexForAIData(context);
		}
		
	}
	
}
