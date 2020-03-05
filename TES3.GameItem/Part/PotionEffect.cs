
using TES3.Records.SubRecords;
using TES3.GameItem.TypeConstant;
using TES3.Util;

namespace TES3.GameItem.Part
{
    public class PotionEffect : ICopyable<PotionEffect>
    {

        MagicEffectType effect;
        AttributeType affectedAttribute;
        SkillType affectedSkill;

        public PotionEffect(MagicEffectType effect)
        {
            Effect = effect;
        }

        public PotionEffect(MagicEffectType effect, AttributeType affectedAttribute)
        {
            Effect = effect;
            AffectedAttribute = affectedAttribute;
        }

        public PotionEffect(MagicEffectType effect, SkillType affectedSkill)
        {
            Effect = effect;
            AffectedSkill = affectedSkill;
        }

        internal PotionEffect(MagicEffectType effect, AttributeType affectedAttribute, SkillType affectedSkill, int duration, int magnitude)
        {
            this.effect = effect;
            this.affectedAttribute = affectedAttribute;
            this.affectedSkill = affectedSkill;
            Duration = duration;
            Magnitude = magnitude;
        }

        internal PotionEffect(SpellItemData subRecord)
        {
            effect = (MagicEffectType) subRecord.EffectId;
            affectedAttribute = effect.ByteConvertToAttribute(subRecord.AttributeId);
            affectedSkill = effect.ByteConvertToSkill(subRecord.SkillId);
            Duration = subRecord.Duration;
            Magnitude = subRecord.MagMin;
        }

        public MagicEffectType Effect
        {
            get => effect;
            set
            {
                if (!effect.AffectsAttribute())
                {
                    affectedAttribute = AttributeType.None;
                }
                if (!effect.AffectsSkill())
                {
                    affectedSkill = SkillType.None;
                }
                effect = value;
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

        public SkillType AffectedSkill
        {
            get => affectedSkill;
            set
            {
                Effect.CheckSkill(value);
                affectedSkill = value;
            }
        }

        public int Duration
        {
            get;
            set;
        }

        public int Magnitude
        {
            get;
            set;
        }

        public PotionEffect Copy()
        {
            return new PotionEffect(effect, affectedAttribute, affectedSkill, Duration, Magnitude);
        }
    }
}
