﻿
using System.Collections.Generic;
using System.IO;
using TES3.Records;
using TES3.Records.SubRecords;
using TES3.Util;

namespace TES3.GameItem.Item
{

    [TargetRecord("MISC")]
    public class MiscItem : TES3GameItem
    {

        string model;

        public MiscItem(string name) : base(name)
        {
            
        }

        public MiscItem(Record record) : base(record)
        {
            
        }

        public override string RecordName => "MISC";

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

        protected override void OnCreate(IList<SubRecord> subRecords)
        {
            subRecords.Add(new StringSubRecord("NAME", Name));
            subRecords.Add(new StringSubRecord("MODL", Model));
            subRecords.Add(new MiscItemData("MCDT", Weight, Value, 0));
        }

        protected override void UpdateRequired(Record record)
        {
            record.GetSubRecord<StringSubRecord>("NAME").Data = Name;
            record.GetSubRecord<StringSubRecord>("MODL").Data = Model;

            var miscItemData = record.GetSubRecord<MiscItemData>("MCDT");
            miscItemData.Weight = Weight;
            miscItemData.Value = Value;
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

            var miscItemData = record.GetSubRecord<MiscItemData>("MCDT");
            Weight = miscItemData.Weight;
            Value = miscItemData.Value;

            // Optional
            DisplayName = record.TryGetSubRecord<StringSubRecord>("FNAM")?.Data;
            Icon = record.TryGetSubRecord<StringSubRecord>("ITEX")?.Data;
            Script = record.TryGetSubRecord<StringSubRecord>("SCRI")?.Data;
            Deleted = record.ContainsSubRecord("DELE");
        }

        protected override void DoValidateRecord(Record record, Validator validator)
        {
            validator.CheckRequired(record, "NAME");
            validator.CheckRequired(record, "MODL");
            validator.CheckRequired(record, "MCDT");
        }

        public override TES3GameItem Copy()
        {
            return new MiscItem(Name)
            {
                Model = Model,
                DisplayName = DisplayName,
                Weight = Weight,
                Value = Value,
                Icon = Icon,
                Script = Script,
                Deleted = Deleted
            };
        }

        public override void StreamDebug(TextWriter target)
        {
            var writer = new IndentWriter(target);

            writer.WriteLine(ToString());
            writer.IncIndent();
            writer.WriteLine($"Name: {DisplayName}");
            writer.WriteLine($"Model: {Model}");
            writer.WriteLine($"Icon: {Icon}");
            writer.WriteLine($"Weight: {Weight}");
            writer.WriteLine($"Value: {Value}");
            writer.WriteLine($"Script: {Script}");
            writer.DecIndent();
        }

        public override string ToString()
        {
            return $"Misc Item ({Name})";
        }


    }
}
