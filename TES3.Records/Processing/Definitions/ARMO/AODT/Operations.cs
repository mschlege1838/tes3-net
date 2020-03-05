
using System;

using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.ARMO.AODT 
{
	
	[HandlesSubRecord("ARMO", "AODT")]
    static class Operations
	{
		[HandlesOperation(SubRecordOperationType.Read)]
		internal static object Read(SubRecordReadContext context)
		{
        
            var pos = 0;

            var type = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var weight = BitConverter.ToSingle(context.buf, pos); pos += 4;
            var value = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var health = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var enchantPoints = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var armor = BitConverter.ToInt32(context.buf, pos); pos += 4;

            return new ArmorData(context.SubRecordName, type, weight, value, health, enchantPoints, armor);
		}
		
		[HandlesOperation(SubRecordOperationType.Size)]
		internal static object GetSize(SubRecordLengthContext context)
		{
            return 24;
		}
		
		[HandlesOperation(SubRecordOperationType.Write)]
		internal static object Write(SubRecordWriteContext context)
		{
        
            var armorData = (ArmorData) context.subRecord;

            WriteBytes(context.stream, BitConverter.GetBytes(armorData.Type));
            WriteBytes(context.stream, BitConverter.GetBytes(armorData.Weight));
            WriteBytes(context.stream, BitConverter.GetBytes(armorData.Value));
            WriteBytes(context.stream, BitConverter.GetBytes(armorData.Health));
            WriteBytes(context.stream, BitConverter.GetBytes(armorData.EnchantPoints));
            WriteBytes(context.stream, BitConverter.GetBytes(armorData.Armor));
			return null;
		}
		
		[HandlesOperation(SubRecordOperationType.AddIndex)]
		internal static object GetAddIndex(SubRecordAddIndexContext context)
		{
            return GetAddIndexOrdered(context.record, "AODT", Names.InitialSubRecordOrder);
		}
		
	}
	
}
