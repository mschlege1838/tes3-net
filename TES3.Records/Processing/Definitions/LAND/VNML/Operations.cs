
using TES3.Core;
using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.LAND.VNML 
{
	
	[HandlesSubRecord("LAND", "VNML")]
    static class Operations
	{
		[HandlesOperation(SubRecordOperationType.Read)]
		internal static object Read(SubRecordReadContext context)
		{
        
            var pos = 0;
            var mapping = new TES3VertexNormal[VertexNormalSubRecord.MAPPING_SIDE_LENGTH, VertexNormalSubRecord.MAPPING_SIDE_LENGTH];

            for (var i = 0; i < VertexNormalSubRecord.MAPPING_SIDE_LENGTH; ++i)
            {
                for (var j = 0; j < VertexNormalSubRecord.MAPPING_SIDE_LENGTH; ++j)
                {
                    var x = (sbyte) context.buf[pos++];
                    var y = (sbyte) context.buf[pos++];
                    var z = (sbyte) context.buf[pos++];

                    mapping[i, j] = new TES3VertexNormal(x, y, z);
                }
            }

            return new VertexNormalSubRecord(context.SubRecordName, mapping);
		}
		
		[HandlesOperation(SubRecordOperationType.Size)]
		internal static object GetSize(SubRecordLengthContext context)
		{
            return 12675;
		}
		
		[HandlesOperation(SubRecordOperationType.Write)]
		internal static object Write(SubRecordWriteContext context)
		{
        
            var normalData = (VertexNormalSubRecord) context.subRecord;

            for (var i = 0; i < VertexNormalSubRecord.MAPPING_SIDE_LENGTH; ++i)
            {
                for (var j = 0; j < VertexNormalSubRecord.MAPPING_SIDE_LENGTH; ++j)
                {
                    var vec = normalData.NormalVectorMapping[i, j];

                    context.stream.WriteByte((byte) vec.X);
                    context.stream.WriteByte((byte) vec.Y);
                    context.stream.WriteByte((byte) vec.Z);
                }
            }
			return null;
		}
		
		[HandlesOperation(SubRecordOperationType.AddIndex)]
		internal static object GetAddIndex(SubRecordAddIndexContext context)
		{
			return GetAddIndexOrdered(context.record, "VNML", Names.SubRecordOrder);
		}
		
	}
	
}
