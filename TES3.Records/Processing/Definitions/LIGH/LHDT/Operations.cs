
using System;

using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.LIGH.LHDT 
{
	
	[HandlesSubRecord("LIGH", "LHDT")]
    static class Operations
	{
		[HandlesOperation(SubRecordOperationType.Read)]
		internal static object Read(SubRecordReadContext context)
		{
        
            var pos = 0;

            var weight = BitConverter.ToSingle(context.buf, pos); pos += 4;
            var value = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var time = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var radius = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var color = ReadColorRef(context.buf, ref pos);
            var flags = BitConverter.ToInt32(context.buf, pos); pos += 4;

            return new LightData(context.SubRecordName, weight, value, time, radius, color, flags);
		}
		
		[HandlesOperation(SubRecordOperationType.Size)]
		internal static object GetSize(SubRecordLengthContext context)
		{
            return 24;
		}
		
		[HandlesOperation(SubRecordOperationType.Write)]
		internal static object Write(SubRecordWriteContext context)
		{
        
            var lightData = (LightData) context.subRecord;

            WriteBytes(context.stream, BitConverter.GetBytes(lightData.Weight));
            WriteBytes(context.stream, BitConverter.GetBytes(lightData.Value));
            WriteBytes(context.stream, BitConverter.GetBytes(lightData.Time));
            WriteBytes(context.stream, BitConverter.GetBytes(lightData.Radius));
            WriteColorRef(context.stream, lightData.Color);
            WriteBytes(context.stream, BitConverter.GetBytes(lightData.Flags));
			return null;
		}
		
		[HandlesOperation(SubRecordOperationType.AddIndex)]
		internal static object GetAddIndex(SubRecordAddIndexContext context)
		{
			return GetAddIndexOrdered(context.record, "LHDT", Names.SubRecordOrder);
		}
		
	}
	
}
