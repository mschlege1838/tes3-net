
using System.Collections.Generic;
using System.IO;
using TES3.Records;
using TES3.Records.SubRecords;
using TES3.Util;

namespace TES3.GameItem.Item
{

    [TargetRecord("DOOR")]
    public class Door : TES3GameItem
    {

        public Door(string name) : base(name)
        {
            
        }

        public Door(Record record) : base(record)
        {
            
        }

        public override string RecordName => "DOOR";

        [IdField]
        public string Name
        {
            get => (string) Id;
            set => Id = value;
        }

        public string DisplayName
        {
            get;
            set;
        }

        public string Model
        {
            get;
            set;
        }

        public string Script
        {
            get;
            set;
        }

        public string OpenSound
        {
            get;
            set;
        }

        public string CloseSound
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
            ProcessOptional(record, "SNAM", OpenSound != null, () => new StringSubRecord("SNAM", OpenSound), (sr) => sr.Data = OpenSound);
            ProcessOptional(record, "ANAM", CloseSound != null, () => new StringSubRecord("ANAM", CloseSound), (sr) => sr.Data = CloseSound);
            ProcessOptional(record, "DELE", Deleted, () => new IntSubRecord("DELE", 0), (sr) => sr.Data = 0);
        }

        protected override void DoSyncWithRecord(Record record)
        {
            // Required
            Id = record.GetSubRecord<StringSubRecord>("NAME").Data;
            Model = record.GetSubRecord<StringSubRecord>("MODL").Data;

            // Optional
            DisplayName = record.TryGetSubRecord<StringSubRecord>("FNAM")?.Data;
            Script = record.TryGetSubRecord<StringSubRecord>("SCRI")?.Data;
            OpenSound = record.TryGetSubRecord<StringSubRecord>("SNAM")?.Data;
            CloseSound = record.TryGetSubRecord<StringSubRecord>("ANAM")?.Data;
            Deleted = record.ContainsSubRecord("DELE");
        }

        protected override void DoValidateRecord(Record record, Validator validator)
        {
            validator.CheckRequired(record, "NAME");
            validator.CheckRequired(record, "MODL");
        }

        public override TES3GameItem Copy()
        {
            return new Door(Name)
            {
                DisplayName = DisplayName,
                Model = Model,
                Script = Script,
                OpenSound = OpenSound,
                CloseSound = CloseSound,
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
            writer.WriteLine($"Open Sound: {OpenSound}");
            writer.WriteLine($"Close Sound: {CloseSound}");
            writer.DecIndent();
        }

        public override string ToString()
        {
            return $"Door ({Name})";
        }
    }
}
