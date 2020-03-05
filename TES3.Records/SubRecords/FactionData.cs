
using System;

namespace TES3.Records.SubRecords
{
    public class FactionData : SubRecord
    {
        
        RankData[] rankData;
        int[] skillIds;
        
        public FactionData(string name, int attribute1, int attribute2, RankData[] rankData, int[] skillIds, int flags) : base(name)
        {
            Attribute1 = attribute1;
            Attribute2 = attribute2;
            RankData = rankData;
            SkillIds = skillIds;
            Flags = flags;
        }

        public int Attribute1
        {
            get;
            set;
        }

        public int Attribute2
        {
            get;
            set;
        }

        public RankData[] RankData
        {
            get => rankData;
            set => rankData = value ?? throw new ArgumentNullException("value", "Rank Data cannot be null.");
        }

        public int[] SkillIds
        {
            get => skillIds;
            set => skillIds = value ?? throw new ArgumentNullException("value", "Skill IDs cannot be null.");
        }

        public int Flags
        {
            get;
            set;
        }

        public override string ToString()
        {
            return $"{Name} attributes({Attribute1}, {Attribute2}) skills({string.Join(", ", SkillIds)}) flags({Flags:X8}) ranks({RankData.Length})";
        }

    }

    public class RankData
    {
        

        public RankData(int attribute1, int attribute2, int firstSkill, int secondSkill, int faction)
        {
            Attribute1 = attribute1;
            Attribute2 = attribute2;
            FirstSkill = firstSkill;
            SecondSkill = secondSkill;
            Faction = faction;
        }

        public int Attribute1
        {
            get;
            set;
        }

        public int Attribute2
        {
            get;
            set;
        }

        public int FirstSkill
        {
            get;
            set;
        }

        public int SecondSkill
        {
            get;
            set;
        }

        public int Faction
        {
            get;
            set;
        }

        public override string ToString()
        {
            return $"atr2,atr2,sk1,sk2,rct_mod({Attribute1}, {Attribute2}, {FirstSkill}, {SecondSkill}, {Faction})";
        }

    }
}