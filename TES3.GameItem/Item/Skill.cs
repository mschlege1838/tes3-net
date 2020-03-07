using System;
using System.Collections.Generic;
using System.IO;
using TES3.GameItem.Part;
using TES3.GameItem.TypeConstant;
using TES3.Records;
using TES3.Records.SubRecords;
using TES3.Util;

namespace TES3.GameItem.Item
{

    [TargetRecord("SKIL")]
    public class Skill : TES3GameItem
    {

        const int MAX_USAGE_TYPES = 4;

        static SkillUsageType[] GetUsageType(SkillType skill)
        {
            switch (skill)
            {
                case SkillType.Acrobatics:
                    return new SkillUsageType[] { SkillUsageType.Jump, SkillUsageType.Fall };
                case SkillType.Alchemy:
                    return new SkillUsageType[] { SkillUsageType.PotionCreation, SkillUsageType.IngredientUse };
                case SkillType.Alteration:
                case SkillType.Conjuration:
                case SkillType.Destruction:
                case SkillType.Illusion:
                case SkillType.Mysticism:
                case SkillType.Restoration:
                    return new SkillUsageType[] { SkillUsageType.SuccessfulCast };
                case SkillType.Athletics:
                    return new SkillUsageType[] { SkillUsageType.SecondOfRunning, SkillUsageType.SecondOfSwimming };
                case SkillType.Axe:
                case SkillType.Blunt:
                case SkillType.HandToHand:
                case SkillType.LongBlade:
                case SkillType.Marksman:
                case SkillType.ShortBlade:
                case SkillType.Spear:
                    return new SkillUsageType[] { SkillUsageType.SuccessfulAttack };
                case SkillType.Block:
                    return new SkillUsageType[] { SkillUsageType.SuccessfulBlock };
                case SkillType.Enchant:
                    return new SkillUsageType[] { SkillUsageType.RechargeItem, SkillUsageType.UseMagicItem, SkillUsageType.CreateMagicItem, SkillUsageType.CastWhenStrikes };
                case SkillType.HeavyArmor:
                case SkillType.LightArmor:
                case SkillType.MediumArmor:
                case SkillType.Unarmored:
                    return new SkillUsageType[] { SkillUsageType.HitByOpponent };
                case SkillType.Mercantile:
                    return new SkillUsageType[] { SkillUsageType.SuccessfulBargain, SkillUsageType.SuccessfulBribe };
                case SkillType.Security:
                    return new SkillUsageType[] { SkillUsageType.DefeatTrap, SkillUsageType.PickLock };
                case SkillType.Sneak:
                    return new SkillUsageType[] { SkillUsageType.AvoidNotice, SkillUsageType.SuccessfulPickpocket };
                case SkillType.Speechcraft:
                    return new SkillUsageType[] { SkillUsageType.SuccessfulPersuasion, SkillUsageType.FailedPersuasion };
                case SkillType.Armorer:
                    return new SkillUsageType[] { SkillUsageType.SuccessfulRepair };
                default:
                    throw new ArgumentException($"Unrecognized skill type: {skill}", "skill");
            }
        }

        public IList<SkillUsage> Usages { get; private set; }

        public Skill(SkillType type, AttributeType governingAttribute, SpecializationType specialization) : base(type)
        {
            GoverningAttribute = governingAttribute;
            Specialization = specialization;
        }

        public Skill(Record record) : base(record)
        {
            
        }

        public override string RecordName => "SKIL";

        [IdField]
        public SkillType Type
        {
            get => (SkillType) Id;
            set => Id = value;
        }

        public AttributeType GoverningAttribute
        {
            get;
            set;
        }

        public SpecializationType Specialization
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        protected override void OnCreate(IList<SubRecord> subRecords)
        {
            subRecords.Add(new IntSubRecord("INDX", (int) Type));

            var useValues = new float[MAX_USAGE_TYPES];
            ProcessUseValues(useValues);
            subRecords.Add(new SkillData("SKDT", (int) GoverningAttribute, (int) Specialization, useValues));
        }

        protected override void UpdateRequired(Record record)
        {
            var skillData = record.GetSubRecord<SkillData>("SKDT");
            skillData.Attribute = (int) GoverningAttribute;
            skillData.Specialization = (int) Specialization;
            ProcessUseValues(skillData.UseValue);
        }

        protected override void UpdateOptional(Record record)
        {
            ProcessOptional(record, "DESC", Description != null, () => new StringSubRecord("DESC", Description), (sr) => sr.Data = Description);
        }

        protected override void DoSyncWithRecord(Record record)
        {
            // Required
            Id = (SkillType) record.GetSubRecord<IntSubRecord>("INDX").Data;

            var skillData = record.GetSubRecord<SkillData>("SKDT");
            GoverningAttribute = (AttributeType) skillData.Attribute;
            Specialization = (SpecializationType) skillData.Specialization;

            var usageTypes = GetUsageType(Type);
            var usages = new List<SkillUsage>(usageTypes.Length);
            for (int i = 0; i < usageTypes.Length; ++i)
            {
                usages.Add(new SkillUsage(usageTypes[i], skillData.UseValue[i]));
            }
            Usages = new ReadOnlyList<SkillUsage>(usages);

            // Optinal
            Description = record.TryGetSubRecord<StringSubRecord>("DESC")?.Data;

        }

        protected override void DoValidateRecord(Record record, Validator validator)
        {
            validator.CheckRequired(record, "INDX");
            validator.CheckRequired(record, "SKDT");
        }

        public override TES3GameItem Copy()
        {
            return new Skill(Type, GoverningAttribute, Specialization)
            {
                Description = Description
            };
        }

        public override void StreamDebug(TextWriter target)
        {
            var writer = new IndentWriter(target);

            writer.WriteLine(ToString());

            writer.IncIndent();
            writer.WriteLine($"Specialization: {Specialization}");
            writer.WriteLine($"Governing Attribute: {GoverningAttribute}");
            writer.WriteLine($"Description: {Description}");

            writer.WriteLine("Usages");
            writer.IncIndent();
            {
                var table = new Table("Usage Type", "Usage Value");
                foreach (var usage in Usages)
                {
                    table.AddRow(usage.UsageType, usage.UsageValue);
                }
                table.Print(writer);
            }
            writer.DecIndent();

            writer.DecIndent();
        }

        public override string ToString()
        {
            return $"Skill ({Type})";
        }

        void ProcessUseValues(float[] useValues)
        {
            for (var i = 0; i < MAX_USAGE_TYPES; ++i)
            {
                useValues[i] = i < Usages.Count ? Usages[i].UsageValue : 1.0f;
            }
        }
    }

}
