
using System;

using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.MGEF.MEDT 
{
	
	[HandlesSubRecord("MGEF", "MEDT")]
    static class Operations
	{
		[HandlesOperation(SubRecordOperationType.Read)]
		internal static object Read(SubRecordReadContext context)
		{
        
            var pos = 0;

            var spellSchool = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var baseCost = BitConverter.ToSingle(context.buf, pos); pos += 4;
            var flags = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var red = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var blue = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var green = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var speedX = BitConverter.ToSingle(context.buf, pos); pos += 4;
            var sizeX = BitConverter.ToSingle(context.buf, pos); pos += 4;
            var sizeCap = BitConverter.ToSingle(context.buf, pos); pos += 4;

            return new MagicEffectData(context.SubRecordName, spellSchool, baseCost, flags, red, blue, green, speedX, sizeX, sizeCap);
		}
		
		[HandlesOperation(SubRecordOperationType.Size)]
		internal static object GetSize(SubRecordLengthContext context)
		{
            return 36;
		}
		
		[HandlesOperation(SubRecordOperationType.Write)]
		internal static object Write(SubRecordWriteContext context)
		{
        
            var magicEffectData = (MagicEffectData) context.subRecord;

            WriteBytes(context.stream, BitConverter.GetBytes(magicEffectData.SpellSchool));
            WriteBytes(context.stream, BitConverter.GetBytes(magicEffectData.BaseCost));
            WriteBytes(context.stream, BitConverter.GetBytes(magicEffectData.Flags));
            WriteBytes(context.stream, BitConverter.GetBytes(magicEffectData.Red));
            WriteBytes(context.stream, BitConverter.GetBytes(magicEffectData.Blue));
            WriteBytes(context.stream, BitConverter.GetBytes(magicEffectData.Green));
            WriteBytes(context.stream, BitConverter.GetBytes(magicEffectData.SpeedX));
            WriteBytes(context.stream, BitConverter.GetBytes(magicEffectData.SizeX));
            WriteBytes(context.stream, BitConverter.GetBytes(magicEffectData.SizeCap));
			return null;
		}
		
		[HandlesOperation(SubRecordOperationType.AddIndex)]
		internal static object GetAddIndex(SubRecordAddIndexContext context)
		{
			return GetAddIndexOrdered(context.record, "MEDT", Names.SubRecordOrder);
		}
		
	}
	
}
