
using System;

using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.NPC_.AI_E 
{
	
	[HandlesSubRecord("NPC_", "AI_E")]
    static class Operations
	{
		[HandlesOperation(SubRecordOperationType.Read)]
		internal static object Read(SubRecordReadContext context)
		{
        
            var pos = 0;

            var x = BitConverter.ToSingle(context.buf, pos); pos += 4;
            var y = BitConverter.ToSingle(context.buf, pos); pos += 4;
            var z = BitConverter.ToSingle(context.buf, pos); pos += 4;
            var duration = BitConverter.ToInt16(context.buf, pos); pos += 2;

            var index = pos;
            var count = 0;
            while (context.buf[index++] != 0 && index < context.header.Size) ++count;
            var id = GetString(context.buf, pos, count); pos += 32;


            var unknown = BitConverter.ToInt16(context.buf, pos); pos += 2;

            return new AIFollowEscortData(context.SubRecordName, x, y, z, duration, id, unknown);
		}
		
		[HandlesOperation(SubRecordOperationType.Size)]
		internal static object GetSize(SubRecordLengthContext context)
		{
            return 48;
		}
		
		[HandlesOperation(SubRecordOperationType.Write)]
		internal static object Write(SubRecordWriteContext context)
		{
        
            var aiData = (AIFollowEscortData) context.subRecord;

            WriteBytes(context.stream, BitConverter.GetBytes(aiData.X));
            WriteBytes(context.stream, BitConverter.GetBytes(aiData.Y));
            WriteBytes(context.stream, BitConverter.GetBytes(aiData.Z));
            WriteBytes(context.stream, BitConverter.GetBytes(aiData.Duration));

            WriteBytes(context.stream, GetBytes(aiData.Id, Constants.AI_X_ID_LENGTH, $"NPC_.context.subRecord.Name.Id"));

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
