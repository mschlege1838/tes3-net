
using System.Collections.Generic;
using System.IO;
using TES3.Records;
using TES3.Records.SubRecords;
using TES3.Util;

namespace TES3.GameItem.Item
{

    [TargetRecord("ACTI")]
    public class TES3Activator : TES3GameItem
    {

        string model;
        string displayName;
        string script;

        public TES3Activator(string name) : base(name)
        {
            Model = Constants.DefaultModelValue;
            DisplayName = "";
        }

        public TES3Activator(Record record) : base(record)
        {
            
        }

        public override string RecordName => "ACTI";

        [IdField]
        public string Name
        {
            get => (string) Id;
            set => Id = value;
        }

        public string Model
        {
            get => model;
            set => model = Validation.Length(value, Constants.Activator.MODL_MAX_LENGTH, "value", "Model", true).Length > 0 ? value : Constants.DefaultModelValue;
        }

        public string DisplayName
        {
            get => displayName;
            set => displayName = Validation.Length(value, Constants.Activator.FNAM_MAX_LENGTH, "value", "Display Name");
        }

        public string Script
        {
            get => script;
            set => script = Validation.Length(value, Constants.Script.SCHD_NAME_MAX_LENGTH, "value", "Script");
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
        }

        protected override void UpdateRequired(Record record)
        {
            record.GetSubRecord<StringSubRecord>("NAME").Data = Name;
            record.GetSubRecord<StringSubRecord>("MODL").Data = Model;
        }

        protected override void UpdateOptional(Record record)
        {
            ProcessOptional(record, "FNAM", DisplayName != null, () => new StringSubRecord("FNAM", DisplayName), (sr) => sr.Data = DisplayName);
            ProcessOptional(record, "SCRI", Script != null, () => new StringSubRecord("SCRI", Script), (sr) => sr.Data = Script);
            ProcessOptional(record, "DELE", Deleted, () => new IntSubRecord("DELE", 0), (sr) => sr.Data = 0);
        }

        protected override void DoSyncWithRecord(Record record)
        {
            // Required
            Id = record.GetSubRecord<StringSubRecord>("NAME").Data;
            Model = record.GetSubRecord<StringSubRecord>("MODL").Data;
            DisplayName = record.GetSubRecord<StringSubRecord>("FNAM").Data;

            // Optional
            Script = record.TryGetSubRecord<StringSubRecord>("SCRI")?.Data;
            Deleted = record.ContainsSubRecord("DELE");
        }

        protected override void DoValidateRecord(Record record, Validator validator)
        {
            validator.CheckRequired(record, "NAME");
            validator.CheckRequired(record, "MODL");
        }

        public override TES3GameItem Clone()
        {
            return new TES3Activator(Name)
            {
                Model = Model,
                DisplayName = DisplayName,
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
            writer.WriteLine($"Script: {Script}");
            writer.DecIndent();
        }

        public override string ToString()
        {
            return $"Activator ({Name})";
        }
    }
}
