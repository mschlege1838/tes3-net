
using System.Collections.Generic;
using TES3.Records;
using TES3.GameItem.Part;
using TES3.GameItem.TypeConstant;
using TES3.Records.SubRecords;
using TES3.Util;

namespace TES3.GameItem.Item
{

    public abstract class Apparel : TES3GameItem
    {

		string model;

        public Apparel(string name) : base(name)
        {
			Model = Constants.DefaultModelValue;
        }

        public Apparel(Record record) : base(record)
        {
            
        }

        protected abstract string DataSubRecordName { get; }

        [IdField]
        public string Name
        {
            get => (string) Id;
            set => Id = value;
        }

        public virtual string DisplayName
        {
            get;
            set;
        }

        public string Model
        {
            get => model;
            set => model = Validation.NotNull(value, "value", "Model").Length > 0 ? value : Constants.DefaultModelValue;
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

        public string Enchantment
        {
            get;
            set;
        }

        public bool Deleted
        {
            get;
            set;
        }

        public float Weight
        {
            get;
            set;
        }


        protected abstract SubRecord CreateDataSubRecord();

        public IList<ApparelBipedPart> BipedParts
        {
            get;
        } = new List<ApparelBipedPart>();

        protected override void OnCreate(IList<SubRecord> subRecords)
        {
            subRecords.Add(new StringSubRecord("NAME", Name));
            subRecords.Add(new StringSubRecord("MODL", Model));
            if (DisplayName != null)
            {
                subRecords.Add(new StringSubRecord("FNAM", DisplayName));
            }
            subRecords.Add(CreateDataSubRecord());
        }

        protected override void UpdateRequired(Record record)
        {
            record.GetSubRecord<StringSubRecord>("NAME").Data = Name;
            record.GetSubRecord<StringSubRecord>("MODL").Data = Model;
        }

        protected override void UpdateOptional(Record record)
        {
            // Optional
            ProcessOptional(record, "FNAM", DisplayName != null, () => new StringSubRecord("FNAM", DisplayName), (sr) => sr.Data = DisplayName);
            ProcessOptional(record, "ITEX", Icon != null, () => new StringSubRecord("ITEX", Icon), (sr) => sr.Data = Icon);
            ProcessOptional(record, "SCRI", Script != null, () => new StringSubRecord("SCRI", Script), (sr) => sr.Data = Script);
            ProcessOptional(record, "ENAM", Enchantment != null, () => new StringSubRecord("ENAM", Enchantment), (sr) => sr.Data = Enchantment);
            ProcessOptional(record, "DELE", Deleted, () => new IntSubRecord("DELE", 0), (sr) => sr.Data = 0);

            // Collection
            UpdateCollection(record, BipedParts, "INDX", new string[] { "INDX", "BNAM", "CNAM" },
                delegate (ref int index, ApparelBipedPart item)
                {
                    record.InsertSubRecordAt(index++, new ByteSubRecord("INDX", (byte) item.Type));
                    if (item.MaleBodyPart != null)
                    {
                        record.InsertSubRecordAt(index++, new StringSubRecord("BNAM", item.MaleBodyPart));
                    }
                    if (item.FemaleBodyPart != null)
                    {
                        record.InsertSubRecordAt(index++, new StringSubRecord("CNAM", item.FemaleBodyPart));
                    }
                }
            );
        }

        protected override void DoSyncWithRecord(Record record)
        {
            // Required
            Id = record.GetSubRecord<StringSubRecord>("NAME").Data;
            Model = record.GetSubRecord<StringSubRecord>("MODL").Data;

            // Optional
            DisplayName = record.TryGetSubRecord<StringSubRecord>("FNAM")?.Data;
            Icon = record.TryGetSubRecord<StringSubRecord>("ITEX")?.Data;
            Script = record.TryGetSubRecord<StringSubRecord>("SCRI")?.Data;
            Enchantment = record.TryGetSubRecord<StringSubRecord>("ENAM")?.Data;
            Deleted = record.ContainsSubRecord("DELE");

            // Collection
            BipedParts.Clear();
            {
                var enumerator = record.GetEnumerableFor("INDX", "BNAM", "CNAM");
                while (enumerator.MoveNext())
                {
                    var type = (ApparelBipedPartType) ((ByteSubRecord) enumerator.Current).Data;

                    string maleBipedPart = null;
                    string femaleBipedPart = null;
                    for (var i = 0; i < 2; ++i)
                    {
                        if (!enumerator.HasNext())
                        {
                            break;
                        }

                        var next = enumerator.PeekNext();
                        if (next.Name == "BNAM")
                        {
                            maleBipedPart = ((StringSubRecord) next).Data;
                            enumerator.MoveNext();
                        }
                        else if (next.Name == "CNAM")
                        {
                            femaleBipedPart = ((StringSubRecord) next).Data;
                            enumerator.MoveNext();
                        }
                    }

                    BipedParts.Add(new ApparelBipedPart(type, maleBipedPart, femaleBipedPart));
                }
            }
        }

        protected override void DoValidateRecord(Record record, Validator validator)
        {
            validator.CheckRequired(record, "NAME");
            validator.CheckRequired(record, "MODL");
            validator.CheckRequired(record, DataSubRecordName);
        }

        protected void CopyClone(Apparel clone)
        {
            clone.DisplayName = DisplayName;
            clone.Model = Model;
            clone.Icon = Icon;
            clone.Script = Script;
            clone.Enchantment = Enchantment;
            clone.Deleted = Deleted;
            clone.Weight = Weight;

            CollectionUtils.Copy(BipedParts, clone.BipedParts);
        }
    }
}
