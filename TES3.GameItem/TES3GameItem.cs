

using System;
using System.Collections.Generic;
using System.IO;
using TES3.Core;
using TES3.Records;
using TES3.Records.SubRecords;
using TES3.Util;

namespace TES3.GameItem
{
    public abstract class TES3GameItem : ICopyable<TES3GameItem>
    {

        object id;

        public TES3GameItem(object id)
        {
            Init();
            Id = Validation.NotNull(id, "id", "Id");
        }

        public TES3GameItem(Record record)
        {
            Init();
            SyncWithRecord(record);
            if (Id == null)
            {
                throw new InvalidOperationException("Initialized TES3GameItem with record, but Id is still null.");
            }
        }

        public abstract string RecordName { get; }

        public bool Blocked
        {
            get;
            set;
        }

        public bool Persistent
        {
            get;
            set;
        }

        public object Id
        {
            get => id;
            set
            { 
                var oldId = id;
                id = Validation.NotNull(value, "value", "Id");
                if (oldId == null || !oldId.Equals(id))
                {
                    IdChanged?.Invoke(this, new IdChangedEventArgs(oldId, value));
                }
            }
        }

        public event EventHandler<IdChangedEventArgs> IdChanged;

        public void UpdateRecord(Record record)
        {
            ValidateRecord(record);
            record.Blocked = Blocked;
            record.Persistent = Persistent;
            UpdateRequired(record);
            UpdateOptional(record);
        }

        public void SyncWithRecord(Record record)
        {
            ValidateRecord(record);
            Blocked = record.Blocked;
            Persistent = record.Persistent;
            DoSyncWithRecord(record);
        }

        public Record CreateRecord()
        {

            var subRecords = new List<SubRecord>();
            OnCreate(subRecords);


            var recordFlags = 0;
            if (Blocked)
            {
                recordFlags |= Record.BLOCKED_FLAG;
            }
            if (Persistent)
            {
                recordFlags |= Record.PERSISTENT_FLAG;
            }

            var record = new Record(RecordName, 0, recordFlags, subRecords);

            UpdateOptional(record);

            return record;
        }

        void ValidateRecord(Record record)
        {
            if (record.Name != RecordName)
            {
                throw new TES3ValidationException($"Expected record with name {RecordName}: {record.Name}");
            }

            var errors = new List<string>();
            var warnings = new List<string>();
            var validator = new Validator(errors, warnings);

            DoValidateRecord(record, validator);

            // TODO Decide what to do with warnings.
            if (warnings.Count > 0)
            {
                foreach (var warning in warnings)
                {
                    TES3Logger.Log(TES3LogLevel.Warn, warning);
                }
            }
            if (errors.Count > 0)
            {
                throw new TES3ValidationException(errors);
            }

        }


        protected abstract void OnCreate(IList<SubRecord> subRecords);

        protected abstract void UpdateRequired(Record record);

        protected abstract void UpdateOptional(Record record);

        protected abstract void DoSyncWithRecord(Record record);

        protected abstract void DoValidateRecord(Record record, Validator validator);

        public abstract void StreamDebug(TextWriter target);

        public abstract TES3GameItem Copy();

        protected virtual void Init()
        {

        }


        protected delegate void DoFlagSet<T>(T subRecord, int flag) where T : SubRecord;

        protected static void FlagSet<T>(T subRecord, bool value, int flag, DoFlagSet<T> set, DoFlagSet<T> clear) where T : SubRecord
        {
            if (value)
            {
                set(subRecord, flag);
            }
            else
            {
                clear(subRecord, flag);
            }
        }

        protected static void IntFlagSet(IntSubRecord subRecord, int flag)
        {
            subRecord.Data |= flag;
        }

        protected static void IntFlagClear(IntSubRecord subRecord, int flag)
        {
            subRecord.Data &= ~flag;
        }

        protected static bool HasFlagSet(int flags, int flag)
        {
            return (flags & flag) != 0;
        }

        

        protected delegate T ProcessOptionalCreate<T>() where T : SubRecord;
        protected delegate void ProcessOptionalUpdate<T>(T subRecord) where T : SubRecord;
        protected static void ProcessOptional<T>(Record record, string subRecordName, bool valuePresent, ProcessOptionalCreate<T> create, ProcessOptionalUpdate<T> update) where T : SubRecord
        {
            if (record.ContainsSubRecord(subRecordName))
            {
                var subRecord = record.GetSubRecord<T>(subRecordName);
                if (valuePresent)
                {
                    update(subRecord);
                }
                else
                {
                    record.RemoveSubRecord(subRecord);
                }
            }
            else
            {
                if (valuePresent)
                {
                    record.InsertSubRecordAt(record.GetAddIndex(subRecordName), create());
                }
            }
        }



        protected delegate void UpdateCollectionAdd<T>(ref int index, T item);
        protected static void UpdateCollection<T>(Record record, IList<T> collection, string subRecordName, UpdateCollectionAdd<T> add)
        {
            record.RemoveAllSubRecords(subRecordName);
            var targetIndex = record.GetAddIndex(subRecordName);
            foreach (var item in collection)
            {
                add(ref targetIndex, item);
            }
        }

        protected static void UpdateCollection<T>(Record record, IList<T> collection, string addTargetName, string[] subRecordNames, UpdateCollectionAdd<T> add)
        {
            record.RemoveAllSubRecords(subRecordNames);
            var targetIndex = record.GetAddIndex(addTargetName);
            foreach (var item in collection)
            {
                add(ref targetIndex, item);
            }
        }


        protected class Validator
        {
            readonly IList<string> errors;
            readonly IList<string> warnings;

            internal Validator(IList<string> errors, IList<string> warnings)
            {
                this.errors = errors;
                this.warnings = warnings;
            }

            internal void CheckRequired(Record record, string subRecordName)
            {
                if (!record.ContainsSubRecord(subRecordName))
                {
                    errors.Add($"The sub-record {subRecordName} is required for the record {record}");
                }
            }

            internal void WarnRequired(Record record, string subRecordName)
            {
                if (!record.ContainsSubRecord(subRecordName))
                {
                    errors.Add($"The sub-record {subRecordName} is normally created for the record {record}, but is not present.");
                }
            }

            internal void CheckCount(Record record, string subRecordName, int max)
            {
                var count = record.GetSubRecordCount(subRecordName);
                if (count > max)
                {
                    errors.Add($"Cannot have more than {max} sub-records of the name {subRecordName}: {count}");
                }
            }

            internal void WarnCount(Record record, string subRecordName, int max)
            {
                var count = record.GetSubRecordCount(subRecordName);
                if (count > max)
                {
                    warnings.Add($"Only {max} sub-records of the name {subRecordName} are recognized by the game; the remainder will be truncated: {count}");
                }
            }

            internal void CheckAny(Record record, params string[] subRecordNames)
            {
                
                if (!record.ContainsAnySubRecords(subRecordNames))
                {
                    errors.Add($"The record {record} requires at least one of the following sub-records: {string.Join(", ", subRecordNames)}. Has: {string.Join(", ", record.SubRecordNames)}.");
                }
            }

            internal void AddError(string error)
            {
                errors.Add(error);
            }
        }


    }


    public class IdChangedEventArgs
    {

        internal IdChangedEventArgs(object oldId, object newId)
        {
            OldId = oldId;
            NewId = newId;
        }

        public object OldId
        {
            get;
        }

        public object NewId
        {
            get;
        }

    }

}