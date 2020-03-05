
using System;

using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.CELL.AMBI 
{
	
	[HandlesSubRecord("CELL", "AMBI")]
    static class Operations
	{
		[HandlesOperation(SubRecordOperationType.Read)]
		internal static object Read(SubRecordReadContext context)
		{
        
            var pos = 0;

            var ambientColor = ReadColorRef(context.buf, ref pos);
            var sunlightColor = ReadColorRef(context.buf, ref pos);
            var fogColor = ReadColorRef(context.buf, ref pos);
            var fogDensity = BitConverter.ToSingle(context.buf, pos); pos += 4;

            return new InteriorLightSubRecord(context.SubRecordName, ambientColor, sunlightColor, fogColor, fogDensity);
		}
		
		[HandlesOperation(SubRecordOperationType.Size)]
		internal static object GetSize(SubRecordLengthContext context)
		{
            return 16;
		}
		
		[HandlesOperation(SubRecordOperationType.Write)]
		internal static object Write(SubRecordWriteContext context)
		{
        
            var lightData = (InteriorLightSubRecord) context.subRecord;

            WriteColorRef(context.stream, lightData.AmbientColor);
            WriteColorRef(context.stream, lightData.SunlightColor);
            WriteColorRef(context.stream, lightData.FogColor);
            WriteBytes(context.stream, BitConverter.GetBytes(lightData.FogDensity));
			return null;
		}
		
		[HandlesOperation(SubRecordOperationType.AddIndex)]
		internal static object GetAddIndex(SubRecordAddIndexContext context)
		{
            return GetAddIndexOrdered(context.record, "AMBI", Names.InitialSubRecordOrder);
		}
		
	}
	
}
