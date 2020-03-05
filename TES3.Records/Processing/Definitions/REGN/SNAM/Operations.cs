
using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.REGN.SNAM 
{
	
	[HandlesSubRecord("REGN", "SNAM")]
    static class Operations
	{
		[HandlesOperation(SubRecordOperationType.Read)]
		internal static object Read(SubRecordReadContext context)
		{
        
            // All sound names are null-terminated strings, but the data size is hard-coded
            // as 32 bits; any data after the first null character can be discarded. When writing
            // the file, 32 bits must be written, but all subsequent bits after the first null 
            // character can also be null.
            var index = 0;
            while (index < context.header.Size && context.buf[index++] != 0) ;
            var soundName = GetString(context.buf, 0, index);

            var chance = context.buf[32];

            return new RegionSoundData(context.SubRecordName, soundName, chance);
		}
		
		[HandlesOperation(SubRecordOperationType.Size)]
		internal static object GetSize(SubRecordLengthContext context)
		{
            return 33;
		}
		
		[HandlesOperation(SubRecordOperationType.Write)]
		internal static object Write(SubRecordWriteContext context)
		{
        
            var regionSoundData = (RegionSoundData) context.subRecord;

            WriteBytes(context.stream, GetBytes(regionSoundData.SoundName, Constants.REGN_SOUND_NAME_LENGTH, "REGN.SNAM.SoundName"));
            context.stream.WriteByte(regionSoundData.Chance);
			return null;
		}
		
		[HandlesOperation(SubRecordOperationType.AddIndex)]
		internal static object GetAddIndex(SubRecordAddIndexContext context)
		{
			return GetAddIndexOrdered(context.record, "SNAM", Names.SubRecordOrder);
		}
		
	}
	
}
