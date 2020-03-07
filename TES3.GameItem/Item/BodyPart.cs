
using System.Collections.Generic;
using System.IO;
using TES3.GameItem.TypeConstant;
using TES3.Records;
using TES3.Records.SubRecords;
using TES3.Util;

namespace TES3.GameItem.Item
{

    [TargetRecord("BODY")]
    public class BodyPart : TES3GameItem
    {
		
		
        const byte FEMALE_FLAG = 0x01;
        const byte PLAYABLE_FLAG = 0x02;

		string model;

        public BodyPart(string name) : base(name)
        {
			Model = Constants.DefaultModelValue;
        }

        public BodyPart(Record record) : base(record)
        {
            
        }

        public override string RecordName => "BODY";

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

        public string Race
        {
            get;
            set;
        }

        public BodyPartSkinType SkinType
        {
            get;
            set;
        }

        public BodyPartType Type
        {
            get;
            set;
        }

        public BodyPartPart Part
        {
            get;
            set;
        }

        public bool Female
        {
            get;
            set;
        }

        public bool Playable
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
            //subRecords.Add(new StringSubRecord("FNAM", Race));

            byte flags = 0;
            if (Female)
            {
                flags |= FEMALE_FLAG;
            }
            if (Playable)
            {
                flags |= PLAYABLE_FLAG;
            }
            subRecords.Add(new BodyPartData("BYDT", (byte) Part, (byte) SkinType, flags, (byte) Type));
        }

        protected override void UpdateRequired(Record record)
        {
            record.GetSubRecord<StringSubRecord>("NAME").Data = Name;
            record.GetSubRecord<StringSubRecord>("MODL").Data = Model;
            record.GetSubRecord<StringSubRecord>("FNAM").Data = Race;

            var bodyPartData = record.GetSubRecord<BodyPartData>("BYDT");
            bodyPartData.Vampire = (byte) SkinType;
            bodyPartData.PartType = (byte) Type;
            bodyPartData.Part = (byte) Part;
            BodyDataFlagSet(bodyPartData, Female, FEMALE_FLAG);
            BodyDataFlagSet(bodyPartData, Playable, PLAYABLE_FLAG);
        }

        protected override void UpdateOptional(Record record)
        {
            ProcessOptional(record, "DELE", Deleted, () => new IntSubRecord("DELE", 0), (sr) => sr.Data = 0);
        }

        protected override void DoSyncWithRecord(Record record)
        {
            // Required
            Id = record.GetSubRecord<StringSubRecord>("NAME").Data;
            Model = record.GetSubRecord<StringSubRecord>("MODL").Data;
            Race = record.GetSubRecord<StringSubRecord>("FNAM").Data;

            var bodyPartData = record.GetSubRecord<BodyPartData>("BYDT");
            SkinType = (BodyPartSkinType) bodyPartData.Vampire;
            Type = (BodyPartType) bodyPartData.PartType;
            Part = (BodyPartPart) bodyPartData.Part;
            Female = HasFlagSet(bodyPartData.Flags, FEMALE_FLAG);
            Playable = HasFlagSet(bodyPartData.Flags, PLAYABLE_FLAG);

            // Optional
            Deleted = record.ContainsSubRecord("DELE");
        }

        protected override void DoValidateRecord(Record record, Validator validator)
        {
            validator.CheckRequired(record, "NAME");
            validator.CheckRequired(record, "MODL");
            validator.CheckRequired(record, "FNAM");
            validator.CheckRequired(record, "BYDT");
        }

        public override TES3GameItem Copy()
        {
            return new BodyPart(Name)
            {
                Model = Model,
                Race = Race,
                SkinType = SkinType,
                Type = Type,
                Part = Part,
                Female = Female,
                Playable = Playable,
                Deleted = Deleted
            };
        }

        public override void StreamDebug(TextWriter target)
        {
            var writer = new IndentWriter(target);

            writer.WriteLine(ToString());

            writer.IncIndent();
            writer.WriteLine($"Type: {Type}");
            writer.WriteLine($"Part: {Part}");
            writer.WriteLine($"Model: {Model}");
            writer.WriteLine($"Female: {Female}");
            writer.WriteLine($"Playable: {Playable}");

            writer.WriteLine("Skin Information");
            writer.IncIndent();
            writer.WriteLine($"Race: {Race}");
            writer.WriteLine($"Type: {SkinType}");
            writer.DecIndent();
            writer.DecIndent();
        }

        public override string ToString()
        {
            return $"Body Part ({Name})";
        }

        static void BodyDataFlagSet(BodyPartData subRecord, bool value, byte flag)
        {
            if (value)
            {
                subRecord.Flags |= flag;
            }
            else
            {
                subRecord.Flags &= (byte) ~flag;
            }
        }
    }
}
