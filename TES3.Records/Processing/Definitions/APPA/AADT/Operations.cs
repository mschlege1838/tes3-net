
using System;

using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.APPA.AADT 
{
	
	[HandlesSubRecord("APPA", "AADT")]
    static class Operations
	{
		[HandlesOperation(SubRecordOperationType.Read)]
		internal static object Read(SubRecordReadContext context)
		{
        
            var pos = 0;

            var type = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var quality = BitConverter.ToSingle(context.buf, pos); pos += 4;
            var weight = BitConverter.ToSingle(context.buf, pos); pos += 4;
            var value = BitConverter.ToInt32(context.buf, pos); pos += 4;

            return new AlchemyApparatusData(context.SubRecordName, type, quality, weight, value);
		}
		
		[HandlesOperation(SubRecordOperationType.Size)]
		internal static object GetSize(SubRecordLengthContext context)
		{
            return 16;
		}
		
		[HandlesOperation(SubRecordOperationType.Write)]
		internal static object Write(SubRecordWriteContext context)
		{
        
            var apparatusData = (AlchemyApparatusData) context.subRecord;

            WriteBytes(context.stream, BitConverter.GetBytes(apparatusData.Type));
            WriteBytes(context.stream, BitConverter.GetBytes(apparatusData.Quality));
            WriteBytes(context.stream, BitConverter.GetBytes(apparatusData.Weight));
            WriteBytes(context.stream, BitConverter.GetBytes(apparatusData.Value));
			return null;
		}
		
		[HandlesOperation(SubRecordOperationType.AddIndex)]
		internal static object GetAddIndex(SubRecordAddIndexContext context)
		{
			return GetAddIndexOrdered(context.record, "AADT", Names.SubRecordOrder);
		}
		
	}
	
}
