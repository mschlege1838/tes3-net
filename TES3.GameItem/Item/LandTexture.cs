
using System.Collections.Generic;
using System.IO;
using TES3.Records;
using TES3.Records.SubRecords;
using TES3.Util;

namespace TES3.GameItem.Item
{

    [TargetRecord("LTEX")]
    public class LandTexture : TES3GameItem
    {

        public LandTexture(string name) : base(name)
        {
            
        }

        public LandTexture(Record record) : base(record)
        {
            
        }

        public override string RecordName => "LTEX";

        [IdField]
        public string Name
        {
            get => (string) Id;
            set => Id = value;
        }

        public int Index
        {
            get;
            set;
        }

        public string Texture
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
            subRecords.Add(new IntSubRecord("INTV", Index));
            subRecords.Add(new StringSubRecord("DATA", Texture));
        }

        protected override void UpdateRequired(Record record)
        {
            record.GetSubRecord<StringSubRecord>("NAME").Data = Name;
            record.GetSubRecord<IntSubRecord>("INTV").Data = Index;
            record.GetSubRecord<StringSubRecord>("DATA").Data = Texture;
        }

        protected override void UpdateOptional(Record record)
        {
            ProcessOptional(record, "DELE", Deleted, () => new IntSubRecord("DELE", 0), (sr) => sr.Data = 0);
        }

        protected override void DoSyncWithRecord(Record record)
        {
            // Required
            Id = record.GetSubRecord<StringSubRecord>("NAME").Data;
            Index = record.GetSubRecord<IntSubRecord>("INTV").Data;
            Texture = record.GetSubRecord<StringSubRecord>("DATA").Data;

            // Optional
            Deleted = record.ContainsSubRecord("DELE");
        }

        protected override void DoValidateRecord(Record record, Validator validator)
        {
            validator.CheckRequired(record, "NAME");
            validator.CheckRequired(record, "INTV");
            validator.CheckRequired(record, "DATA");
        }

        public override TES3GameItem Copy()
        {
            return new LandTexture(Name)
            {
                Index = Index,
                Texture = Texture,
                Deleted = Deleted
            };
        }

        public override void StreamDebug(TextWriter target)
        {
            var writer = new IndentWriter(target);

            writer.WriteLine(ToString());

            writer.IncIndent();
            writer.WriteLine($"Index: {Index}");
            writer.WriteLine($"Texture: {Texture}");
            writer.DecIndent();
        }

        public override string ToString()
        {
            return $"Land Texture ({Name})";
        }
    }
}
