
using System;

using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.NPC_.NPDT 
{
	
	[HandlesSubRecord("NPC_", "NPDT")]
    static class Operations
	{
		[HandlesOperation(SubRecordOperationType.Read)]
		internal static object Read(SubRecordReadContext context)
		{
            switch (context.header.Size)
            {
                case 52:
                {
                    var pos = 0;

                    var level = BitConverter.ToInt16(context.buf, pos); pos += 2;
                    var strength = context.buf[pos++];
                    var intelligence = context.buf[pos++];
                    var willpower = context.buf[pos++];
                    var agility = context.buf[pos++];
                    var speed = context.buf[pos++];
                    var endurance = context.buf[pos++];
                    var personality = context.buf[pos++];
                    var luck = context.buf[pos++];

                    var skills = new byte[27];
                    Array.Copy(context.buf, pos, skills, 0, 27); pos += 27;

                    var reputation = context.buf[pos++];
                    var health = BitConverter.ToInt16(context.buf, pos); pos += 2;
                    var spellPoints = BitConverter.ToInt16(context.buf, pos); pos += 2;
                    var fatigue = BitConverter.ToInt16(context.buf, pos); pos += 2;
                    var disposition = context.buf[pos++];
                    var factionID = context.buf[pos++];
                    var rank = context.buf[pos++];
                    var unknown = context.buf[pos++];
                    var gold = BitConverter.ToInt32(context.buf, pos); pos += 4;

                    return new NPCData52(context.SubRecordName, level, strength, intelligence, willpower, agility, speed,
                            endurance, personality, luck, skills, reputation, health, spellPoints, fatigue,
                            disposition, factionID, rank, unknown, gold);
                }
                case 12:
                {
                    var pos = 0;

                    var level = BitConverter.ToInt16(context.buf, pos); pos += 2;
                    var disposition = context.buf[pos++];
                    var factionID = context.buf[pos++];
                    var rank = context.buf[pos++];
                    var unknown = context.buf[pos++];
                    var unknown1 = context.buf[pos++];
                    var unknown2 = context.buf[pos++];
                    var gold = BitConverter.ToInt32(context.buf, pos); pos += 4;

                    return new NPCData12(context.SubRecordName, level, disposition, factionID, rank, unknown, unknown1, unknown2, gold);
                }
                default:
                    throw new InvalidOperationException($"Unexpected size for NPDT: {context.header.Size}");
            }
		}
		
		[HandlesOperation(SubRecordOperationType.Size)]
		internal static object GetSize(SubRecordLengthContext context)
		{
            return context.subRecord is NPCData12 ? 12 : 52;
		}
		
		[HandlesOperation(SubRecordOperationType.Write)]
		internal static object Write(SubRecordWriteContext context)
		{
            if (context.subRecord is NPCData52)
            {
                var npcData = (NPCData52) context.subRecord;

                WriteBytes(context.stream, BitConverter.GetBytes(npcData.Level));
                context.stream.WriteByte(npcData.Strength);
                context.stream.WriteByte(npcData.Intelligence);
                context.stream.WriteByte(npcData.Willpower);
                context.stream.WriteByte(npcData.Agility);
                context.stream.WriteByte(npcData.Speed);
                context.stream.WriteByte(npcData.Endurance);
                context.stream.WriteByte(npcData.Personality);
                context.stream.WriteByte(npcData.Luck);

                WriteBytes(context.stream, npcData.Skills);

                context.stream.WriteByte(npcData.Reputation);
                WriteBytes(context.stream, BitConverter.GetBytes(npcData.Health));
                WriteBytes(context.stream, BitConverter.GetBytes(npcData.SpellPoints));
                WriteBytes(context.stream, BitConverter.GetBytes(npcData.Fatigue));
                context.stream.WriteByte(npcData.Disposition);
                context.stream.WriteByte(npcData.FactionId);
                context.stream.WriteByte(npcData.Rank);
                context.stream.WriteByte(npcData.Unknown);
                WriteBytes(context.stream, BitConverter.GetBytes(npcData.Gold));

            }
            else if (context.subRecord is NPCData12)
            {
                var npcData = (NPCData12) context.subRecord;

                WriteBytes(context.stream, BitConverter.GetBytes(npcData.Level));
                context.stream.WriteByte(npcData.Disposition);
                context.stream.WriteByte(npcData.FactionId);
                context.stream.WriteByte(npcData.Rank);
                context.stream.WriteByte(npcData.Unknown);
                context.stream.WriteByte(npcData.Unknown1);
                context.stream.WriteByte(npcData.Unknown2);
                WriteBytes(context.stream, BitConverter.GetBytes(npcData.Gold));
            }
			return null;
		}
		
		[HandlesOperation(SubRecordOperationType.AddIndex)]
		internal static object GetAddIndex(SubRecordAddIndexContext context)
		{
            return GetAddIndexOrdered(context.record, "NPDT", Names.InitialSubRecordOrder);
		}
		
	}
	
}
