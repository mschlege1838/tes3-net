
using System;

namespace TES3.GameItem.TypeConstant
{

    public enum MagicEffectType
    {
        AbsorbAttribute = 85
        , AbsorbFatigue = 88
        , AbsorbHealth = 86
        , AbsorbMagicka = 87
        , AbsorbSkill = 89
        , AlmsiviIntervention = 63
        , Blind = 47
        , BoundBattleAxe = 123
        , BoundBoots = 129
        , BoundCuirass = 127
        , BoundDagger = 120
        , BoundGloves = 131
        , BoundHelm = 128
        , BoundLongbow = 125
        , BoundLongsword = 121
        , BoundMace = 122
        , BoundShield = 130
        , BoundSpear = 124
        , Burden = 7
        , CalmCreature = 50
        , CalmHumanoid = 49
        , CallBear = 139
        , CallWolf = 138
        , Chameleon = 40
        , Charm = 44
        , CommandCreature = 118
        , CommandHumanoid = 119
        , Corprus = 132
        , CureBlightDisease = 70
        , CureCommonDisease = 69
        , CureCorprusDisease = 71
        , CureParalyzation = 73
        , CurePoison = 72
        , DamageAttribute = 22
        , DamageFatigue = 25
        , DamageHealth = 23
        , DamageMagicka = 24
        , DamageSkill = 26
        , DemoralizeCreature = 54
        , DemoralizeHumanoid = 53
        , DetectAnimal = 64
        , DetectEnchantment = 65
        , DetectKey = 66
        , DisintegrateArmor = 38
        , DisintegrateWeapon = 37
        , Dispel = 57
        , DivineIntervention = 62
        , DrainAttribute = 17
        , DrainFatigure = 20
        , DrainHealth = 18
        , DrainMagicka = 19
        , DrainSkill = 21
        , EXTRA_SPELL = 126
        , Feather = 8
        , FireDamage = 14
        , FireShield = 4
        , FortifyAttack = 117
        , FortifyAttribute = 79
        , FortifyFatigue = 82
        , FortifyHealth = 80
        , FortifyMagicka = 81
        , FortifyMaximumMagicka = 84
        , FortifySkill = 83
        , FrenzyCreature = 52
        , FrenzyHumanoid = 51
        , FrostDamage = 16
        , FrostShield = 6
        , Invisiblity = 39
        , Jump = 9
        , Levitate = 10
        , Light = 41
        , LightningShield = 5
        , Lock = 12
        , Mark = 60
        , NightEye = 43
        , Open = 13
        , Paralyze = 45
        , Poison = 27
        , RallyCreature = 56
        , RallyHumanoid = 55
        , Recall = 61
        , Reflect = 68
        , RemoveCurse = 100
        , ResistBlightDisease = 95
        , ResistCommonDisease = 94
        , ResistCorprusDisease = 96
        , ResistFire = 90
        , ResistFrost = 91
        , ResistMagicka = 93
        , ResistNormalWeapons = 98
        , ResistParalysis = 99
        , ResistPoison = 97
        , ResistShock = 92
        , RestoreAttribute = 74
        , RestoreFatigue = 77
        , RestoreHealth = 75
        , RestoreMagicka = 76
        , RestoreSkill = 78
        , Sancturary = 42
        , sEffectSummonCreature04 = 141
        , sEffectSummonCreature05 = 142
        , Shield = 3
        , ShockDamage = 15
        , Silence = 46
        , SlowFall = 11
        , Soultrap = 58
        , Sound = 48
        , SpellAbsorption = 67
        , StuntedMagicka = 136
        , SummonAncestralGhost = 106
        , SummonBonelord = 110
        , SummonBonewalker = 108
        , SummonBoneWolf = 140
        , SummonCenturionSphere = 134
        , SummonClannfear = 103
        , SummonDaedroth = 104
        , SummonDremora = 105
        , SummonFabricant = 137
        , SummonFlameAtronach = 114
        , SummonFrostAtronach = 115
        , SummonGoldenSaint = 113
        , SummonGreaterBonewalker = 109
        , SummonHunger = 112
        , SummonScamp = 102
        , SummonSkeletalMinion = 107
        , SummonStormAtronach = 116
        , SummonWingedTwilight = 111
        , SunDamage = 135
        , SwitftSwim = 1
        , Telekinesis = 59
        , TurnUndead = 101
        , Vampirism = 133
        , WaterBreathing = 0
        , WaterWalking = 2
        , WeaknessToBlightDisease = 33
        , WeaknessToCommonDisease = 32
        , WeaknessToCorprusDisease = 34
        , WeaknessToFire = 28
        , WeaknessToFrost = 29
        , WeaknessToMagicka = 31
        , WeaknessToNormalWeapons = 36
        , WeaknessToPoison = 35
        , WeaknessToShock = 30
    }


