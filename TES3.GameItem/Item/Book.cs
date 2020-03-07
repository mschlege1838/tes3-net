
using System.Collections.Generic;
using System.IO;
using TES3.GameItem.TypeConstant;
using TES3.Core;
using TES3.Records;
using TES3.Records.SubRecords;
using TES3.Util;

namespace TES3.GameItem.Item
{

    [TargetRecord("BOOK")]
    public class Book : TES3GameItem
    {

		string model;

        public Book(string name) : base(name)
        {
			Model = Constants.DefaultModelValue;
        }

        public Book(Record record) : base(record)
        {
            
        }

        public override string RecordName => "BOOK";

        [IdField]
        public string Name
        {
            get => (string) Id;
            set => Id = value;
        }

        public string Model
        {
            get => model;
            set => model = Validation.NotNull(value, "value", "Model").Length > 0 ? value : Constants.DefaultModelValue;
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

        public bool Scroll
        {
            get;
            set;
        }

        public string Enchantment
        {
            get;
            set;
        }

        public SkillType Skill
        {
            get;
            set;
        }

        public int EnchantCapacity
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

        public string Text
        {
            get;
            set;
        }

        public bool Deleted
        {
            get;
            set;
        }

        protected override void OnCreate(IList<SubRecord> subRecords)
        {
            subRecords.Add(new StringSubRecord("NAME", Name));
            subRecords.Add(new StringSubRecord("MODL", Model));
            subRecords.Add(new BookData("BKDT", Weight, Value, Scroll ? 1 : 0, Skill == SkillType.None ? -1 : (int) Skill, EnchantCapacity));
        }

        protected override void UpdateRequired(Record record)
        {
            record.GetSubRecord<StringSubRecord>("NAME").Data = Name;
            record.GetSubRecord<StringSubRecord>("MODL").Data = Model;

            var bookData = record.GetSubRecord<BookData>("BKDT");
            bookData.Weight = Weight;
            bookData.Value = Value;
            bookData.Scroll = Scroll ? 1 : 0;
            bookData.SkillId = Skill == SkillType.None ? -1 : (int) Skill;
            bookData.EnchantPoints = EnchantCapacity;
        }

        protected override void UpdateOptional(Record record)
        {
            ProcessOptional(record, "FNAM", DisplayName != null, () => new StringSubRecord("FNAM", DisplayName), (sr) => sr.Data = DisplayName);
            ProcessOptional(record, "ITEX", Icon != null, () => new StringSubRecord("ITEX", Icon), (sr) => sr.Data = Icon);
            ProcessOptional(record, "TEXT", Text != null, () => new StringSubRecord("TEXT", Text), (sr) => sr.Data = Text);
            ProcessOptional(record, "SCRI", Script != null, () => new StringSubRecord("SCRI", Script), (sr) => sr.Data = Script);
            ProcessOptional(record, "ENAM", Enchantment != null, () => new StringSubRecord("ENAM", Enchantment), (sr) => sr.Data = Enchantment);
            ProcessOptional(record, "DELE", Deleted, () => new IntSubRecord("DELE", 0), (sr) => sr.Data = 0);
        }

        protected override void DoSyncWithRecord(Record record)
        {
            // Required
            Id = record.GetSubRecord<StringSubRecord>("NAME").Data;
            Model = record.GetSubRecord<StringSubRecord>("MODL").Data;

            var bookData = record.GetSubRecord<BookData>("BKDT");
            Weight = bookData.Weight;
            Value = bookData.Value;
            Scroll = bookData.Scroll == 1;
            Skill = bookData.SkillId == -1 ? SkillType.None : (SkillType) bookData.SkillId;

            // Optional
            DisplayName = record.TryGetSubRecord<StringSubRecord>("FNAM")?.Data;
            Icon = record.TryGetSubRecord<StringSubRecord>("ITEX")?.Data;
            Text = record.TryGetSubRecord<StringSubRecord>("TEXT")?.Data;
            Script = record.TryGetSubRecord<StringSubRecord>("SCRI")?.Data;
            Enchantment = record.TryGetSubRecord<StringSubRecord>("ENAM")?.Data;
        }

        protected override void DoValidateRecord(Record record, Validator validator)
        {
            validator.CheckRequired(record, "NAME");
            validator.CheckRequired(record, "MODL");
            validator.CheckRequired(record, "BKDT");
        }

        public override TES3GameItem Copy()
        {
            return new Book(Name)
            {
                Model = Model,
                DisplayName = DisplayName,
                Weight = Weight,
                Value = Value,
                Scroll = Scroll,
                Enchantment = Enchantment,
                Skill = Skill,
                EnchantCapacity = EnchantCapacity,
                Icon = Icon,
                Script = Script,
                Text = Text,
                Deleted = Deleted
            };
        }

        public override void StreamDebug(TextWriter target)
        {
            var writer = new IndentWriter(target);

            writer.WriteLine(ToString());

            writer.IncIndent();
            writer.WriteLine($"Name: {DisplayName}");
            writer.WriteLine($"Value: {Value}");
            writer.WriteLine($"Weight: {Weight}");
            writer.WriteLine($"Enchant Capacity: {EnchantCapacity}");
            writer.WriteLine($"Teaches Skill: {Skill}");
            writer.WriteLine($"Scroll: {Scroll}");
            writer.WriteLine($"Enchantment: {Enchantment}");
            writer.WriteLine($"Model: {Model}");
            writer.WriteLine($"Icon: {Icon}");
            writer.WriteLine($"Script: {Script}");

            if (Text != null)
            {
                target.WriteLine("Text");
                target.WriteLine();
                target.WriteLine("-----------------------------------------------");
                target.WriteLine(Text);
                target.WriteLine("-----------------------------------------------");
                target.WriteLine();
            }


            writer.DecIndent();
        }

        public override string ToString()
        {
            return $"Book ({Name})";
        }
    }
}
