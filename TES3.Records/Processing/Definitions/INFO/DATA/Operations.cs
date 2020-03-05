
using System;

using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.INFO.DATA 
{
	
	[HandlesSubRecord("INFO", "DATA")]
    static class Operations
	{
		[HandlesOperation(SubRecordOperationType.Read)]
		internal static object Read(SubRecordReadContext context)
		{
        
            var pos = 0;

            var unknown = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var disposition = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var rank = context.buf[pos++];
            var gender = context.buf[pos++];
            var pcRank = context.buf[pos++];
            var unknown1 = context.buf[pos++];

            return new DialogueInfoData(context.SubRecordName, unknown, disposition, rank, gender, pcRank, unknown1);
		}
		
		[HandlesOperation(SubRecordOperationType.Size)]
		internal static object GetSize(SubRecordLengthContext context)
		{
            return 12;
		}
		
		[HandlesOperation(SubRecordOperationType.Write)]
		internal static object Write(SubRecordWriteContext context)
		{
        
            var infoData = (DialogueInfoData) context.subRecord;

            WriteBytes(context.stream, BitConverter.GetBytes(infoData.Unknown));
            WriteBytes(context.stream, BitConverter.GetBytes(infoData.Disposition));
            context.stream.WriteByte(infoData.Rank);
            context.stream.WriteByte(infoData.Gender);
            context.stream.WriteByte(infoData.PCRank);
            context.stream.WriteByte(infoData.Unknown1);
			return null;
		}
		
		[HandlesOperation(SubRecordOperationType.AddIndex)]
		internal static object GetAddIndex(SubRecordAddIndexContext context)
		{
            return GetAddIndexOrdered(context.record, "DATA", Names.InitialSubRecordOrder);
		}
		
	}
	
}
