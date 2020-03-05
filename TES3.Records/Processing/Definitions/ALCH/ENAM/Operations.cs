
using System;

using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.ALCH.ENAM 
{
	
	[HandlesSubRecord("ALCH", "ENAM")]
    static class Operations
	{
		[HandlesOperation(SubRecordOperationType.Read)]
		internal static object Read(SubRecordReadContext context)
		{
        
            var pos = 0;

            var effectID = BitConverter.ToInt16(context.buf, pos); pos += 2;
            var skillID = context.buf[pos++];
            var attributeID = context.buf[pos++];
            var rangeType = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var area = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var duration = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var magMin = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var magMax = BitConverter.ToInt32(context.buf, pos); pos += 4;

            return new SpellItemData(context.SubRecordName, effectID, skillID, attributeID, rangeType,
                    area, duration, magMin, magMax);
		}
		
		[HandlesOperation(SubRecordOperationType.Size)]
		internal static object GetSize(SubRecordLengthContext context)
		{
            return 24;
		}
		
		[HandlesOperation(SubRecordOperationType.Write)]
		internal static object Write(SubRecordWriteContext context)
		{
        
            var item = (SpellItemData) context.subRecord;

            WriteBytes(context.stream, BitConverter.GetBytes(item.EffectId));
            context.stream.WriteByte(item.SkillId);
            context.stream.WriteByte(item.AttributeId);
            WriteBytes(context.stream, BitConverter.GetBytes(item.RangeType));
            WriteBytes(context.stream, BitConverter.GetBytes(item.Area));
            WriteBytes(context.stream, BitConverter.GetBytes(item.Duration));
            WriteBytes(context.stream, BitConverter.GetBytes(item.MagMin));
            WriteBytes(context.stream, BitConverter.GetBytes(item.MagMax));
			return null;
		}
		
		[HandlesOperation(SubRecordOperationType.AddIndex)]
		internal static object GetAddIndex(SubRecordAddIndexContext context)
		{
			return GetAddIndexOrdered(context.record, "ENAM", Names.SubRecordOrder);
		}
		
	}
	
}
