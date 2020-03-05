using TES3.GameItem.TypeConstant;
using TES3.Util;

namespace TES3.GameItem.Part
{
    public class RaceSkillBonus : ICopyable<RaceSkillBonus>
    {
        public RaceSkillBonus(SkillType skill, int bonus)
        {
            Skill = skill;
            Bonus = bonus;
        }

        public SkillType Skill
        {
            get;
            set;
        }

        public int Bonus
        {
            get;
            set;
        }

        public RaceSkillBonus Copy()
        {
            return new RaceSkillBonus(Skill, Bonus);
        }
    }

}
