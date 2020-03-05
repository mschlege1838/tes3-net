using TES3.Util;

namespace TES3.GameItem.Part
{
    public class FactionRank : ICopyable<FactionRank>
    {

        public FactionRank(string name, int attribute1Requirement, int attribute2Requirement, int primarySkillRequirement,
                int secondarySkillRequirement, int factionReactionValue)
        {
            Name = name;
            Attribute1Requirement = attribute1Requirement;
            Attribute2Requirement = attribute2Requirement;
            PrimarySkillRequirement = primarySkillRequirement;
            SecondarySkillsRequirement = secondarySkillRequirement;
            FactionReactionValue = factionReactionValue;
        }

        public string Name
        {
            get;
            set;
        }

        public int Attribute1Requirement
        {
            get;
            set;
        }

        public int Attribute2Requirement
        {
            get;
            set;
        }

        public int PrimarySkillRequirement
        {
            get;
            set;
        }

        public int SecondarySkillsRequirement
        {
            get;
            set;
        }

        public int FactionReactionValue
        {
            get;
            set;
        }

        public FactionRank Copy()
        {
            return new FactionRank(Name, Attribute1Requirement, Attribute2Requirement, PrimarySkillRequirement, SecondarySkillsRequirement, FactionReactionValue);
        }
    }

}
