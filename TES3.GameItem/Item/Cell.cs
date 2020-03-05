
using System.Collections.Generic;
using System.IO;
using TES3.Core;
using TES3.GameItem.Part;
using TES3.Records;
using TES3.Records.SubRecords;
using TES3.Util;

namespace TES3.GameItem.Item
{
    public abstract class Cell : TES3GameItem
    {

        protected const int HAS_WATER_FLAG = 0x02;
        protected const int SLEEPING_ILLEGAL_FLAG = 0x04;
        protected const int LIKE_EXTERIOR_FLAG = 0x80;

        public Cell(object id) : base(id)
        {

        }

        public Cell(Record record) : base(record)
        {
            
        }

        public override string RecordName => "CELL";

        public abstract string DisplayName
        {
            get;
        }

        public abstract bool IsInterior
        {
            get;
        }

        public bool SleepingIllegal
        {
            get;
            set;
        }

        public bool Deleted
        {
            get;
            set;
        }



        public IList<CellReferenceObject> ObjectReferences
        {
            get;
        } = new List<CellReferenceObject>();


        protected override void OnCreate(IList<SubRecord> subRecords)
        {
            
        }

        protected override void UpdateRequired(Record record)
        {

        }

        protected override void UpdateOptional(Record record)
        {
            ProcessOptional(record, "DELE", Deleted, () => new IntSubRecord("DELE", 0), (sr) => sr.Data = 0);

            // Reference Sub-Records
            record.RemoveAllSubRecords("NAM0");

            var targetIndex = -1;
            foreach (var subRecord in record)
            {
                if (subRecord.Name == "FRMR" || subRecord.Name == "MVRF")
                {
                    targetIndex = record.GetSubRecordIndex(subRecord);
                    break;
                }
            }
            if (targetIndex > -1)
            {
                while (targetIndex < record.Count)
                {
                    record.RemoveSubRecordAt(targetIndex);
                }
            }
            else
            {
                targetIndex = record.GetAddIndex("FRMR", "MVRF");
            }

            record.InsertSubRecordAt(targetIndex++, new IntSubRecord("NAM0", ObjectReferences.Count));
            foreach (var objectReference in ObjectReferences)
            {
                if (objectReference.Moved)
                {
                    record.InsertSubRecordAt(targetIndex++, new IntSubRecord("MVRF", objectReference.Index));
                    record.InsertSubRecordAt(targetIndex++, new GridSubRecord("CNDT", objectReference.MovedGridX, objectReference.MovedGridY));
                }
                record.InsertSubRecordAt(targetIndex++, new IntSubRecord("FRMR", objectReference.Index));
                record.InsertSubRecordAt(targetIndex++, new StringSubRecord("NAME", objectReference.ObjectName));
                record.InsertSubRecordAt(targetIndex++, new PositionSubRecord("DATA", objectReference.Position.Copy()));

                if (objectReference.Scale != 1.0f)
                {
                    record.InsertSubRecordAt(targetIndex++, new FloatSubRecord("XSCL", objectReference.Scale));
                }
                if (objectReference.ExitPosition != null)
                {
                    record.InsertSubRecordAt(targetIndex++, new PositionSubRecord("DODT", objectReference.ExitPosition.Copy()));
                }
                DoStringInsert(record, ref targetIndex, objectReference.ExitCell, "DNAM");
                DoIntInsert(record, ref targetIndex, objectReference.LockLevel, "FLTV");
                DoStringInsert(record, ref targetIndex, objectReference.Key, "KNAM");
                DoStringInsert(record, ref targetIndex, objectReference.Trap, "TNAM");
                if (objectReference.Blocked)
                {
                    record.InsertSubRecordAt(targetIndex++, new ByteSubRecord("UNAM", 1));
                }
                DoStringInsert(record, ref targetIndex, objectReference.Owner, "ANAM");
                DoStringInsert(record, ref targetIndex, objectReference.OwnerGlobal, "BNAM");
                DoStringInsert(record, ref targetIndex, objectReference.OwningFaction, "CNAM");
                DoIntInsert(record, ref targetIndex, objectReference.RequiredRank, "INDX");
                if (objectReference.RawHealth != null)
                {
                    record.InsertSubRecordAt(targetIndex++, new GenericSubRecord("INTV", objectReference.RawHealth));
                }
                DoIntInsert(record, ref targetIndex, objectReference.ObjectCount, "NAM9");
                DoStringInsert(record, ref targetIndex, objectReference.Soul, "XSOL");
                if (objectReference.Charge != 0)
                {
                    record.InsertSubRecordAt(targetIndex++, new FloatSubRecord("XCHG", objectReference.Charge));
                }
            }

            

        }

