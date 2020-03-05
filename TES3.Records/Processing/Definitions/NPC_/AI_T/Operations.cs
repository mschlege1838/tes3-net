
using System;

using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.NPC_.AI_T 
{
	
	[HandlesSubRecord("NPC_", "AI_T")]
    static class Operations
	{
		[HandlesOperation(SubRecordOperationType.Read)]
		internal static object Read(SubRecordReadContext context)
		{
        
            var pos = 0;

            var x = BitConverter.ToSingle(context.buf, pos); pos += 4;
            var y = BitConverter.ToSingle(context.buf, pos); pos += 4;
            var z = BitConverter.ToSingle(context.buf, pos); pos += 4;
            var unknown = BitConverter.ToInt32(context.buf, pos); pos += 4;

            return new AITravelData(context.SubRecordName, x, y, z, unknown);
		}
		
		[HandlesOperation(SubRecordOperationType.Size)]
		internal static object GetSize(SubRecordLengthContext context)
		{
            return 16;
		}
		
		[HandlesOperation(SubRecordOperationType.Write)]
		internal static object Write(SubRecordWriteContext context)
		{
        
            var aiData = (AITravelData) context.subRecord;

            WriteBytes(context.stream, BitConverter.GetBytes(aiData.X));
            WriteBytes(context.stream, BitConverter.GetBytes(aiData.Y));
            WriteBytes(context.stream, BitConverter.GetBytes(aiData.Z));
            WriteBytes(context.stream, BitConverter.GetBytes(aiData.Unknown));
			return null;
		}
		
		[HandlesOperation(SubRecordOperationType.AddIndex)]
		internal static object GetAddIndex(SubRecordAddIndexContext context)
		{
            return Names.GetAddIndexForAIData(context);
        }
		
	}
	
}
