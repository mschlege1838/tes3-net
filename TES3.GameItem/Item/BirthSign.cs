
using System.Collections.Generic;
using System.IO;
using TES3.Records;
using TES3.Records.SubRecords;
using TES3.Util;

namespace TES3.GameItem.Item
{
 
    [TargetRecord("BSGN")]
    public class BirthSign : TES3GameItem
    {


        public BirthSign(string name) : base(name)
        {
            
        }

        public BirthSign(Record record) : base(record)
        {
            
        }

        public override string RecordName => "BSGN";

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

        public string TextureName
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public bool Deleted
        {
            get;
            set;
        }

        public IList<string> Abilities
        {
            get;
        } = new List<string>();

        protected override void OnCreate(IList<SubRecord> subRecords)
        {
            subRecords.Add(new StringSubRecord("NAME", Name));
        }

        protected override void UpdateRequired(Record record)
        {
            record.GetSubRecord<StringSubRecord>("NAME").Data = Name;
        }

        protected override void UpdateOptional(Record record)
        {
            ProcessOptional(record, "FNAM", DisplayName != null, () => new StringSubRecord("FNAM", DisplayName), (sr) => sr.Data = DisplayName);
            ProcessOptional(record, "TNAM", TextureName != null, () => new StringSubRecord("TNAM", TextureName), (sr) => sr.Data = TextureName);
            ProcessOptional(record, "DESC", Description != null, () => new StringSubRecord("DESC", Description), (sr) => sr.Data = Description);
            ProcessOptional(record, "DELE", Deleted, () => new IntSubRecord("DELE", 0), (sr) => sr.Data = 0);

            UpdateCollection(record, Abilities, "NPCS",
                delegate (ref int index, string item)
                {
                    record.InsertSubRecordAt(index++, new StringSubRecord("NPCS", item));
                }
            );
        }

        protected override void DoSyncWithRecord(Record record)
        {
            // Required
            Id = record.GetSubRecord<StringSubRecord>("NAME").Data;

            // Optional
            DisplayName = record.TryGetSubRecord<StringSubRecord>("FNAM")?.Data;
            TextureName = record.TryGetSubRecord<StringSubRecord>("TNAM")?.Data;
            Description = record.TryGetSubRecord<StringSubRecord>("DESC")?.Data;
            Deleted = record.ContainsSubRecord("DELE");

            // Collection
            Abilities.Clear();
            foreach (StringSubRecord subRecord in record.GetEnumerableFor("NPCS"))
            {
                Abilities.Add(subRecord.Data);
            }
        }

        protected override void DoValidateRecord(Record record, Validator validator)
        {
            validator.CheckRequired(record, "NAME");
        }

        public override TES3GameItem Clone()
        {
            var result = new BirthSign(Name)
            {
                DisplayName = DisplayName,
                TextureName = TextureName,
                Description = Description,
                Deleted = Deleted
            };

            CollectionUtils.Copy(Abilities, result.Abilities);
            return result;
        }

        public override void StreamDebug(TextWriter target)
        {
            var writer = new IndentWriter(target);

            writer.WriteLine(ToString());

            writer.IncIndent();
            writer.WriteLine($"Name: {DisplayName}");
            writer.WriteLine($"Texture: {TextureName}");
            writer.WriteLine($"Description: {Description}");
            writer.DecIndent();

        }

        public override string ToString()
        {
            return $"Birthsign ({Name})";
        }

    }
}
