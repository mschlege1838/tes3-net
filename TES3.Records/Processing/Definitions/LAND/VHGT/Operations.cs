
using System;

using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.LAND.VHGT 
{
	
	[HandlesSubRecord("LAND", "VHGT")]
    static class Operations
	{
		[HandlesOperation(SubRecordOperationType.Read)]
		internal static object Read(SubRecordReadContext context)
		{
        
            var pos = 0;

            var baseHeight = BitConverter.ToSingle(context.buf, pos); pos += 4;

            var slopeMapping = new sbyte[HeightMapSubRecord.MAPPING_SIDE_LENGTH, HeightMapSubRecord.MAPPING_SIDE_LENGTH];
            for (var i = 0; i < HeightMapSubRecord.MAPPING_SIDE_LENGTH; ++i)
            {
                for (var j = 0; j < HeightMapSubRecord.MAPPING_SIDE_LENGTH; j++)
                {
                    slopeMapping[i, j] = (sbyte) context.buf[pos++];
                }
            }

            var unknown = new byte[3];
            Array.Copy(context.buf, pos, unknown, 0, 3); pos += 3;

            return new HeightMapSubRecord(context.SubRecordName, baseHeight, slopeMapping, unknown);
		}
		
		[HandlesOperation(SubRecordOperationType.Size)]
		internal static object GetSize(SubRecordLengthContext context)
		{
            return 4232;
		}
		
		[HandlesOperation(SubRecordOperationType.Write)]
		internal static object Write(SubRecordWriteContext context)
		{
        
            var heightData = (HeightMapSubRecord) context.subRecord;

            WriteBytes(context.stream, BitConverter.GetBytes(heightData.BaseHeight));

            for (var i = 0; i < HeightMapSubRecord.MAPPING_SIDE_LENGTH; ++i)
            {
                for (var j = 0; j < HeightMapSubRecord.MAPPING_SIDE_LENGTH; j++)
                {
                    context.stream.WriteByte((byte) heightData.SlopeMapping[i, j]);
                }
            }

            WriteBytes(context.stream, heightData.Unknown);
			return null;
		}
		
		[HandlesOperation(SubRecordOperationType.AddIndex)]
		internal static object GetAddIndex(SubRecordAddIndexContext context)
		{
			return GetAddIndexOrdered(context.record, "VHGT", Names.SubRecordOrder);
		}
		
	}
	
}
