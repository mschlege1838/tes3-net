
using System.Collections.Generic;
using System.IO;
using TES3.Records;
using TES3.Records.SubRecords;
using TES3.Util;

namespace TES3.GameItem.Item
{

    [TargetRecord("SOUN")]
    public class Sound : TES3GameItem
    {

        float volume;
        string fileName;

        public Sound(string name) : base(name)
        {
            
        }

        public Sound(Record record) : base(record)
        {

        }

        public override string RecordName => "SOUN";

        [IdField]
        public string Name
        {
            get => (string) Id;
            set => Id = value;
        }

        public string FileName
        {
            get => fileName;
            set => fileName = Validation.NotNull(value, "value", "File Name");
        }

        public float Volume
        {
            get => volume;
            set => volume = (byte) (Validation.Range(value, 0, 1, "value", "Volume") * 255);
        }

        public byte MinRange
        {
            get;
            set;
        }

        public byte MaxRange
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
            subRecords.Add(new StringSubRecord("FNAM", FileName));
            subRecords.Add(new SoundData("DATA", (byte) (Volume * 255), MinRange, MaxRange));
        }

        protected override void UpdateRequired(Record record)
        {
            record.GetSubRecord<StringSubRecord>("NAME").Data = Name;
            record.GetSubRecord<StringSubRecord>("FNAM").Data = FileName;

            var soundData = record.GetSubRecord<SoundData>("DATA");
            soundData.Volume = (byte) (Volume * 255);
            soundData.MinRange = MinRange;
            soundData.MaxRange = MaxRange;
        }

        protected override void UpdateOptional(Record record)
        {
            ProcessOptional(record, "DELE", Deleted, () => new IntSubRecord("DELE", 0), (sr) => sr.Data = 0);
        }

        protected override void DoSyncWithRecord(Record record)
        {
            // Required
            Id = record.GetSubRecord<StringSubRecord>("NAME").Data;
            FileName = record.GetSubRecord<StringSubRecord>("FNAM").Data;

            var soundData = record.GetSubRecord<SoundData>("DATA");
            Volume = soundData.Volume / 255f;
            MinRange = soundData.MinRange;
            MaxRange = soundData.MaxRange;

            // Optional
            Deleted = record.ContainsSubRecord("DELE");
        }

        protected override void DoValidateRecord(Record record, Validator validator)
        {
            validator.CheckRequired(record, "NAME");
            validator.CheckRequired(record, "FNAM");
            validator.CheckRequired(record, "DATA");
        }

        public override TES3GameItem Clone()
        {
            return new Sound(Name)
            {
                FileName = FileName,
                Volume = Volume,
                MinRange = MinRange,
                MaxRange = MaxRange,
                Deleted = Deleted
            };
        }

        public override void StreamDebug(TextWriter target)
        {
            var writer = new IndentWriter(target);

            writer.WriteLine(ToString());

            writer.IncIndent();
            writer.WriteLine($"File: {FileName}");
            writer.WriteLine($"Volume: {Volume}");
            writer.WriteLine($"Min Range: {MinRange}");
            writer.WriteLine($"Max Range: {MaxRange}");
            writer.DecIndent();
        }

        public override string ToString()
        {
            return $"Sound ({Name})";
        }
    }
}
