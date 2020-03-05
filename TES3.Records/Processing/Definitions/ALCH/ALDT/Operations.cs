
using System;

using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.ALCH.ALDT 
{
	
	[HandlesSubRecord("ALCH", "ALDT")]
    static class Operations
	{
		[HandlesOperation(SubRecordOperationType.Read)]
		internal static object Read(SubRecordReadContext context)
		{
        
            var pos = 0;

            var weight = BitConverter.ToSingle(context.buf, pos); pos += 4;
            var value = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var autoCalc = BitConverter.ToInt32(context.buf, pos); pos += 4;

            return new PotionData(context.SubRecordName, weight, value, autoCalc);
		}
		
		[HandlesOperation(SubRecordOperationType.Size)]
		internal static object GetSize(SubRecordLengthContext context)
		{
            return 12;
		}
		
		[HandlesOperation(SubRecordOperationType.Write)]
		internal static object Write(SubRecordWriteContext context)
		{
        
            var potionData = (PotionData) context.subRecord;

            WriteBytes(context.stream, BitConverter.GetBytes(potionData.Weight));
            WriteBytes(context.stream, BitConverter.GetBytes(potionData.Value));
            WriteBytes(context.stream, BitConverter.GetBytes(potionData.AutoCalc));
			return null;
		}
		
		[HandlesOperation(SubRecordOperationType.AddIndex)]
		internal static object GetAddIndex(SubRecordAddIndexContext context)
		{
			return GetAddIndexOrdered(context.record, "ALDT", Names.SubRecordOrder);
		}
		
	}
	
}
