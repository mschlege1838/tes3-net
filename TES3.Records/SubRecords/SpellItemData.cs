namespace TES3.Records.SubRecords
{
    public class SpellItemData : SubRecord
    {
        public short EffectId { get; set; }
        public byte SkillId { get; set; }
        public byte AttributeId { get; set; }
        public int RangeType { get; set; }
        public int Area { get; set; }
        public int Duration { get; set; }
        public int MagMin { get; set; }
        public int MagMax { get; set; }

        public SpellItemData(string name, short effectId, byte skillId, byte attributeId,
                int rangeType, int area, int duration, int magMin, int magMax) : base(name)
        {
            EffectId = effectId;
            SkillId = skillId;
            AttributeId = attributeId;
            RangeType = rangeType;
            Area = area;
            Duration = duration;
            MagMin = magMin;
            MagMax = magMax;
        }

        public override string ToString()
        {
            return $"{Name} effect,skill,attr({EffectId}, {SkillId}, {AttributeId}) cast,area,duration({RangeType}, {Area}, {Duration}) magnitude({MagMin} - {MagMax})";
        }
    }

}