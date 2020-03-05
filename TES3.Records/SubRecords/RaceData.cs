
using System;

namespace TES3.Records.SubRecords
{
    public class RaceData : SubRecord
    {
        SkillBonus[] skillBonuses;
        int[] strength;
        int[] intelligence;
        int[] willpower;
        int[] agility;
        int[] speed;
        int[] endurance;
        int[] personality;
        int[] luck;
        float[] height;
        float[] weight;
        public int Flags { get; set; }

        public RaceData(string name, SkillBonus[] skillBonuses, int[] strength, int[] intelligence,
                int[] willpower, int[] agility, int[] speed, int[] endurance, int[] personality, int[] luck,
                float[] height, float[] weight, int flags) : base(name)
        {
            SkillBonuses = skillBonuses;
            Strength = strength;
            Intelligence = intelligence;
            Willpower = willpower;
            Agility = agility;
            Speed = speed;
            Endurance = endurance;
            Personality = personality;
            Luck = luck;
            Height = height;
            Weight = weight;
            Flags = flags;
        }

        public SkillBonus[] SkillBonuses
        {
            get => skillBonuses;
            set => skillBonuses = value ?? throw new ArgumentNullException("skillBonuses", "Skill Bonuses cannot be null.");
        }
        public int[] Strength
        {
            get => strength;
            set => strength = value ?? throw new ArgumentNullException("strength", "Strength Bonus cannot be null.");
        }
        public int[] Intelligence
        {
            get => intelligence;
            set => intelligence = value ?? throw new ArgumentNullException("intelligence", "Intelligence Bonus cannot be null.");
        }
        public int[] Willpower
        {
            get => willpower;
            set => willpower = value ?? throw new ArgumentNullException("willpower", "Willpower Bonus cannot be null.");
        }
        public int[] Agility
        {
            get => agility;
            set => agility = value ?? throw new ArgumentNullException("agility", "Agility Bonus cannot be null.");
        }
        public int[] Speed
        {
            get => speed;
            set => speed = value ?? throw new ArgumentNullException("speed", "Speed Bonus cannot be null.");
        }
        public int[] Endurance
        {
            get => endurance;
            set => endurance = value ?? throw new ArgumentNullException("endurance", "Endurance Bonus cannot be null.");
        }
        public int[] Personality
        {
            get => personality;
            set => personality = value ?? throw new ArgumentNullException("personality", "Personality Bonus cannot be null.");
        }
        public int[] Luck
        {
            get => luck;
            set => luck = value ?? throw new ArgumentNullException("luck", "Luck Bonus cannot be null.");
        }
        public float[] Height
        {
            get => height;
            set => height = value ?? throw new ArgumentNullException("height", "Height cannot be null.");
        }
        public float[] Weight
        {
            get => weight;
            set => weight = value ?? throw new ArgumentNullException("weight", "Weight cannot be null.");
        }

        public override string ToString()
        {
            return $"{Name} male/female:st,i,wi,a,sp,e,p,l,h,wht ({strength[0]}, {intelligence[0]}, {willpower[0]}, {agility[0]}, {speed[0]}, {endurance[0]}, {personality[0]}, {luck[0]}, {height[0]}, {weight[0]})/" +
                $"({strength[1]}, {intelligence[1]}, {willpower[1]}, {agility[1]}, {speed[1]}, {endurance[1]}, {personality[1]}, {luck[1]}, {height[1]}, {weight[1]})";
        }
    }


    public class SkillBonus
    {
        

        public SkillBonus(int skillId, int bonus)
        {
            SkillId = skillId;
            Bonus = bonus;
        }

        public int SkillId
        {
            get;
            set;
        }

        public int Bonus
        {
            get;
            set;
        }

        public override string ToString()
        {
            return $"skil,bonus({SkillId}, {Bonus})";
        }
    }

}