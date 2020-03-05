
using System;

using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.WEAP.WPDT 
{
	
	[HandlesSubRecord("WEAP", "WPDT")]
    static class Operations
	{
		[HandlesOperation(SubRecordOperationType.Read)]
		internal static object Read(SubRecordReadContext context)
		{
        
            var pos = 0;

            var weight = BitConverter.ToSingle(context.buf, pos); pos += 4;
            var value = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var type = BitConverter.ToInt16(context.buf, pos); pos += 2;
            var health = BitConverter.ToInt16(context.buf, pos); pos += 2;
            var speed = BitConverter.ToSingle(context.buf, pos); pos += 4;
            var reach = BitConverter.ToSingle(context.buf, pos); pos += 4;
            var enchantPoints = BitConverter.ToInt16(context.buf, pos); pos += 2;

            var chopMin = context.buf[pos++];
            var chopMax = context.buf[pos++];
            var slashMin = context.buf[pos++];
            var slashMax = context.buf[pos++];
            var thrustMin = context.buf[pos++];
            var thrustMax = context.buf[pos++];

            var flags = BitConverter.ToInt32(context.buf, pos); pos += 4;

            return new WeaponData(context.SubRecordName, weight, value, type, health, speed, reach, enchantPoints,
                    chopMin, chopMax, slashMin, slashMax, thrustMin, thrustMax, flags);
		}
		
		[HandlesOperation(SubRecordOperationType.Size)]
		internal static object GetSize(SubRecordLengthContext context)
		{
            return 32;
		}
		
		[HandlesOperation(SubRecordOperationType.Write)]
		internal static object Write(SubRecordWriteContext context)
		{
        
            var weaponData = (WeaponData) context.subRecord;

            WriteBytes(context.stream, BitConverter.GetBytes(weaponData.Weight));
            WriteBytes(context.stream, BitConverter.GetBytes(weaponData.Value));
            WriteBytes(context.stream, BitConverter.GetBytes(weaponData.Type));
            WriteBytes(context.stream, BitConverter.GetBytes(weaponData.Health));
            WriteBytes(context.stream, BitConverter.GetBytes(weaponData.Speed));
            WriteBytes(context.stream, BitConverter.GetBytes(weaponData.Reach));
            WriteBytes(context.stream, BitConverter.GetBytes(weaponData.EnchantPoints));

            context.stream.WriteByte(weaponData.ChopMin);
            context.stream.WriteByte(weaponData.ChopMax);
            context.stream.WriteByte(weaponData.SlashMin);
            context.stream.WriteByte(weaponData.SlashMax);
            context.stream.WriteByte(weaponData.ThrustMin);
            context.stream.WriteByte(weaponData.ThrustMax);

            WriteBytes(context.stream, BitConverter.GetBytes(weaponData.Flags));
			return null;
		}
		
		[HandlesOperation(SubRecordOperationType.AddIndex)]
		internal static object GetAddIndex(SubRecordAddIndexContext context)
		{
			return GetAddIndexOrdered(context.record, "WPDT", Names.SubRecordOrder);
		}
		
	}
	
}
