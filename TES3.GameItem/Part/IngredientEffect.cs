using TES3.GameItem.TypeConstant;
using TES3.Util;

namespace TES3.GameItem.Part
{
    public class IngredientEffect : ICopyable<IngredientEffect>
    {
        MagicEffectType effect;
        AttributeType affectedAttribute;
        SkillType affectedSkill;

        public IngredientEffect(MagicEffectType effect) : this(effect, AttributeType.None, SkillType.None)
        {

        }

        public IngredientEffect(MagicEffectType effect, AttributeType affectedAttribute) : this(effect, affectedAttribute, SkillType.None)
        {

        }

        public IngredientEffect(MagicEffectType effect, SkillType affectedSkill) : this(effect, AttributeType.None, affectedSkill)
        {

        }

        internal IngredientEffect(MagicEffectType effect, AttributeType affectedAttribute, SkillType affectedSkill)
        {
            Effect = effect;
            AffectedAttribute = affectedAttribute;
            AffectedSkill = affectedSkill;
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

        public IngredientEffect Copy()
        {
            return new IngredientEffect(effect, affectedAttribute, affectedSkill);
        }
    }
}
