
using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.SOUN.DATA 
{
	
	[HandlesSubRecord("SOUN", "DATA")]
    static class Operations
	{
		[HandlesOperation(SubRecordOperationType.Read)]
		internal static object Read(SubRecordReadContext context)
		{
        
            var volume = context.buf[0];
            var minRange = context.buf[1];
            var maxRange = context.buf[2];

            return new SoundData(context.SubRecordName, volume, minRange, maxRange);
		}
		
		[HandlesOperation(SubRecordOperationType.Size)]
		internal static object GetSize(SubRecordLengthContext context)
		{
            return 3;
		}
		
		[HandlesOperation(SubRecordOperationType.Write)]
		internal static object Write(SubRecordWriteContext context)
		{
        
            var soundData = (SoundData) context.subRecord;

            context.stream.WriteByte(soundData.Volume);
            context.stream.WriteByte(soundData.MinRange);
            context.stream.WriteByte(soundData.MaxRange);
			return null;
		}
		
		[HandlesOperation(SubRecordOperationType.AddIndex)]
		internal static object GetAddIndex(SubRecordAddIndexContext context)
		{
			return GetAddIndexOrdered(context.record, "DATA", Names.SubRecordOrder);
		}
		
	}
	
}
