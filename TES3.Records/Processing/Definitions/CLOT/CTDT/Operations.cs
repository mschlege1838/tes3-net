
using System;

using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.CLOT.CTDT 
{
	
	[HandlesSubRecord("CLOT", "CTDT")]
    static class Operations
	{
		[HandlesOperation(SubRecordOperationType.Read)]
		internal static object Read(SubRecordReadContext context)
		{
        
            var pos = 0;

            var type = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var weight = BitConverter.ToSingle(context.buf, pos); pos += 4;
            var value = BitConverter.ToInt16(context.buf, pos); pos += 2;
            var enchantPoints = BitConverter.ToInt16(context.buf, pos); pos += 2;

            return new ClothingData(context.SubRecordName, type, weight, value, enchantPoints);
		}
		
		[HandlesOperation(SubRecordOperationType.Size)]
		internal static object GetSize(SubRecordLengthContext context)
		{
            return 12;
		}
		
		[HandlesOperation(SubRecordOperationType.Write)]
		internal static object Write(SubRecordWriteContext context)
		{
        
            var clothingData = (ClothingData) context.subRecord;

            WriteBytes(context.stream, BitConverter.GetBytes(clothingData.Type));
            WriteBytes(context.stream, BitConverter.GetBytes(clothingData.Weight));
            WriteBytes(context.stream, BitConverter.GetBytes(clothingData.Value));
            WriteBytes(context.stream, BitConverter.GetBytes(clothingData.EnchantPoints));
			return null;
		}
		
		[HandlesOperation(SubRecordOperationType.AddIndex)]
		internal static object GetAddIndex(SubRecordAddIndexContext context)
		{
            return GetAddIndexOrdered(context.record, "CTDT", Names.InitialSubRecordOrder);
		}
		
	}
	
}
