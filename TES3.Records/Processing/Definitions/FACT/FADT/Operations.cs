
using System;

using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.FACT.FADT 
{
	
	[HandlesSubRecord("FACT", "FADT")]
    static class Operations
	{
		[HandlesOperation(SubRecordOperationType.Read)]
		internal static object Read(SubRecordReadContext context)
		{
        
            var pos = 0;

            var attributeID1 = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var attributeID2 = BitConverter.ToInt32(context.buf, pos); pos += 4;

            var rankData = new RankData[Constants.FACT_MAX_RANKS];
            for (var i = 0; i < Constants.FACT_MAX_RANKS; ++i)
            {
                var attribute1 = BitConverter.ToInt32(context.buf, pos); pos += 4;
                var attribute2 = BitConverter.ToInt32(context.buf, pos); pos += 4;
                var firstSkill = BitConverter.ToInt32(context.buf, pos); pos += 4;
                var secondSkill = BitConverter.ToInt32(context.buf, pos); pos += 4;
                var faction = BitConverter.ToInt32(context.buf, pos); pos += 4;

                rankData[i] = new RankData(attribute1, attribute2, firstSkill, secondSkill, faction);
            }

            var skillIds = new int[Constants.FACT_MAX_SKILL_IDS];
            for (int i = 0; i < Constants.FACT_MAX_SKILL_IDS; ++i)
            {
                skillIds[i] = BitConverter.ToInt32(context.buf, pos); pos += 4;
            }

            var flags = BitConverter.ToInt32(context.buf, pos); pos += 4;

            return new FactionData(context.SubRecordName, attributeID1, attributeID2, rankData, skillIds, flags);
		}
		
		[HandlesOperation(SubRecordOperationType.Size)]
		internal static object GetSize(SubRecordLengthContext context)
		{
            return 240;
		}
		
		[HandlesOperation(SubRecordOperationType.Write)]
		internal static object Write(SubRecordWriteContext context)
		{
        
            var factionData = (FactionData) context.subRecord;

            WriteBytes(context.stream, BitConverter.GetBytes(factionData.Attribute1));
            WriteBytes(context.stream, BitConverter.GetBytes(factionData.Attribute2));

            for (var i = 0; i < Constants.FACT_MAX_RANKS; ++i)
            {
                var rankData = factionData.RankData[i];
                WriteBytes(context.stream, BitConverter.GetBytes(rankData.Attribute1));
                WriteBytes(context.stream, BitConverter.GetBytes(rankData.Attribute2));
                WriteBytes(context.stream, BitConverter.GetBytes(rankData.FirstSkill));
                WriteBytes(context.stream, BitConverter.GetBytes(rankData.SecondSkill));
                WriteBytes(context.stream, BitConverter.GetBytes(rankData.Faction));
            }

            for (var i = 0; i < Constants.FACT_MAX_SKILL_IDS; ++i)
            {
                WriteBytes(context.stream, BitConverter.GetBytes(factionData.SkillIds[i]));
            }

            WriteBytes(context.stream, BitConverter.GetBytes(factionData.Flags));
			return null;
		}
		
		[HandlesOperation(SubRecordOperationType.AddIndex)]
		internal static object GetAddIndex(SubRecordAddIndexContext context)
		{
            return GetAddIndexOrdered(context.record, "FADT", Names.InitialSubRecordOrder);
		}
		
	}
	
}
