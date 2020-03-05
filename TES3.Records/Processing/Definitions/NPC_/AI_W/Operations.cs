
using System;

using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.NPC_.AI_W 
{
	
	[HandlesSubRecord("NPC_", "AI_W")]
    static class Operations
	{
		[HandlesOperation(SubRecordOperationType.Read)]
		internal static object Read(SubRecordReadContext context)
		{
            var pos = 0;

            var distance = BitConverter.ToInt16(context.buf, pos); pos += 2;
            var duration = BitConverter.ToInt16(context.buf, pos); pos += 2;
            var timeOfDay = context.buf[pos++];

            var idle = new byte[8];
            Array.Copy(context.buf, pos, idle, 0, 8); pos += 8;

            var unknown = context.buf[pos++];

            return new AIWanderData(context.SubRecordName, distance, duration, timeOfDay, idle, unknown);
		}
		
		[HandlesOperation(SubRecordOperationType.Size)]
		internal static object GetSize(SubRecordLengthContext context)
		{
            return 14;
		}
		
		[HandlesOperation(SubRecordOperationType.Write)]
		internal static object Write(SubRecordWriteContext context)
		{
        
            var aiData = (AIWanderData) context.subRecord;

            WriteBytes(context.stream, BitConverter.GetBytes(aiData.Distance));
            WriteBytes(context.stream, BitConverter.GetBytes(aiData.Duration));
            context.stream.WriteByte(aiData.TimeOfDay);

            WriteBytes(context.stream, aiData.Idle);

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
