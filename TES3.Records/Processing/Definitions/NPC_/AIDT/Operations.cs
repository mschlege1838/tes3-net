
using System;

using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.NPC_.AIDT 
{
	
	[HandlesSubRecord("NPC_", "AIDT")]
    static class Operations
	{
		[HandlesOperation(SubRecordOperationType.Read)]
		internal static object Read(SubRecordReadContext context)
		{
        
            var pos = 0;

            var hello = context.buf[pos++];
            var unknown = context.buf[pos++];
            var fight = context.buf[pos++];
            var flee = context.buf[pos++];
            var alarm = context.buf[pos++];
            var unknown1 = context.buf[pos++];
            var unknown2 = context.buf[pos++];
            var unknown3 = context.buf[pos++];
            var flags = BitConverter.ToInt32(context.buf, pos); pos += 4;

            return new AIData(context.SubRecordName, hello, unknown, fight, flee, alarm, unknown1, unknown2, unknown3, flags);
		}
		
		[HandlesOperation(SubRecordOperationType.Size)]
		internal static object GetSize(SubRecordLengthContext context)
		{
            return 12;
		}
		
		[HandlesOperation(SubRecordOperationType.Write)]
		internal static object Write(SubRecordWriteContext context)
		{
        
            var aiData = (AIData) context.subRecord;

            context.stream.WriteByte(aiData.Hello);
            context.stream.WriteByte(aiData.Unknown);
            context.stream.WriteByte(aiData.Fight);
            context.stream.WriteByte(aiData.Flee);
            context.stream.WriteByte(aiData.Alarm);
            context.stream.WriteByte(aiData.Unknown1);
            context.stream.WriteByte(aiData.Unknown2);
            context.stream.WriteByte(aiData.Unknown3);
            WriteBytes(context.stream, BitConverter.GetBytes(aiData.Flags));
			return null;
		}
		
		[HandlesOperation(SubRecordOperationType.AddIndex)]
		internal static object GetAddIndex(SubRecordAddIndexContext context)
		{
            return GetAddIndexOrdered(context.record, "AIDT", Names.InitialSubRecordOrder);
		}
		
	}
	
}
