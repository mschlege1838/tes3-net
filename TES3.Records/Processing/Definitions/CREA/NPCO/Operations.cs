
using System;

using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.CREA.NPCO 
{
	
	[HandlesSubRecord("CREA", "NPCO")]
    static class Operations
	{
		[HandlesOperation(SubRecordOperationType.Read)]
		internal static object Read(SubRecordReadContext context)
		{
        
            var pos = 0;

            var count = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var name = GetString(context.buf, pos, 32); pos += 32;

            return new InventoryItemSubRecord(context.SubRecordName, count, name);
		}
		
		[HandlesOperation(SubRecordOperationType.Size)]
		internal static object GetSize(SubRecordLengthContext context)
		{
            return 36;
		}
		
		[HandlesOperation(SubRecordOperationType.Write)]
		internal static object Write(SubRecordWriteContext context)
		{
        
            var item = (InventoryItemSubRecord) context.subRecord;

            WriteBytes(context.stream, BitConverter.GetBytes(item.Count));
            WriteBytes(context.stream, GetBytes(item.ItemName, Constants.NPCO_ITEM_NAME_LENGTH, "CREA.NPCO.ItemName"));
			return null;
		}
		
		[HandlesOperation(SubRecordOperationType.AddIndex)]
		internal static object GetAddIndex(SubRecordAddIndexContext context)
		{
            return GetAddIndexOrdered(context.record, "NPCO", Names.InitalSubRecordOrder);
		}
		
	}
	
}
