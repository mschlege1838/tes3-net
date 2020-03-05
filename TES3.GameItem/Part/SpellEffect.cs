using System;
using TES3.GameItem.TypeConstant;
using TES3.Records.SubRecords;
using TES3.Util;

namespace TES3.GameItem.Part
{


    public class SpellEffect : ICopyable<SpellEffect>
    {

        MagicEffectType effect;
        SkillType affectedSkill;
        AttributeType affectedAttribute;
        int area;
        int duration;
        int minMagnitude;
        int maxMagnitude;

        public SpellEffect(MagicEffectType type, RangeType range)
        {
            Effect = type;
            Range = range;
        }

        public SpellEffect(MagicEffectType type, RangeType range, SkillType affectedSkill)
        {
            Effect = type;
            Range = range;
            AffectedSkill = affectedSkill;
        }

        public SpellEffect(MagicEffectType type, RangeType range, AttributeType affectedAttribute)
        {
            Effect = type;
            Range = range;
            AffectedAttribute = affectedAttribute;
        }

        internal SpellEffect(MagicEffectType effect, SkillType affectedSkill, AttributeType affectedAttribute, RangeType range, int area, int duration, int minMagnitude, int maxMagnitude)
        {
            this.effect = effect;
            this.affectedSkill = affectedSkill;
            this.affectedAttribute = affectedAttribute;
            Range = range;
            this.area = area;
            this.duration = duration;
            this.minMagnitude = minMagnitude;
            this.maxMagnitude = maxMagnitude;
        }

        internal SpellEffect(SpellItemData subRecord)
        {
            effect = (MagicEffectType) subRecord.EffectId;
            affectedSkill = effect.ByteConvertToSkill(subRecord.SkillId);
            affectedAttribute = effect.ByteConvertToAttribute(subRecord.AttributeId);
            Range = (RangeType) subRecord.RangeType;
            area = subRecord.Area;
            duration = subRecord.Duration;
            minMagnitude = subRecord.MagMin;
            maxMagnitude = subRecord.MagMax;
        }

        public MagicEffectType Effect
        {
            get => effect;
            set
            {
                if (!value.AffectsAttribute())
                {
                    affectedAttribute = AttributeType.None;
                }
                if (!value.AffectsSkill())
                {
                    affectedSkill = SkillType.None;
                }
                effect = value;
            }
        }

        public SkillType AffectedSkill
        {
            get => affectedSkill;
            set
            {
                Effect.CheckSkill(value);
                affectedSkill = value;
            }
        }

        public AttributeType AffectedAttribute
        {
            get => affectedAttribute;
            set
            {
                Effect.CheckAttribute(value);
                affectedAttribute = value;
            }
        }

        public RangeType Range
        {
            get;
            set;
        }

        public int Area
        {
            get => area;
            set => area = value < 0 ? throw new ArgumentOutOfRangeException("value", value, $"Area cannot be negative: {value}") : value;
        }

        public int Duration
        {
            get => duration;
            set => duration = value < 0 ? throw new ArgumentOutOfRangeException("value", value, $"Duration cannot be negative: {value}") : value;
        }

        public int MinMagnitude
        {
            get => minMagnitude;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value", value, $"Min Magnnitude cannot be negative: {value}");
                }
                if (value > MaxMagnitude)
                {
                    throw new ArgumentOutOfRangeException("value", value, $"Min Magnitude cannot be greater than Max Magnitude ({MaxMagnitude}): {value}");
                }
                minMagnitude = value;
            }
        }

        public int MaxMagnitude
        {
            get => maxMagnitude;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value", value, $"Max Magnitude cannot be negative: {value}");
                }
                if (value < MinMagnitude)
                {
                    throw new ArgumentOutOfRangeException("value", value, $"Max Magnitude cannot be less than Min Magnitude ({MinMagnitude}): {value}");
                }
                maxMagnitude = value;
            }
        }



        public SpellItemData ToSubRecord()
        {
            var effectId = (short) Effect;
            var skillId = Effect.ByteConvertToSkillId(AffectedSkill);
            var attributeId = Effect.ByteConvertToAttributeId(AffectedAttribute);
            var rangeType = (int) Range;

            return new SpellItemData("ENAM", effectId, skillId, attributeId, rangeType, Area, Duration, MinMagnitude, MaxMagnitude);
        }

        public SpellEffect Copy()
        {
            return new SpellEffect(effect, affectedSkill, affectedAttribute, Range, area, duration, minMagnitude, maxMagnitude);
        }


    }

}
