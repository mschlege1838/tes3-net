
using System;

using TES3.Core;
using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.LAND.VTEX 
{
	
	[HandlesSubRecord("LAND", "VTEX")]
    static class Operations
	{
		[HandlesOperation(SubRecordOperationType.Read)]
		internal static object Read(SubRecordReadContext context)
		{
        
            var pos = 0;
            var map = new short[16, 16];
            for (var i = 0; i < 16; ++i)
            {
                for (var j = 0; j < 16; ++j)
                {
                    map[i, j] = BitConverter.ToInt16(context.buf, pos); pos += 2;
                }
            }

            var textureMap = new LandTextureMap(map, true);
            textureMap.UnSwizzle();
            return new LandTextureSubRecord(context.SubRecordName, textureMap);
		}
		
		[HandlesOperation(SubRecordOperationType.Size)]
		internal static object GetSize(SubRecordLengthContext context)
		{
            return 512;
		}
		
		[HandlesOperation(SubRecordOperationType.Write)]
		internal static object Write(SubRecordWriteContext context)
		{
        
            var textureMap = (LandTextureSubRecord) context.subRecord;

            var map = textureMap.Data;
            var initiallyUnswizzled = !map.Swizzled;
            if (initiallyUnswizzled)
            {
                map.Swizzle();
            }

            for (var i = 0; i < 16; ++i)
            {
                for (var j = 0; j < 16; ++j)
                {
                    WriteBytes(context.stream, BitConverter.GetBytes(map[i, j]));
                }
            }

            if (initiallyUnswizzled)
            {
                map.UnSwizzle();
            }
			return null;
		}
		
		[HandlesOperation(SubRecordOperationType.AddIndex)]
		internal static object GetAddIndex(SubRecordAddIndexContext context)
		{
			return GetAddIndexOrdered(context.record, "VTEX", Names.SubRecordOrder);
		}
		
	}
	
}
