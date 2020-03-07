
using System.IO;
using System.Collections.Generic;

using TES3.Records;
using TES3.Util;
using TES3.GameItem.Part;
using TES3.GameItem.TypeConstant;
using TES3.Records.SubRecords;

namespace TES3.GameItem.Item
{

    [TargetRecord("INGR")]
    public class Ingredient : TES3GameItem
    {

        string model;

        public Ingredient(string name) : base(name)
        {
            
        }

        public Ingredient(Record record) : base(record)
        {
            
        }

        public override string RecordName => "INGR";

        [IdField]
        public string Name
        {
            get => (string) Id;
            set => Id = value;
        }

        public string Model
        {
            get => model;
            set => model = Validation.NotNull(value, "value", "Model");
        }

        public string DisplayName
        {
            get;
            set;
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

        public string Icon
        {
            get;
            set;
        }

        public string Script
        {
            get;
            set;
        }

        public bool Deleted
        {
            get;
            set;
        }

        public IList<IngredientEffect> Effects
        {
            get;
            private set;
        }

        protected override void OnCreate(IList<SubRecord> subRecords)
        {
            subRecords.Add(new StringSubRecord("NAME", Name));
            subRecords.Add(new StringSubRecord("MODL", Model));

            var effectIds = new int[4];
            var skillIds = new int[4];
            var attributeIds = new int[4];
            for (var i = 0; i < 4; ++i)
            {
                if (i < Effects.Count)
                {
                    var ingredientEffect = Effects[i];
                    var effect = ingredientEffect.Effect;

                    effectIds[i] = (int) effect;
                    attributeIds[i] = effect.IntConvertToAttributeId(ingredientEffect.AffectedAttribute);
                    skillIds[i] = effect.IntConvertToSkillId(ingredientEffect.AffectedSkill);
                }
                else
                {
                    effectIds[i] = skillIds[i] = attributeIds[i] = -1;
                }
            }

            subRecords.Add(new IngredientData("IRDT", Weight, Value, effectIds, skillIds, attributeIds));
        }

        protected override void UpdateRequired(Record record)
        {
            record.GetSubRecord<StringSubRecord>("NAME").Data = Name;
            record.GetSubRecord<StringSubRecord>("MODL").Data = Model;

            var ingredientData = record.GetSubRecord<IngredientData>("IRDT");
            ingredientData.Weight = Weight;
            ingredientData.Value = Value;
            for (var i = 0; i < 4; ++i)
            {
                if (i < Effects.Count)
                {
                    var ingredientEffect = Effects[i];
                    var effect = ingredientEffect.Effect;

                    ingredientData.EffectIds[i] = (int) effect;
                    ingredientData.AttributeIds[i] = effect.IntConvertToAttributeId(ingredientEffect.AffectedAttribute);
                    ingredientData.SkillIds[i] = effect.IntConvertToSkillId(ingredientEffect.AffectedSkill);
                }
                else
                {
                    ingredientData.EffectIds[i] = ingredientData.SkillIds[i] = ingredientData.AttributeIds[i] = -1;
                }
            }
        }

        protected override void UpdateOptional(Record record)
        {
            ProcessOptional(record, "FNAM", DisplayName != null, () => new StringSubRecord("FNAM", DisplayName), (sr) => sr.Data = DisplayName);
            ProcessOptional(record, "ITEX", Icon != null, () => new StringSubRecord("ITEX", Icon), (sr) => sr.Data = Icon);
            ProcessOptional(record, "SCRI", Script != null, () => new StringSubRecord("SCRI", Script), (sr) => sr.Data = Script);
            ProcessOptional(record, "DELE", Deleted, () => new IntSubRecord("DELE", 0), (sr) => sr.Data = 0);
        }

        protected override void DoSyncWithRecord(Record record)
        {
            // Required
            Id = record.GetSubRecord<StringSubRecord>("NAME").Data;
            Model = record.GetSubRecord<StringSubRecord>("MODL").Data;

            var ingredientData = record.GetSubRecord<IngredientData>("IRDT");
            Weight = ingredientData.Weight;
            Value = ingredientData.Value;

            // Optional
            DisplayName = record.TryGetSubRecord<StringSubRecord>("FNAM")?.Data;
            Icon = record.TryGetSubRecord<StringSubRecord>("ITEX")?.Data;
            Script = record.TryGetSubRecord<StringSubRecord>("SCRI")?.Data;
            Deleted = record.ContainsSubRecord("DELE");

            // Collection
            Effects = new List<IngredientEffect>(4);
            for (var i = 0; i < 4; ++i)
            {
                var rawEffectId = ingredientData.EffectIds[i];
                if (rawEffectId == -1)
                {
                    continue;
                }

                var effect = (MagicEffectType) rawEffectId;
                var affectedAttribute = effect.IntConvertToAttribute(ingredientData.AttributeIds[i]);
                var affectedSkill = effect.IntConvertToSkill(ingredientData.SkillIds[i]);

                Effects.Add(new IngredientEffect(effect, affectedAttribute, affectedSkill));
            }
        }

        protected override void DoValidateRecord(Record record, Validator validator)
        {
            validator.CheckRequired(record, "NAME");
            validator.CheckRequired(record, "MODL");
            validator.CheckRequired(record, "IRDT");
        }

        public override TES3GameItem Copy()
        {
            var result = new Ingredient(Name)
            {
                Model = Model,
                DisplayName = DisplayName,
                Weight = Weight,
                Value = Value,
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
            writer.WriteLine($"Weight: {Weight}");
            writer.WriteLine($"Value: {Value}");
            writer.WriteLine($"Model: {Model}");
            writer.WriteLine($"Icon: {Icon}");
            writer.WriteLine($"Script: {Script}");

            if (Effects.Count > 1)
            {
                writer.WriteLine("Effects");
                writer.IncIndent();

                var table = new Table("Effect", "Affected Item");
                foreach (var ingredientEffect in Effects)
                {
                    var row = new object[2];
                    var effect = ingredientEffect.Effect;

                    row[0] = effect;

                    if (effect.AffectsAttribute())
                    {
                        row[1] = ingredientEffect.AffectedAttribute;
                    }
                    else if (effect.AffectsSkill())
                    {
                        row[1] = ingredientEffect.AffectedSkill;
                    }
                    else
                    {
                        row[1] = "None";
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
            return $"Ingredient ({Name})";
        }
    }
}
