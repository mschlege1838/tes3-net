
using System;

namespace TES3.Records.SubRecords
{
    public class IngredientData : SubRecord
    {
        public float Weight { get; set; }
        public int Value { get; set; }
        int[] effectIds;
        int[] skillIds;
        int[] attributeIds;

        public IngredientData(string name, float weight, int value, int[] effectIds, int[] skillIds, int[] attributeIds) : base(name)
        {
            Weight = weight;
            Value = value;
            EffectIds = effectIds;
            SkillIds = skillIds;
            AttributeIds = attributeIds;
        }

        public int[] EffectIds
        {
            get => effectIds;
            set => effectIds = value ?? throw new ArgumentNullException("effectIds", "Effect Ids cannot be null.");
        }
        public int[] SkillIds
        {
            get => skillIds;
            set => skillIds = value ?? throw new ArgumentNullException("skillIds", "Skill Ids cannot be null.");
        }
        public int[] AttributeIds
        {
            get => attributeIds;
            set => attributeIds = value ?? throw new ArgumentNullException("attributeIds", "Attribute Ids cannot be null.");
        }

        public override string ToString()
        {
            return $"{Name} (wght, val)({Weight}, {Value})";
        }
    }


}