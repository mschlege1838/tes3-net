
using System.IO;
using System.Collections.Generic;

using TES3.Records;
using TES3.Util;
using TES3.GameItem.TypeConstant;
using TES3.Records.SubRecords;
using TES3.Core;
using TES3.GameItem.Part;

namespace TES3.GameItem.Item
{

    [TargetRecord("ALCH")]
    public class Potion : TES3GameItem
    {

        const int MAX_ALCHEMY_EFFECTS = 8;

        string model;
        string icon;
        string displayName;
        string script;

        public Potion(string name) : base(name)
        {
            
        }

        public Potion(Record record) : base(record)
        {

        }

        public override string RecordName => "ALCH";

        [IdField]
        public string Name
        {
            get => (string) Id;
            set => Id = value;
        }

        public string Model
        {
            get => model;
            set => model = Validation.Length(value, Constants.Potion.MODL_MAX_LENGTH, "Model");
        }

        public string DisplayName
        {
            get => displayName;
            set => displayName = Validation.Length(value, Constants.Potion.FNAM_MAX_LENGTH, "Display Name");
        }

        public float Weight
        {
            get;
            set;
        }

        public int Value
        {
            get;
            set;
        }

        public bool AutoCalcValue
        {
            get;
            set;
        }

        public string Icon
        {
            get => icon;
            set => icon = Validation.Length(value, Constants.Potion.TEXT_MAX_LENGTH, "Icon");
        }

        public string Script
        {
            get => script;
            set => script = Validation.Length(value, Constants.Script.SCHD_NAME_MAX_LENGTH, "Script");
        }

        public bool Deleted
        {
            get;
            set;
        }


        public IList<PotionEffect> Effects
        {
            get;
        } = new MaxCapacityList<PotionEffect>(MAX_ALCHEMY_EFFECTS, "Effects", false);


        protected override void OnCreate(IList<SubRecord> subRecords)
        {
            subRecords.Add(new StringSubRecord("NAME", Name));
            subRecords.Add(new PotionData("ALDT", Weight, Value, AutoCalcValue ? 1 : 0));
        }

        protected override void UpdateRequired(Record record)
        {
            record.GetSubRecord<StringSubRecord>("NAME").Data = Name;

            var potionData = record.GetSubRecord<PotionData>("ALDT");
            potionData.Weight = Weight;
            potionData.Value = Value;
            potionData.AutoCalc = AutoCalcValue ? 1 : 0;
        }

        protected override void UpdateOptional(Record record)
        {
            ProcessOptional(record, "MODL", Model != null, () => new StringSubRecord("MODL", Model), (sr) => sr.Data = Model);
            ProcessOptional(record, "FNAM", DisplayName != null, () => new StringSubRecord("FNAM", DisplayName), (sr) => sr.Data = DisplayName);
            ProcessOptional(record, "TEXT", Icon != null, () => new StringSubRecord("TEXT", Icon), (sr) => sr.Data = Icon);
            ProcessOptional(record, "SCRI", Script != null, () => new StringSubRecord("SCRI", Script), (sr) => sr.Data = Script);
            ProcessOptional(record, "DELE", Deleted, () => new IntSubRecord("DELE", 0), (sr) => sr.Data = 0);
            
            UpdateCollection(record, Effects, "ENAM",
                delegate (ref int index, PotionEffect item)
                {
                    var effect = item.Effect;

                    var attributeId = effect.ByteConvertToAttributeId(item.AffectedAttribute);
                    var skillId = effect.ByteConvertToSkillId(item.AffectedSkill);
                    record.InsertSubRecordAt(index++, new SpellItemData("ENAM", (short) effect, skillId, attributeId, 0, 0, item.Duration, item.Magnitude, item.Magnitude));
                }
            );
        }

        protected override void DoSyncWithRecord(Record record)
        {
            // Required
            Id = record.GetSubRecord<StringSubRecord>("NAME").Data;

            var potionData = record.GetSubRecord<PotionData>("ALDT");
            Weight = potionData.Weight;
            Value = potionData.AutoCalc;
            AutoCalcValue = potionData.AutoCalc == 1;

            // Optional
            Model = record.TryGetSubRecord<StringSubRecord>("MODL")?.Data;
            DisplayName = record.TryGetSubRecord<StringSubRecord>("FNAM")?.Data;
            Icon = record.TryGetSubRecord<StringSubRecord>("TEXT")?.Data;
            Script = record.TryGetSubRecord<StringSubRecord>("SCRI")?.Data;

            // Collection
            Effects.Clear();
            foreach (SpellItemData subRecord in record.GetEnumerableFor("ENAM"))
            {
                Effects.Add(new PotionEffect(subRecord));
            }
        }

        protected override void DoValidateRecord(Record record, Validator validator)
        {
            validator.CheckRequired(record, "NAME");
            validator.WarnRequired(record, "ALDT");
            validator.WarnCount(record, "ENAM", MAX_ALCHEMY_EFFECTS);
        }

        public override TES3GameItem Clone()
        {
            var result = new Potion(Name)
            {
                Model = Model,
                DisplayName = DisplayName,
                Weight = Weight,
                Value = Value,
                AutoCalcValue = AutoCalcValue,
                Icon = Icon,
                Script = Script,
                Deleted = Deleted
            };

            CollectionUtils.Copy(Effects, result.Effects);
            return result;
        }

        public override void StreamDebug(TextWriter target)
        {
            var writer = new IndentWriter(target);

            writer.WriteLine(ToString());

            writer.IncIndent();
            writer.WriteLine($"Name: {DisplayName}");
            writer.WriteLine($"Value: {Value}");
            writer.WriteLine($"Weight: {Weight}");
            writer.WriteLine($"Auto Calc Value: {AutoCalcValue}");
            writer.WriteLine($"Model: {Model}");
            writer.WriteLine($"Icon: {Icon}");
            writer.WriteLine($"Script: {Script}");

            if (Effects.Count > 0)
            {
                writer.WriteLine("Effects");
                writer.IncIndent();

                var table = new Table("Effect", "Affected Item", "Duration", "Magnitude");
                foreach (var potionEffect in Effects)
                {
                    var row = new object[4];
                    var effect = potionEffect.Effect;

                    row[0] = effect;
                    row[2] = potionEffect.Duration;
                    row[3] = potionEffect.Magnitude;

                    if (effect.AffectsAttribute())
                    {
                        row[1] = potionEffect.AffectedAttribute;
                    }
                    else if (effect.AffectsSkill())
                    {
                        row[1] = potionEffect.AffectedSkill;
                    }
                    else
                    {
                        row[1] = "N/A";
                    }

                    table.AddRow(row);
                }
                table.Print(writer);

                writer.DecIndent();
            }
            writer.DecIndent();
        }

        public override string ToString()
        {
            return $"Potion ({Name})";
        }

    }
}