    public static class MagicEffectExtensions
    {
        const byte BYTE_AFFECTED_ITEM_NONE = 255;
        const int INT_AFFECTED_ITEM_NONE = -1;

        public static bool AffectsAttribute(this MagicEffectType effect)
        {
            switch (effect)
            {
                case MagicEffectType.FortifyAttribute:
                case MagicEffectType.DrainAttribute:
                case MagicEffectType.AbsorbAttribute:
                case MagicEffectType.DamageAttribute:
                case MagicEffectType.RestoreAttribute:
                    return true;
                default:
                    return false;
            }
        }

        public static void CheckAttribute(this MagicEffectType effect, AttributeType attribute)
        {
            if (!effect.AffectsAttribute() && attribute != AttributeType.None)
            {
                throw new InvalidOperationException($"This effect does not affect an Attribute: {effect}");
            }
            if (attribute == AttributeType.None && effect.AffectsAttribute())
            {
                throw new InvalidOperationException($"Affected attribute required for this effect: {effect}");
            }
        }

        public static AttributeType ByteConvertToAttribute(this MagicEffectType effect, byte id)
        {
            return effect.AffectsAttribute() ? (AttributeType) id : AttributeType.None;
        }

        public static byte ByteConvertToAttributeId(this MagicEffectType effect, AttributeType attribute)
        {
            return effect.AffectsAttribute() ? (byte) attribute : BYTE_AFFECTED_ITEM_NONE;
        }

        public static AttributeType IntConvertToAttribute(this MagicEffectType effect, int id)
        {
            return effect.AffectsAttribute() ? (AttributeType) id : AttributeType.None;
        }

        public static int IntConvertToAttributeId(this MagicEffectType effect, AttributeType attribute)
        {
            return effect.AffectsAttribute() ? (int) attribute : INT_AFFECTED_ITEM_NONE;
        }


        public static bool AffectsSkill(this MagicEffectType effect)
        {
            switch (effect)
            {
                case MagicEffectType.FortifySkill:
                case MagicEffectType.DrainSkill:
                case MagicEffectType.AbsorbSkill:
                case MagicEffectType.DamageSkill:
                case MagicEffectType.RestoreSkill:
                    return true;
                default:
                    return false;
            }
        }

        public static void CheckSkill(this MagicEffectType effect, SkillType skill)
        {
            if (!effect.AffectsSkill() && skill != SkillType.None)
            {
                throw new InvalidOperationException($"This effect does not affect a Skill: {effect}");
            }
            if (skill == SkillType.None && effect.AffectsSkill())
            {
                throw new InvalidOperationException($"Affected skill required for this effect: {effect}");
            }
        }

        public static SkillType ByteConvertToSkill(this MagicEffectType effect, byte id)
        {
            return effect.AffectsSkill() ? (SkillType) id : SkillType.None;
        }

        public static byte ByteConvertToSkillId(this MagicEffectType effect, SkillType skill)
        {
            return effect.AffectsSkill() ? (byte) skill : BYTE_AFFECTED_ITEM_NONE;
        }


        public static SkillType IntConvertToSkill(this MagicEffectType effect, int id)
        {
            return effect.AffectsSkill() ? (SkillType) id : SkillType.None;
        }

        public static int IntConvertToSkillId(this MagicEffectType effect, SkillType skill)
        {
            return effect.AffectsSkill() ? (int) skill : INT_AFFECTED_ITEM_NONE;
        }

    }

}