        protected override void DoSyncWithRecord(Record record)
        {
            // Required
            Deleted = record.ContainsSubRecord("DELE");

            // Collection
            // Reference Sub-Records
            ObjectReferences.Clear();
            CellReferenceObject current = null;
            foreach (var subRecord in record)
            {
                if (subRecord.Name == "MVRF")
                {
                    if (current != null)
                    {
                        ObjectReferences.Add(current);
                    }

                    current = new CellReferenceObject() { Index = ((IntSubRecord) subRecord).Data, Moved = true };
                }
                if (subRecord.Name == "FRMR")
                {
                    var index = ((IntSubRecord) subRecord).Data;
                    if (current != null)
                    {
                        if (current.Index == index)
                        {
                            continue;
                        }

                        ObjectReferences.Add(current);
                    }

                    current = new CellReferenceObject() { Index = index };
                }

                if (current == null)
                {
                    continue;
                }

                switch (subRecord.Name)
                {
                    case "CNDT":
                    {
                        var gridSubRecord = (GridSubRecord) subRecord;
                        current.MovedGridX = gridSubRecord.GridX;
                        current.MovedGridY = gridSubRecord.GridY;
                        break;
                    }
                    case "NAME":
                        current.ObjectName = ((StringSubRecord) subRecord).Data;
                        break;
                    case "XSCL":
                        current.Scale = ((FloatSubRecord) subRecord).Data;
                        break;
                    case "DODT":
                        current.ExitPosition = ((PositionSubRecord) subRecord).Data.Copy();
                        break;
                    case "DNAM":
                        current.ExitCell = ((StringSubRecord) subRecord).Data;
                        break;
                    case "FLTV":
                        current.LockLevel = ((IntSubRecord) subRecord).Data;
                        break;
                    case "KNAM":
                        current.Key = ((StringSubRecord) subRecord).Data;
                        break;
                    case "TNAM":
                        current.Trap = ((StringSubRecord) subRecord).Data;
                        break;
                    case "UNAM":
                        current.Blocked = ((ByteSubRecord) subRecord).Data == 1;
                        break;
                    case "ANAM":
                        current.Owner = ((StringSubRecord) subRecord).Data;
                        break;
                    case "BNAM":
                        current.OwnerGlobal = ((StringSubRecord) subRecord).Data;
                        break;
                    case "CNAM":
                        current.OwningFaction = ((StringSubRecord) subRecord).Data;
                        break;
                    case "INDX":
                        current.RequiredRank = ((IntSubRecord) subRecord).Data;
                        break;
                    case "INTV":
                        current.RawHealth = ((GenericSubRecord) subRecord).Data;
                        break;
                    case "NAM9":
                        current.ObjectCount = ((IntSubRecord) subRecord).Data;
                        break;
                    case "XSOL":
                        current.Soul = ((StringSubRecord) subRecord).Data;
                        break;
                    case "DATA":
                        current.Position = ((PositionSubRecord) subRecord).Data.Copy();
                        break;
                    case "XCHG":
                        current.Charge = ((FloatSubRecord) subRecord).Data;
                        break;
                }
            }
        }

        protected override void DoValidateRecord(Record record, Validator validator)
        {
            validator.CheckRequired(record, "NAME");
            validator.CheckRequired(record, "DATA");
        }

        protected abstract void StreamSpecific(IndentWriter writer);

        public override void StreamDebug(TextWriter target)
        {
            var writer = new IndentWriter(target);

            writer.WriteLine(ToString());

            writer.IncIndent();
            StreamSpecific(writer);
            writer.WriteLine($"Sleeping Illegal: {SleepingIllegal}");

            if (ObjectReferences.Count > 0)
            {
                writer.WriteLine("Object References");
                writer.IncIndent();

                var table = new Table("Index", "Object Name", "Position", "Scale", "Blocked", "Exit Position", "Exit Cell", "Lock Level",
                    "Key", "Trap", "Owner", "Owner Global/Rank", "Stack Count", "Soul", "Charge", "Health As Int", "Health As Float");
                foreach (var reference in ObjectReferences)
                {
                    var exitPosition = reference.ExitPosition;

                    string exitCell;
                    if (exitPosition != null)
                    {
                        exitCell = reference.ExitCell ?? $"({exitPosition.GridX}, {exitPosition.GridY})";
                    }
                    else
                    {
                        exitCell = null;
                    }

                    var owner = reference.Owner;
                    object ownerExtra = null;
                    if (owner != null)
                    {
                        ownerExtra = reference.OwnerGlobal;
                    }
                    else
                    {
                        owner = reference.OwningFaction;
                        if (owner != null)
                        {
                            ownerExtra = reference.RequiredRank;
                        }
                    }

                    table.AddRow(reference.Index, reference.ObjectName, reference.Position, reference.Scale, reference.Blocked,
                        exitPosition, exitCell, reference.LockLevel, reference.Key, reference.Trap, owner, ownerExtra, reference.ObjectCount,
                        reference.Soul, reference.Charge, reference.IntHealth, reference.FloatHealth);
                }
                table.Print(writer);

                writer.DecIndent();
            }

            writer.DecIndent();
        }

        protected void CopyClone(Cell clone)
        {
            clone.SleepingIllegal = SleepingIllegal;
            clone.Deleted = Deleted;

            CollectionUtils.Copy(ObjectReferences, clone.ObjectReferences);
        }

        public override string ToString()
        {
            return $"Cell ({DisplayName})";
        }


        protected static void CellDataFlagSet(CellData subRecord, int flag)
        {
            subRecord.Flags |= flag;
        }

        protected static void CellDataFlagClear(CellData subRecord, int flag)
        {
            subRecord.Flags &= ~flag;
        }

        static void DoStringInsert(Record record, ref int targetIndex, string value, string name)
        {
            if (value != null)
            {
                record.InsertSubRecordAt(targetIndex++, new StringSubRecord(name, value));
            }
        }

        static void DoIntInsert(Record record, ref int targetIndex, int value, string name)
        {
            if (value != 0)
            {
                record.InsertSubRecordAt(targetIndex++, new IntSubRecord(name, value));
            }
        }

    }
}
