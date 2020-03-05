
using System.Collections.Generic;
using System.IO;
using TES3.Records;
using TES3.Records.SubRecords;
using TES3.Util;

namespace TES3.GameItem.Item
{

    [TargetRecord("REPA")]
    public class RepairItem : TES3GameItem
    {

        string model;

        public RepairItem(string name) : base(name)
        {
            
        }

        public RepairItem(Record record) : base(record)
        {
            
        }

        public override string RecordName => "REPA";

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

        public int Uses
        {
            get;
            set;
        }

        public float Quality
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
            subRecords.Add(new RepairData("RIDT", Weight, Value, Uses, Quality));
        }

        protected override void UpdateRequired(Record record)
        {
            record.GetSubRecord<StringSubRecord>("NAME").Data = Name;
            record.GetSubRecord<StringSubRecord>("MODL").Data = Model;

            var repairData = record.GetSubRecord<RepairData>("RIDT");
            repairData.Weight = Weight;
            repairData.Value = Value;
            repairData.Uses = Uses;
            repairData.Quality = Quality;
        }

        protected override void UpdateOptional(Record record)
        {
            ProcessOptional(record, "FNAM", DisplayName != null, () => new StringSubRecord("FNAM", DisplayName), (sr) => sr.Data = DisplayName);
            ProcessOptional(record, "ITEX", Icon != null, () => new StringSubRecord("ITEX", DisplayName), (sr) => sr.Data = Icon);
            ProcessOptional(record, "SCRI", Script != null, () => new StringSubRecord("SCRI", Script), (sr) => sr.Data = Script);
            ProcessOptional(record, "DELE", Deleted, () => new IntSubRecord("DELE", 0), (sr) => sr.Data = 0);
        }

        protected override void DoSyncWithRecord(Record record)
        {
            // Required
            Id = record.GetSubRecord<StringSubRecord>("NAME").Data;
            Model = record.GetSubRecord<StringSubRecord>("MODL").Data;

            var repairData = record.GetSubRecord<RepairData>("RIDT");
            Weight = repairData.Weight;
            Value = repairData.Value;
            Uses = repairData.Uses;
            Quality = repairData.Quality;

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
            validator.CheckRequired(record, "RIDT");
        }

        public override TES3GameItem Clone()
        {
            return new RepairItem(Name)
            {
                Model = Model,
                DisplayName = DisplayName,
                Weight = Weight,
                Value = Value,
                Uses = Uses,
                Quality = Quality,
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
            writer.WriteLine($"Weight: {Weight}");
            writer.WriteLine($"Value: {Value}");
            writer.WriteLine($"Quality: {Quality}");
            writer.WriteLine($"Uses: {Uses}");
            writer.WriteLine($"Model: {Model}");
            writer.WriteLine($"Icon: {Icon}");
            writer.WriteLine($"Script: {Script}");
            writer.DecIndent();
        }

        public override string ToString()
        {
            return $"Repair Item ({Name})";
        }
    }
}
