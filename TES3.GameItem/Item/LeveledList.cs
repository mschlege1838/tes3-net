
using System;
using System.Collections.Generic;
using TES3.GameItem.Part;
using TES3.Records;
using TES3.Records.SubRecords;
using TES3.Util;

namespace TES3.GameItem.Item
{

    public abstract class LeveledList : TES3GameItem
    {
        const int CALC_FROM_PC_LEVEL_FLAG = 0x01;

        public LeveledList(string name) : base(name)
        {
            
        }

        public LeveledList(Record record) : base(record)
        {
            
        }

        protected abstract string ListItemName { get; }

        [IdField]
        public string Name
        {
            get => (string) Id;
            set => Id = value;
        }

        public bool CalcFromPCLevel
        {
            get;
            set;
        }

        public byte ChanceNone
        {
            get;
            set;
        }

        public IList<LeveledListItem> ItemList
        {
            get;
        } = new List<LeveledListItem>();

        public bool Deleted
        {
            get;
            set;
        }

        protected override void OnCreate(IList<SubRecord> subRecords)
        {
            subRecords.Add(new StringSubRecord("NAME", Name));

            var flags = 0;
            if (CalcFromPCLevel)
            {
                flags |= CALC_FROM_PC_LEVEL_FLAG;
            }
            var dataSubRecord = new IntSubRecord("DATA", flags);
            UpdateData(dataSubRecord);
            subRecords.Add(dataSubRecord);

            subRecords.Add(new ByteSubRecord("NNAM", ChanceNone));
        }

        protected override void UpdateRequired(Record record)
        {
            record.GetSubRecord<StringSubRecord>("NAME").Data = Name;
            record.GetSubRecord<ByteSubRecord>("NNAM").Data = ChanceNone;

            var dataSubRecord = record.GetSubRecord<IntSubRecord>("DATA");
            FlagSet(dataSubRecord, CalcFromPCLevel, CALC_FROM_PC_LEVEL_FLAG, IntFlagSet, IntFlagClear);
            UpdateData(dataSubRecord);
        }

        protected override void UpdateOptional(Record record)
        {
            // Optional
            ProcessOptional(record, "DELE", Deleted, () => new IntSubRecord("DELE", 0), (sr) => sr.Data = 0);
            ProcessOptional(record, "INDX", ItemList.Count > 0, () => new IntSubRecord("INDX", ItemList.Count), (sr) => sr.Data = ItemList.Count);

            // Collection
            UpdateCollection(record, ItemList, ListItemName, new string[] { ListItemName, "INTV" },
                delegate (ref int index, LeveledListItem item)
                {
                    record.InsertSubRecordAt(index++, new StringSubRecord(ListItemName, item.Name));
                    record.InsertSubRecordAt(index++, new ShortSubRecord("INTV", item.PCLevel));
                }
            );
        }

        protected override void DoSyncWithRecord(Record record)
        {
            // Required
            Id = record.GetSubRecord<StringSubRecord>("NAME").Data;
            ChanceNone = record.GetSubRecord<ByteSubRecord>("NNAM").Data;

            var dataSubRecord = record.GetSubRecord<IntSubRecord>("DATA");
            CalcFromPCLevel = HasFlagSet(dataSubRecord.Data, CALC_FROM_PC_LEVEL_FLAG);
            ProcessData(dataSubRecord);

            // Optional
            Deleted = record.ContainsSubRecord("DELE");

            // Collection
            ItemList.Clear();
            {
                var enumerator = record.GetEnumerableFor(ListItemName, "INTV");
                while (enumerator.MoveNext())
                {
                    var name = ((StringSubRecord) enumerator.Current).Data;
                    if (!enumerator.MoveNext())
                    {
                        throw new InvalidOperationException("Expected PC Level Sub-Record to follow Item Name Sub-Record.");
                    }

                    var pcLevel = ((ShortSubRecord) enumerator.Current).Data;

                    ItemList.Add(new LeveledListItem(name, pcLevel));
                }
            }
        }

        protected override void DoValidateRecord(Record record, Validator validator)
        {
            validator.CheckRequired(record, "NAME");
            validator.CheckRequired(record, "DATA");
            validator.CheckRequired(record, "NNAM");
        }

        protected virtual void UpdateData(IntSubRecord subRecord)
        {

        }

        protected virtual void ProcessData(IntSubRecord subRecord)
        {

        }

        protected void CopyClone(LeveledList other)
        {
            other.CalcFromPCLevel = CalcFromPCLevel;
            other.ChanceNone = ChanceNone;
            other.Deleted = Deleted;

            CollectionUtils.Copy(ItemList, other.ItemList);
        }

    }
}
