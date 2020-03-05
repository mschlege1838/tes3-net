
using System;

using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.ENCH.ENDT 
{
	
	[HandlesSubRecord("ENCH", "ENDT")]
    static class Operations
	{
		[HandlesOperation(SubRecordOperationType.Read)]
		internal static object Read(SubRecordReadContext context)
		{
        
            var pos = 0;

            var type = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var enchantCost = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var charge = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var autoCalc = BitConverter.ToInt32(context.buf, pos); pos += 4;

            return new EnchantData(context.SubRecordName, type, enchantCost, charge, autoCalc);
		}
		
		[HandlesOperation(SubRecordOperationType.Size)]
		internal static object GetSize(SubRecordLengthContext context)
		{
            return 16;
		}
		
		[HandlesOperation(SubRecordOperationType.Write)]
		internal static object Write(SubRecordWriteContext context)
		{
        
            var enchantData = (EnchantData) context.subRecord;

            WriteBytes(context.stream, BitConverter.GetBytes(enchantData.Type));
            WriteBytes(context.stream, BitConverter.GetBytes(enchantData.Cost));
            WriteBytes(context.stream, BitConverter.GetBytes(enchantData.Charge));
            WriteBytes(context.stream, BitConverter.GetBytes(enchantData.AutoCalc));
			return null;
		}
		
		[HandlesOperation(SubRecordOperationType.AddIndex)]
		internal static object GetAddIndex(SubRecordAddIndexContext context)
		{
			return GetAddIndexOrdered(context.record, "ENDT", Names.SubRecordOrder);
		}
		
	}
	
}
