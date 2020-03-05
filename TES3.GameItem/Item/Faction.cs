using System;
using System.Collections.Generic;
using System.IO;
using TES3.Core;
using TES3.GameItem.Part;
using TES3.GameItem.TypeConstant;
using TES3.Records;
using TES3.Records.SubRecords;
using TES3.Util;

namespace TES3.GameItem.Item
{

    [TargetRecord("FACT")]
    public class Faction : TES3GameItem
    {

        string displayName;

        public Faction(string name) : base(name)
        {
            
        }

        public Faction(Record record) : base(record)
        {
            
        }

        public override string RecordName => "FACT";
        
        [IdField]
        public string Name
        {
            get => (string) Id;
            set => Id = value;
        }

        public string DisplayName
        {
            get => displayName;
            set => displayName = Validation.NotNull(value, "value", "Display Name");
        }

        public AttributeType FavoredAttribute1
        {
            get;
            set;
        }

        public AttributeType FavoredAttribute2
        {
            get;
            set;
        }

        public bool Hidden
        {
            get;
            set;
        }

        public bool Deleted
        {
            get;
            set;
        }

        public IList<FactionRank> Ranks
        {
            get;
        } = new MaxCapacityList<FactionRank>(Constants.FACT_MAX_RANKS, "Ranks");

        public IList<SkillType> FavoredSkills
        {
            get;
        } = new MaxCapacityList<SkillType>(Constants.FACT_MAX_SKILL_IDS, "Favored Skills");

        public IList<FactionReaction> FactionReactions
        {
            get;
        } = new List<FactionReaction>();

        protected override void OnCreate(IList<SubRecord> subRecords)
        {
            subRecords.Add(new StringSubRecord("NAME", Name));
            subRecords.Add(new StringSubRecord("FNAM", DisplayName));

            foreach (var rank in Ranks)
            {
                subRecords.Add(new StringSubRecord("RNAM", rank.Name));
            }


            var skillIds = new int[Constants.FACT_MAX_SKILL_IDS];
            ProcessSkills(skillIds);
            var rankData = new RankData[Constants.FACT_MAX_RANKS];
            var rankIndex = 0;
            foreach (var rank in Ranks)
            {
                rankData[rankIndex++] = new RankData(rank.Attribute1Requirement, rank.Attribute2Requirement, rank.PrimarySkillRequirement, rank.SecondarySkillsRequirement, rank.FactionReactionValue);
            }
            for (; rankIndex < Constants.FACT_MAX_RANKS; ++rankIndex)
            {
                rankData[rankIndex++] = new RankData(0, 0, 0, 0, 0);
            }
            subRecords.Add(new FactionData("FADT", (int) FavoredAttribute1, (int) FavoredAttribute2, rankData, skillIds, Hidden ? 1 : 0));

        }

        protected override void UpdateRequired(Record record)
        {
            record.GetSubRecord<StringSubRecord>("NAME").Data = Name;
            record.GetSubRecord<StringSubRecord>("FNAM").Data = DisplayName;

            var factionData = record.GetSubRecord<FactionData>("FADT");
            factionData.Attribute1 = (int) FavoredAttribute1;
            factionData.Attribute2 = (int) FavoredAttribute2;
            factionData.Flags = Hidden ? 1 : 0;

            ProcessSkills(factionData.SkillIds);

            // Ranks
            {
                record.RemoveAllSubRecords("RNAM");
                var targetIndex = record.GetAddIndex("RNAM");
                int rankIndex = 0;
                foreach (var rank in Ranks)
                {
                    record.InsertSubRecordAt(targetIndex++, new StringSubRecord("RNAM", rank.Name));

                    var rankData = factionData.RankData[rankIndex++];
                    rankData.Attribute1 = rank.Attribute1Requirement;
                    rankData.Attribute2 = rank.Attribute2Requirement;
                    rankData.FirstSkill = rank.PrimarySkillRequirement;
                    rankData.SecondSkill = rank.SecondarySkillsRequirement;
                    rankData.Faction = rank.FactionReactionValue;
                }
                for (; rankIndex < Constants.FACT_MAX_RANKS; ++rankIndex)
                {
                    var rankData = factionData.RankData[rankIndex++];
                    rankData.Attribute1 = 0;
                    rankData.Attribute2 = 0;
                    rankData.FirstSkill = 0;
                    rankData.SecondSkill = 0;
                    rankData.Faction = 0;
                }
            }

        }

        void ProcessSkills(int[] skillIds)
        {
            for (var i = 0; i < Constants.FACT_MAX_SKILL_IDS; ++i)
            {
                skillIds[i] = i < FavoredSkills.Count ? (int) FavoredSkills[i] : -1;
            }
        }


