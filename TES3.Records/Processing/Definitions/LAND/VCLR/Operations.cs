

using TES3.Core;
using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.LAND.VCLR 
{
	
	[HandlesSubRecord("LAND", "VCLR")]
    static class Operations
	{
		[HandlesOperation(SubRecordOperationType.Read)]
		internal static object Read(SubRecordReadContext context)
		{
        
            var pos = 0;
            var mapping = new TES3VertexColor[VertexColorSubRecord.COLOR_MAPPING_SIDE_LENGTH, VertexColorSubRecord.COLOR_MAPPING_SIDE_LENGTH];

            for (var i = 0; i < VertexColorSubRecord.COLOR_MAPPING_SIDE_LENGTH; ++i)
            {
                for (var j = 0; j < VertexColorSubRecord.COLOR_MAPPING_SIDE_LENGTH; ++j)
                {
                    mapping[i, j] = new TES3VertexColor(context.buf[pos++], context.buf[pos++], context.buf[pos++]);
                }
            }

            return new VertexColorSubRecord(context.SubRecordName, mapping);
		}
		
		[HandlesOperation(SubRecordOperationType.Size)]
		internal static object GetSize(SubRecordLengthContext context)
		{
            return 12675;
		}
		
		[HandlesOperation(SubRecordOperationType.Write)]
		internal static object Write(SubRecordWriteContext context)
		{
        
            var colorData = (VertexColorSubRecord) context.subRecord;

            for (var i = 0; i < VertexColorSubRecord.COLOR_MAPPING_SIDE_LENGTH; ++i)
            {
                for (var j = 0; j < VertexColorSubRecord.COLOR_MAPPING_SIDE_LENGTH; ++j)
                {
                    var color = colorData.ColorMapping[i, j];

                    context.stream.WriteByte(color.Red);
                    context.stream.WriteByte(color.Green);
                    context.stream.WriteByte(color.Blue);
                }
            }
			return null;
		}
		
		[HandlesOperation(SubRecordOperationType.AddIndex)]
		internal static object GetAddIndex(SubRecordAddIndexContext context)
		{
			return GetAddIndexOrdered(context.record, "VCLR", Names.SubRecordOrder);
		}
		
	}
	
}
