
using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.LAND.WNAM 
{
	
	[HandlesSubRecord("LAND", "WNAM")]
    static class Operations
	{
		[HandlesOperation(SubRecordOperationType.Read)]
		internal static object Read(SubRecordReadContext context)
		{
        
            var pos = 0;

            var palette = new byte[WorldMapPaletteSubRecord.PALETTE_SIDE_LENGTH, WorldMapPaletteSubRecord.PALETTE_SIDE_LENGTH];
            for (var i = 0; i < WorldMapPaletteSubRecord.PALETTE_SIDE_LENGTH; ++i)
            {
                for (var j = 0; j < WorldMapPaletteSubRecord.PALETTE_SIDE_LENGTH; ++j)
                {
                    palette[i, j] = context.buf[pos++];
                }
            }

            return new WorldMapPaletteSubRecord(context.SubRecordName, palette);
		}
		
		[HandlesOperation(SubRecordOperationType.Size)]
		internal static object GetSize(SubRecordLengthContext context)
		{
            return 81;
		}
		
		[HandlesOperation(SubRecordOperationType.Write)]
		internal static object Write(SubRecordWriteContext context)
		{
        
            var paletteData = (WorldMapPaletteSubRecord) context.subRecord;

            for (var i = 0; i < WorldMapPaletteSubRecord.PALETTE_SIDE_LENGTH; ++i)
            {
                for (var j = 0; j < WorldMapPaletteSubRecord.PALETTE_SIDE_LENGTH; ++j)
                {
                    context.stream.WriteByte(paletteData.Palette[i, j]);
                }
            }
			return null;
		}
		
		[HandlesOperation(SubRecordOperationType.AddIndex)]
		internal static object GetAddIndex(SubRecordAddIndexContext context)
		{
			return GetAddIndexOrdered(context.record, "WNAM", Names.SubRecordOrder);
		}
		
	}
	
}
