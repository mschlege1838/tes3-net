
using System.Collections.Generic;
using System.IO;
using TES3.Records;
using TES3.Records.SubRecords;
using TES3.Util;

namespace TES3.GameItem.Item
{
    public abstract class LocksmithItem : TES3GameItem
    {

        string model;
        public LocksmithItem(string name) : base(name)
        {
            
        }

        public LocksmithItem(Record record) : base(record)
        {
            
        }

        protected abstract string LockpickDataSubRecordName { get; }

        protected abstract string FriendlyTypeName { get; }


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

        public float Quality
        {
            get;
            set;
        }

        public int Uses
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
            subRecords.Add(new LockpickData(LockpickDataSubRecordName, Weight, Value, Quality, Uses));
        }

        protected override void UpdateRequired(Record record)
        {
            record.GetSubRecord<StringSubRecord>("NAME").Data = Name;
            record.GetSubRecord<StringSubRecord>("MODL").Data = Model;

            var lockpickData = record.GetSubRecord<LockpickData>(LockpickDataSubRecordName);
            lockpickData.Weight = Weight;
            lockpickData.Value = Value;
            lockpickData.Quality = Quality;
            lockpickData.Uses = Uses;
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

            var lockpickData = record.GetSubRecord<LockpickData>(LockpickDataSubRecordName);
            Weight = lockpickData.Weight;
            Value = lockpickData.Value;
            Quality = lockpickData.Quality;
            Uses = lockpickData.Uses;

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
            validator.CheckRequired(record, LockpickDataSubRecordName);
        }

        public override void StreamDebug(TextWriter target)
        {
            var writer = new IndentWriter(target);

            writer.WriteLine(ToString());

            writer.IncIndent();
            writer.WriteLine($"Name: {DisplayName}");
            writer.WriteLine($"Quality: {Quality}");
            writer.WriteLine($"Uses: {Uses}");
            writer.WriteLine($"Value: {Value}");
            writer.WriteLine($"Weight: {Weight}");
            writer.WriteLine($"Model: {Model}");
            writer.WriteLine($"Icon: {Icon}");
            writer.WriteLine($"Script: {Script}");
            writer.DecIndent();
        }

        protected void CopyClone(LocksmithItem clone)
        {
            clone.Model = Model;
            clone.DisplayName = DisplayName;
            clone.Weight = Weight;
            clone.Value = Value;
            clone.Quality = Quality;
            clone.Uses = Uses;
            clone.Icon = Icon;
            clone.Script = Script;
            clone.Deleted = Deleted;
        }

        public override string ToString()
        {
            return $"{FriendlyTypeName} ({Name})";
        }

    }
}