        protected override void UpdateOptional(Record record)
        {
            ProcessOptional(record, "DELE", Deleted, () => new IntSubRecord("DELE", 0), (sr) => sr.Data = 0);

            UpdateCollection(record, FactionReactions, "ANAM", new string[] { "ANAM", "INTV" },
                delegate (ref int index, FactionReaction item)
                {
                    record.InsertSubRecordAt(index++, new StringSubRecord("ANAM", item.Name));
                    record.InsertSubRecordAt(index++, new IntSubRecord("INTV", item.Reaction));
                }
            );

        }

        protected override void DoSyncWithRecord(Record record)
        {
            Id = record.GetSubRecord<StringSubRecord>("NAME").Data;
            DisplayName = record.GetSubRecord<StringSubRecord>("FNAM").Data;

            var factionData = record.GetSubRecord<FactionData>("FADT");
            FavoredAttribute1 = (AttributeType) factionData.Attribute1;
            FavoredAttribute2 = (AttributeType) factionData.Attribute2;
            Hidden = factionData.Flags == 1;
            


            // Optional
            Deleted = record.ContainsSubRecord("DELE");


            // Collection
            FavoredSkills.Clear();
            var skillIds = factionData.SkillIds;
            for (var i = 0; i < Constants.FACT_MAX_SKILL_IDS; ++i)
            {
                var skillId = skillIds[i];
                if (skillId == -1)
                {
                    break;
                }
                FavoredSkills.Add((SkillType) skillId);
            }

            Ranks.Clear();
            {
                var rankIndex = 0;
                foreach (StringSubRecord rankName in record.GetEnumerableFor("RNAM"))
                {
                    var rankData = factionData.RankData[rankIndex++];
                    Ranks.Add(new FactionRank(rankName.Data, rankData.Attribute1, rankData.Attribute2, rankData.FirstSkill,
                        rankData.SecondSkill, rankData.Faction));
                }
            }

            FactionReactions.Clear();
            {
                var enumerator = record.GetEnumerableFor("ANAM", "INTV");
                while (enumerator.MoveNext())
                {
                    var name = ((StringSubRecord) enumerator.Current).Data;

                    if (!enumerator.MoveNext())
                    {
                        throw new InvalidOperationException("Expected INTV record after ANAM.");
                    }

                    var reaction = ((IntSubRecord) enumerator.Current).Data;

                    FactionReactions.Add(new FactionReaction(name, reaction));
                }
            }

        }

        protected override void DoValidateRecord(Record record, Validator validator)
        {
            validator.CheckRequired(record, "NAME");
            validator.CheckRequired(record, "FNAM");
            validator.CheckRequired(record, "FADT");
            validator.CheckCount(record, "RNAM", Constants.FACT_MAX_RANKS);
        }

        public override TES3GameItem Clone()
        {
            var result = new Faction(Name)
            {
                DisplayName = DisplayName,
                FavoredAttribute1 = FavoredAttribute1,
                FavoredAttribute2 = FavoredAttribute2,
                Hidden = Hidden,
                Deleted = Deleted
            };

            CollectionUtils.Copy(Ranks, result.Ranks);
            CollectionUtils.Copy(FavoredSkills, result.FavoredSkills);
            CollectionUtils.Copy(FactionReactions, result.FactionReactions);

            return result;
        }

        public override void StreamDebug(TextWriter target)
        {
            var writer = new IndentWriter(target);

            writer.WriteLine(ToString());

            writer.IncIndent();
            writer.WriteLine($"Name: {DisplayName}");
            writer.WriteLine($"Hidden: {Hidden}");
            writer.WriteLine($"Favored Attribute 1: {FavoredAttribute1}");
            writer.WriteLine($"Favored Attribute 2: {FavoredAttribute2}");

            writer.WriteLine("Favored Skills");
            writer.IncIndent();
            foreach (var skill in FavoredSkills)
            {
                writer.WriteLine(skill);
            }
            writer.DecIndent();

            if (Ranks.Count > 0)
            {
                writer.WriteLine("Ranks");
                writer.IncIndent();
                {
                    var table = new Table("Name", "Attribute 1 Requirement", "Attribute 2 Requirement", "Primary Skill Requirement", "Secondary Skills Requirement", "Reaction Value");
                    foreach (var rank in Ranks)
                    {
                        table.AddRow(rank.Name, rank.Attribute1Requirement, rank.Attribute2Requirement, rank.PrimarySkillRequirement, rank.SecondarySkillsRequirement, rank.FactionReactionValue);
                    }
                    table.Print(writer);
                }
                writer.DecIndent();
            }

            if (FactionReactions.Count > 0)
            {
                writer.WriteLine("Faction Reactions");
                writer.IncIndent();
                {
                    var table = new Table("Faction", "Reaction Value");
                    foreach (var reaction in FactionReactions)
                    {
                        table.AddRow(reaction.Name, reaction.Reaction);
                    }
                    table.Print(writer);
                }
                writer.DecIndent();
            }

            writer.DecIndent();
        }

        public override string ToString()
        {
            return $"Faction ({Name})";
        }
    }

}
