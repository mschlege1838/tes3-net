using TES3.GameItem.TypeConstant;

namespace TES3.GameItem.Part
{
    public class SkillUsage
    {
        public SkillUsage(SkillUsageType usageType, float usageValue)
        {
            UsageType = usageType;
            UsageValue = usageValue;
        }

        public SkillUsageType UsageType { get; }

        public float UsageValue
        {
            get;
            set;
        }

    }


}
