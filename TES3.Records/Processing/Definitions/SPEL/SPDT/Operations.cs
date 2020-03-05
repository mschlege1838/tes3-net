
using System;

using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.SPEL.SPDT 
{
	
	[HandlesSubRecord("SPEL", "SPDT")]
    static class Operations
	{
		[HandlesOperation(SubRecordOperationType.Read)]
		internal static object Read(SubRecordReadContext context)
		{
        
            var pos = 0;

            var type = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var spellCost = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var flags = BitConverter.ToInt32(context.buf, pos); pos += 4;

            return new SpellData(context.SubRecordName, type, spellCost, flags);
		}
		
		[HandlesOperation(SubRecordOperationType.Size)]
		internal static object GetSize(SubRecordLengthContext context)
		{
            return 12;
		}
		
		[HandlesOperation(SubRecordOperationType.Write)]
		internal static object Write(SubRecordWriteContext context)
		{
        
            var spellData = (SpellData) context.subRecord;

            WriteBytes(context.stream, BitConverter.GetBytes(spellData.Type));
            WriteBytes(context.stream, BitConverter.GetBytes(spellData.SpellCost));
            WriteBytes(context.stream, BitConverter.GetBytes(spellData.Flags));
			return null;
		}
		
		[HandlesOperation(SubRecordOperationType.AddIndex)]
		internal static object GetAddIndex(SubRecordAddIndexContext context)
		{
			return GetAddIndexOrdered(context.record, "SPDT", Names.SubRecordOrder);
		}
		
	}
	
}
