
using System;

using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.CREA.NPDT 
{
	
	[HandlesSubRecord("CREA", "NPDT")]
    static class Operations
	{
		[HandlesOperation(SubRecordOperationType.Read)]
		internal static object Read(SubRecordReadContext context)
		{
        
            var pos = 0;

            var type = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var level = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var strength = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var intelligence = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var willpower = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var agility = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var speed = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var endurance = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var personality = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var luck = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var health = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var spellPoints = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var fatigue = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var soul = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var combat = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var magic = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var stealth = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var attackMin1 = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var attackMax1 = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var attackMin2 = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var attackMax2 = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var attackMin3 = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var attackMax3 = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var gold = BitConverter.ToInt32(context.buf, pos); pos += 4;

            return new CreatureData(context.SubRecordName, type, level, strength, intelligence, willpower, agility,
                    speed, endurance, personality, luck, health, spellPoints, fatigue, soul, combat,
                    magic, stealth, attackMin1, attackMax1, attackMin2, attackMax2, attackMin3,
                    attackMax3, gold);
		}
		
		[HandlesOperation(SubRecordOperationType.Size)]
		internal static object GetSize(SubRecordLengthContext context)
		{
            return 96;
		}
		
		[HandlesOperation(SubRecordOperationType.Write)]
		internal static object Write(SubRecordWriteContext context)
		{
        
            var creatureData = (CreatureData) context.subRecord;

            WriteBytes(context.stream, BitConverter.GetBytes(creatureData.Type));
            WriteBytes(context.stream, BitConverter.GetBytes(creatureData.Level));
            WriteBytes(context.stream, BitConverter.GetBytes(creatureData.Strength));
            WriteBytes(context.stream, BitConverter.GetBytes(creatureData.Intelligence));
            WriteBytes(context.stream, BitConverter.GetBytes(creatureData.Willpower));
            WriteBytes(context.stream, BitConverter.GetBytes(creatureData.Agility));
            WriteBytes(context.stream, BitConverter.GetBytes(creatureData.Speed));
            WriteBytes(context.stream, BitConverter.GetBytes(creatureData.Endurance));
            WriteBytes(context.stream, BitConverter.GetBytes(creatureData.Personality));
            WriteBytes(context.stream, BitConverter.GetBytes(creatureData.Luck));
            WriteBytes(context.stream, BitConverter.GetBytes(creatureData.Health));
            WriteBytes(context.stream, BitConverter.GetBytes(creatureData.SpellPoints));
            WriteBytes(context.stream, BitConverter.GetBytes(creatureData.Fatigue));
            WriteBytes(context.stream, BitConverter.GetBytes(creatureData.Soul));
            WriteBytes(context.stream, BitConverter.GetBytes(creatureData.Combat));
            WriteBytes(context.stream, BitConverter.GetBytes(creatureData.Magic));
            WriteBytes(context.stream, BitConverter.GetBytes(creatureData.Stealth));
            WriteBytes(context.stream, BitConverter.GetBytes(creatureData.AttackMin1));
            WriteBytes(context.stream, BitConverter.GetBytes(creatureData.AttackMax1));
            WriteBytes(context.stream, BitConverter.GetBytes(creatureData.AttackMin2));
            WriteBytes(context.stream, BitConverter.GetBytes(creatureData.AttackMax2));
            WriteBytes(context.stream, BitConverter.GetBytes(creatureData.AttackMin3));
            WriteBytes(context.stream, BitConverter.GetBytes(creatureData.AttackMax3));
            WriteBytes(context.stream, BitConverter.GetBytes(creatureData.Gold));
			return null;
		}
		
		[HandlesOperation(SubRecordOperationType.AddIndex)]
		internal static object GetAddIndex(SubRecordAddIndexContext context)
		{
            return GetAddIndexOrdered(context.record, "NPDT", Names.InitalSubRecordOrder);
		}
		
	}
	
}
