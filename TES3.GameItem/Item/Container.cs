
using System.Collections.Generic;
using System.IO;
using TES3.GameItem.Part;
using TES3.Core;
using TES3.Records;
using TES3.Records.SubRecords;
using TES3.Util;

namespace TES3.GameItem.Item
{

    [TargetRecord("CONT")]
    public class Container : TES3GameItem
    {
        const int ORGANIC_FLAG = 0x0001;
        const int RESPAWN_FLAG = 0x0002;

		string model;

        public Container(string name) : base(name)
        {
			Model = Constants.DefaultModelValue;
        }
        public Container(Record record) : base(record)
        {
            
        }

        public override string RecordName => "CONT";

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

        public bool Organic
        {
            get;
            set;
        }

        public bool Respawns
        {
            get;
            set;
        }

        public bool Deleted
        {
            get;
            set;
        }

        public IList<InventoryItem> InventoryList
        {
            get;
        } = new List<InventoryItem>();

        protected override void OnCreate(IList<SubRecord> subRecords)
        {
            subRecords.Add(new StringSubRecord("NAME", Name));
            subRecords.Add(new StringSubRecord("MODL", Model));
            subRecords.Add(new FloatSubRecord("CNDT", Weight));

            var flags = 0;
            if (Organic)
            {
                flags |= ORGANIC_FLAG;
            }
            if (Respawns)
            {
                flags |= RESPAWN_FLAG;
            }
            subRecords.Add(new IntSubRecord("FLAG", flags));
        }

        protected override void UpdateRequired(Record record)
        {
            record.GetSubRecord<StringSubRecord>("NAME").Data = Name;
            record.GetSubRecord<StringSubRecord>("MODL").Data = Model;
            record.GetSubRecord<FloatSubRecord>("CNDT").Data = Weight;

            var flags = record.GetSubRecord<IntSubRecord>("FLAG");
            FlagSet(flags, Organic, ORGANIC_FLAG, IntFlagSet, IntFlagClear);
            FlagSet(flags, Respawns, RESPAWN_FLAG, IntFlagSet, IntFlagClear);
        }

        protected override void UpdateOptional(Record record)
        {
            ProcessOptional(record, "FNAM", DisplayName != null, () => new StringSubRecord("FNAM", DisplayName), (sr) => sr.Data = DisplayName);
            ProcessOptional(record, "DELE", Deleted, () => new IntSubRecord("DELE", 0), (sr) => sr.Data = 0);

            // Collection
            UpdateCollection(record, InventoryList, "NPCO",
                delegate (ref int index, InventoryItem item)
                {
                    record.InsertSubRecordAt(index++, new InventoryItemSubRecord("NPCO", item.Count, item.Name));
                }
            );
        }

        protected override void DoSyncWithRecord(Record record)
        {
            // Required
            Id = record.GetSubRecord<StringSubRecord>("NAME").Data;
            Model = record.GetSubRecord<StringSubRecord>("MODL").Data;
            Weight = record.GetSubRecord<FloatSubRecord>("CNDT").Data;

            var flags = record.GetSubRecord<IntSubRecord>("FLAG");
            Organic = HasFlagSet(flags.Data, ORGANIC_FLAG);
            Respawns = HasFlagSet(flags.Data, RESPAWN_FLAG);

            // Optional
            DisplayName = record.GetSubRecord<StringSubRecord>("FNAM")?.Data;
            Deleted = record.ContainsSubRecord("DELE");

            // Collection
            InventoryList.Clear();
            foreach (InventoryItemSubRecord item in record.GetEnumerableFor("NPCO"))
            {
                InventoryList.Add(new InventoryItem(item.ItemName, item.Count));
            }
        }

        protected override void DoValidateRecord(Record record, Validator validator)
        {
            validator.CheckRequired(record, "NAME");
            validator.CheckRequired(record, "MODL");
            validator.CheckRequired(record, "CNDT");
            validator.CheckRequired(record, "FLAG");
        }

        public override TES3GameItem Clone()
        {
            var result = new Container(Name)
            {
                Model = Model,
                DisplayName = DisplayName,
                Weight = Weight,
                Organic = Organic,
                Respawns = Respawns,
                Deleted = Deleted,
            };

            CollectionUtils.Copy(InventoryList, result.InventoryList);
            return result;
        }

        public override void StreamDebug(TextWriter target)
        {
            var writer = new IndentWriter(target);
            writer.WriteLine(ToString());

            writer.IncIndent();
            writer.WriteLine($"Name: {DisplayName}");
            writer.WriteLine($"Model: {Model}");
            writer.WriteLine($"Weight: {Weight}");
            writer.WriteLine($"Organic: {Organic}");
            writer.WriteLine($"Respawns: {Respawns}");
            writer.WriteLine($"Inventory");

            if (InventoryList.Count > 0)
            {
                writer.IncIndent();
                var table = new Table("Name", "Count");
                foreach (var item in InventoryList)
                {
                    table.AddRow(item.Name, item.Count);
                }
                table.Print(target, writer.Indent);
                writer.DecIndent();
            }

            writer.DecIndent();
        }

        public override string ToString()
        {
            return $"Container ({Name})";
        }
    }


}
