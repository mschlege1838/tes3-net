
using System.Collections.Generic;
using System.IO;
using TES3.GameItem.TypeConstant;
using TES3.Records;
using TES3.Records.SubRecords;
using TES3.Util;
using TES3.Core;

namespace TES3.GameItem.Item
{

    [TargetRecord("APPA")]
    public class Apparatus : TES3GameItem
    {

        string model;
		string displayName;

        public Apparatus(string name, ApparatusType type) : base(name)
        {
			Model = Constants.DefaultModelValue;
            Type = type;
            DisplayName = "";
        }
		
        public Apparatus(Record record) : base(record)
        {
            
        }

        public override string RecordName => "APPA";


        [IdField]
        public string Name
        {
            get => (string) Id;
            set => Id = value;
        }

        public string Model
        {
            get => model;
            set
            {
                model = value;

                if (value == null || value.Length == 0)
                {
                    TES3Logger.Log(TES3LogLevel.Warn, "Model field is normally set for Apparatus by the CS, but the game appears to handle this appropriately.");
                }
            }
        }

        public string DisplayName
        {
            get => displayName;
            set => displayName = Validation.NotNull(value, "value", "Display Name");
        }

        public ApparatusType Type
        {
            get;
            set;
        }

        public float Quality
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
            subRecords.Add(new StringSubRecord("FNAM", DisplayName));
            subRecords.Add(new AlchemyApparatusData("AADT", (int) Type, Quality, Weight, Value));
        }

        protected override void UpdateRequired(Record record)
        {
            record.GetSubRecord<StringSubRecord>("NAME").Data = Name;
            record.GetSubRecord<StringSubRecord>("MODL").Data = Model;
            record.GetSubRecord<StringSubRecord>("FNAM").Data = DisplayName;

            var apparatusData = record.GetSubRecord<AlchemyApparatusData>("AADT");
            apparatusData.Type = (int) Type;
            apparatusData.Quality = Quality;
            apparatusData.Weight = Weight;
            apparatusData.Value = Value;
        }

        protected override void UpdateOptional(Record record)
        {
            ProcessOptional(record, "ITEX", Icon != null, () => new StringSubRecord("ITEX", Icon), (sr) => sr.Data = Icon);
            ProcessOptional(record, "SCRI", Script != null, () => new StringSubRecord("SCRI", Script), (sr) => sr.Data = Script);
            ProcessOptional(record, "DELE", Deleted, () => new IntSubRecord("DELE", 0), (sr) => sr.Data = 0);
        }

        protected override void DoSyncWithRecord(Record record)
        {
            // Required
            Id = record.GetSubRecord<StringSubRecord>("NAME").Data;
            Model = record.GetSubRecord<StringSubRecord>("MODL").Data;
            DisplayName = record.GetSubRecord<StringSubRecord>("FNAM").Data;

            var apparatusData = record.GetSubRecord<AlchemyApparatusData>("AADT");
            Type = (ApparatusType) apparatusData.Type;
            Quality = apparatusData.Quality;
            Weight = apparatusData.Weight;
            Value = apparatusData.Value;

            // Optional
            Icon = record.GetSubRecord<StringSubRecord>("ITEX")?.Data;
            Script = record.TryGetSubRecord<StringSubRecord>("SCRI")?.Data;
            Deleted = record.ContainsSubRecord("DELE");
        }

        protected override void DoValidateRecord(Record record, Validator validator)
        {
            validator.CheckRequired(record, "NAME");
            validator.WarnRequired(record, "MODL");
            validator.WarnRequired(record, "FNAM");
            validator.WarnRequired(record, "AADT");
        }


        public override TES3GameItem Clone()
        {
            return new Apparatus(Name, Type)
            {
                Model = Model,
                DisplayName = DisplayName,
                Quality = Quality,
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
            writer.WriteLine($"Type: {Type}");
            writer.WriteLine($"Quality: {Quality}");
            writer.WriteLine($"Weight: {Weight}");
            writer.WriteLine($"Value: {Value}");
            writer.WriteLine($"Model: {Model}");
            writer.WriteLine($"Icon: {Icon}");
            writer.WriteLine($"Script: {Script}");

            writer.DecIndent();
        }

        public override string ToString()
        {
            return $"Apparatus ({Name})";
        }
    }
}
