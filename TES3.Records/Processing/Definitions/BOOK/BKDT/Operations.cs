
using System;

using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.BOOK.BKDT 
{
	
	[HandlesSubRecord("BOOK", "BKDT")]
    static class Operations
	{
		[HandlesOperation(SubRecordOperationType.Read)]
		internal static object Read(SubRecordReadContext context)
		{
        
            var pos = 0;

            var weight = BitConverter.ToSingle(context.buf, pos); pos += 4;
            var value = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var scroll = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var skillID = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var enchantPoints = BitConverter.ToInt32(context.buf, pos); pos += 4;

            return new BookData(context.SubRecordName, weight, value, scroll, skillID, enchantPoints);
		}
		
		[HandlesOperation(SubRecordOperationType.Size)]
		internal static object GetSize(SubRecordLengthContext context)
		{
            return 20;
		}
		
		[HandlesOperation(SubRecordOperationType.Write)]
		internal static object Write(SubRecordWriteContext context)
		{
        
            var bookData = (BookData) context.subRecord;

            WriteBytes(context.stream, BitConverter.GetBytes(bookData.Weight));
            WriteBytes(context.stream, BitConverter.GetBytes(bookData.Value));
            WriteBytes(context.stream, BitConverter.GetBytes(bookData.Scroll));
            WriteBytes(context.stream, BitConverter.GetBytes(bookData.SkillId));
            WriteBytes(context.stream, BitConverter.GetBytes(bookData.EnchantPoints));
			return null;
		}
		
		[HandlesOperation(SubRecordOperationType.AddIndex)]
		internal static object GetAddIndex(SubRecordAddIndexContext context)
		{
			return GetAddIndexOrdered(context.record, "BKDT", Names.SubRecordOrder);
		}
		
	}
	
}
