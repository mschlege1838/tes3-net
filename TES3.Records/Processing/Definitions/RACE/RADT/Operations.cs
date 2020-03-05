
using System;

using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.RACE.RADT 
{
	
	[HandlesSubRecord("RACE", "RADT")]
    static class Operations
	{
		[HandlesOperation(SubRecordOperationType.Read)]
		internal static object Read(SubRecordReadContext context)
		{
        
            var pos = 0;

            var skillBonuses = new SkillBonus[7];
            for (var i = 0; i < Constants.RACE_MAX_SKILL_BONUSES; ++i)
            {
                var skillID = BitConverter.ToInt32(context.buf, pos); pos += 4;
                var bonus = BitConverter.ToInt32(context.buf, pos); pos += 4;

                skillBonuses[i] = new SkillBonus(skillID, bonus);
            }

            var strength = new int[2];
            strength[0] = BitConverter.ToInt32(context.buf, pos); pos += 4;
            strength[1] = BitConverter.ToInt32(context.buf, pos); pos += 4;

            var intelligence = new int[2];
            intelligence[0] = BitConverter.ToInt32(context.buf, pos); pos += 4;
            intelligence[1] = BitConverter.ToInt32(context.buf, pos); pos += 4;

            var willpower = new int[2];
            willpower[0] = BitConverter.ToInt32(context.buf, pos); pos += 4;
            willpower[1] = BitConverter.ToInt32(context.buf, pos); pos += 4;

            var agility = new int[2];
            agility[0] = BitConverter.ToInt32(context.buf, pos); pos += 4;
            agility[1] = BitConverter.ToInt32(context.buf, pos); pos += 4;

            var speed = new int[2];
            speed[0] = BitConverter.ToInt32(context.buf, pos); pos += 4;
            speed[1] = BitConverter.ToInt32(context.buf, pos); pos += 4;

            var endurance = new int[2];
            endurance[0] = BitConverter.ToInt32(context.buf, pos); pos += 4;
            endurance[1] = BitConverter.ToInt32(context.buf, pos); pos += 4;

            var personality = new int[2];
            personality[0] = BitConverter.ToInt32(context.buf, pos); pos += 4;
            personality[1] = BitConverter.ToInt32(context.buf, pos); pos += 4;

            var luck = new int[2];
            luck[0] = BitConverter.ToInt32(context.buf, pos); pos += 4;
            luck[1] = BitConverter.ToInt32(context.buf, pos); pos += 4;

            var height = new float[2];
            height[0] = BitConverter.ToSingle(context.buf, pos); pos += 4;
            height[1] = BitConverter.ToSingle(context.buf, pos); pos += 4;

            var weight = new float[2];
            weight[0] = BitConverter.ToSingle(context.buf, pos); pos += 4;
            weight[1] = BitConverter.ToSingle(context.buf, pos); pos += 4;

            var flags = BitConverter.ToInt32(context.buf, pos); pos += 4;

            return new RaceData(context.SubRecordName, skillBonuses, strength, intelligence, willpower, agility, speed,
                    endurance, personality, luck, height, weight, flags);
		}
		
		[HandlesOperation(SubRecordOperationType.Size)]
		internal static object GetSize(SubRecordLengthContext context)
		{
            return 140;
		}
		
		[HandlesOperation(SubRecordOperationType.Write)]
		internal static object Write(SubRecordWriteContext context)
		{
        
            var raceData = (RaceData) context.subRecord;

            for (var i = 0; i < Constants.RACE_MAX_SKILL_BONUSES; ++i)
            {
                var skillBonus = raceData.SkillBonuses[i];

                WriteBytes(context.stream, BitConverter.GetBytes(skillBonus.SkillId));
                WriteBytes(context.stream, BitConverter.GetBytes(skillBonus.Bonus));
            }

            var strength = raceData.Strength;
            WriteBytes(context.stream, BitConverter.GetBytes(strength[0]));
            WriteBytes(context.stream, BitConverter.GetBytes(strength[1]));

            var intelligence = raceData.Intelligence;
            WriteBytes(context.stream, BitConverter.GetBytes(intelligence[0]));
            WriteBytes(context.stream, BitConverter.GetBytes(intelligence[1]));

            var willpower = raceData.Willpower;
            WriteBytes(context.stream, BitConverter.GetBytes(willpower[0]));
            WriteBytes(context.stream, BitConverter.GetBytes(willpower[1]));

            var agility = raceData.Agility;
            WriteBytes(context.stream, BitConverter.GetBytes(agility[0]));
            WriteBytes(context.stream, BitConverter.GetBytes(agility[1]));

            var speed = raceData.Speed;
            WriteBytes(context.stream, BitConverter.GetBytes(speed[0]));
            WriteBytes(context.stream, BitConverter.GetBytes(speed[1]));

            var endurance = raceData.Endurance;
            WriteBytes(context.stream, BitConverter.GetBytes(endurance[0]));
            WriteBytes(context.stream, BitConverter.GetBytes(endurance[1]));

            var personality = raceData.Personality;
            WriteBytes(context.stream, BitConverter.GetBytes(personality[0]));
            WriteBytes(context.stream, BitConverter.GetBytes(personality[1]));

            var luck = raceData.Luck;
            WriteBytes(context.stream, BitConverter.GetBytes(luck[0]));
            WriteBytes(context.stream, BitConverter.GetBytes(luck[1]));

            var height = raceData.Height;
            WriteBytes(context.stream, BitConverter.GetBytes(height[0]));
            WriteBytes(context.stream, BitConverter.GetBytes(height[1]));

            var weight = raceData.Weight;
            WriteBytes(context.stream, BitConverter.GetBytes(weight[0]));
            WriteBytes(context.stream, BitConverter.GetBytes(weight[1]));

            WriteBytes(context.stream, BitConverter.GetBytes(raceData.Flags));
			return null;
		}
		
		[HandlesOperation(SubRecordOperationType.AddIndex)]
		internal static object GetAddIndex(SubRecordAddIndexContext context)
		{
			return GetAddIndexOrdered(context.record, "RADT", Names.SubRecordOrder);
		}
		
	}
	
}
