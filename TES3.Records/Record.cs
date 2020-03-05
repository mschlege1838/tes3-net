
using System;
using System.Collections.Generic;
using TES3.Core;
using TES3.Records.Processing;
using TES3.Records.SubRecords;
using TES3.Util;


namespace TES3.Records
{

    public class Record : AbstractEnumerable<SubRecord>, IRecord<SubRecord>, INamed
	{

        public const int BLOCKED_FLAG = 0x0002000;
        public const int PERSISTENT_FLAG = 0x00000400;


        public static readonly object TES3HeaderIdentifier = new object();


        public string Name { get; }
        public int Flags0 { get; set; }

        public int Flags { get; set; }


		readonly IList<SubRecord> subRecords;
        readonly IDictionary<string, IList<SubRecord>> subRecordIndex = new Dictionary<string, IList<SubRecord>>();


        public Record(string name, int flags0, int flags, IList<SubRecord> subRecords)
		{
            if (name == null)
            {
                throw new ArgumentNullException("name", "Name cannot be null.");
            }
            if (name.Length != 4)
            {
                throw new ArgumentOutOfRangeException("name", name.Length, "Record names must be 4 characters.");
            }

            Name = name;
            Flags0 = flags0;
            Flags = flags;

			this.subRecords = subRecords ?? throw new ArgumentNullException("subRecords", "SubRecords cannot be null.");

            foreach (var subRecord in subRecords)
            {
                if (subRecordIndex.ContainsKey(subRecord.Name))
                {
                    subRecordIndex[subRecord.Name].Add(subRecord);
                }
                else
                {
                    var indexList = new List<SubRecord>() { subRecord };
                    subRecordIndex.Add(subRecord.Name, indexList);
                }
            }
            
		}

        public IList<SubRecord> this[string name]
        {
            get
            {
                if (!subRecordIndex.ContainsKey(name))
                {
                    throw new KeyNotFoundException($"Sub-Record not found: {name}");
                }

                return new ReadOnlyList<SubRecord>(subRecordIndex[name]);
            }
        }

        public SubRecord this[int index]
        {
            get => subRecords[index];
            set
            {
                if (index < 0)
                {
                    throw new ArgumentOutOfRangeException("index", index, $"Index cannot be negative: {index}");
                }
                if (index >= subRecords.Count)
                {
                    throw new ArgumentOutOfRangeException("index", index, $"Attempt to access sub-record {index} of {subRecords.Count}.");
                }

                // Sub-Record Index Handling.
                var current = subRecords[index];

                // If the current and new values are the same name, simply reassign the new value to the
                // position of the current value in the index.
                if (current.Name == value.Name)
                {
                    var indexList = subRecordIndex[value.Name];
                    indexList[indexList.IndexOf(current)] = value;
                }

                // Otherwise, it means we're effectively removing a sub-record, and replacing it with another
                // one. We'll have to the same with the index.
                else
                {
                    // Remove the current record.
                    subRecordIndex[current.Name].Remove(current);

                    // Add the new.
                    if (!subRecordIndex.ContainsKey(value.Name))
                    {
                        subRecordIndex.Add(value.Name, new List<SubRecord>() { value });
                    }
                    else
                    {
                        subRecordIndex[value.Name].Add(value);
                    }
                }

                // Update value in main sub-record list.
                subRecords[index] = value;
            }
        }

        public bool Blocked
        {
            get => (Flags & BLOCKED_FLAG) != 0;
            set
            {
                if (value)
                {
                    Flags |= BLOCKED_FLAG;
                }
                else
                {
                    Flags &= ~BLOCKED_FLAG;
                }
            }
        }

        public bool Persistent
        {
            get => (Flags & PERSISTENT_FLAG) != 0;
            set
            {
                if (value)
                {
                    Flags |= PERSISTENT_FLAG;
                }
                else
                {
                    Flags &= ~PERSISTENT_FLAG;
                }
            }
        }

        public ICollection<string> SubRecordNames
        {
            get => subRecordIndex.Keys;
        }

        public int Count
        {
            get => subRecords.Count;
        }

        public int CurrentSize
        {
            get => RecordProcessing.GetCurrentSize(this);
        }

        public object GetIdentifier()
        {
            return RawUtils.ProcessRecord(RecordOperationType.Identifier, new RecordIdentifierContext(this));
        }

        public bool ContainsSubRecord(string name)
        {
            return subRecordIndex.ContainsKey(name);
        }

        public bool ContainsAnySubRecords(params string[] names)
        {
            foreach (var name in names)
            {
                if (ContainsSubRecord(name))
                {
                    return true;
                }
            }
            return false;
        }

        public T GetSubRecord<T>(string name) where T : SubRecord
        {
            if (!subRecordIndex.ContainsKey(name))
            {
                throw new KeyNotFoundException($"Sub-Record not found in Record {Name}: {name}");
            }

            var indexList = subRecordIndex[name];
            if (indexList.Count == 0)
            {
                throw new KeyNotFoundException($"Sub-Record not found in Record {Name}: {name}");
            }

            return (T) indexList[0];
        }

        public T TryGetSubRecord<T>(string name) where T : SubRecord
        {
            if (!subRecordIndex.ContainsKey(name))
            {
                return null;
            }

            var indexList = subRecordIndex[name];
            return indexList.Count > 0 ? (T) indexList[0] : null;
        }

        public int GetSubRecordIndex(SubRecord subRecord)
        {
            return subRecords.IndexOf(subRecord);
        }

        public int GetAddIndex(string name, object additionalContext = null)
        {
            return (int) RawUtils.ProcessSubRecord(SubRecordOperationType.AddIndex, new SubRecordAddIndexContext(this, name, additionalContext));
        }

        public void InsertSubRecordAt(int index, SubRecord subRecord)
        {
            if (subRecord == null)
            {
                throw new ArgumentNullException("subRecord", "Sub-Record cannot be null.");
            }
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index", index, $"Index cannot be negative: {index}");
            }
            if (index > subRecords.Count)
            {
                throw new ArgumentOutOfRangeException("index", index, $"Attempt to access sub-record {index} of {subRecords.Count}.");
            }

            if (!subRecordIndex.ContainsKey(subRecord.Name))
            {
                subRecordIndex.Add(subRecord.Name, new List<SubRecord>() { subRecord });
            }
            else
            {
                subRecordIndex[subRecord.Name].Add(subRecord);
            }

            subRecords.Insert(index, subRecord);
        }

        public int GetSubRecordCount(string name)
        {
            return subRecordIndex.ContainsKey(name) ? subRecordIndex[name].Count : 0;
        }

        public bool RemoveSubRecord(SubRecord record)
        {
            var targetIndex = GetSubRecordIndex(record);
            if (targetIndex == -1)
            {
                return false;
            }

            RemoveSubRecordAt(targetIndex);
            return true;
        }

        public void RemoveSubRecordAt(int index)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index", index, $"Index cannot be negative: {index}");
            }
            if (index >= subRecords.Count)
            {
                throw new ArgumentOutOfRangeException("index", index, $"Attempt to access sub-record {index} of {subRecords.Count}.");
            }

            var current = subRecords[index];

            var indexList = subRecordIndex[current.Name];
            indexList.Remove(current);

            subRecords.RemoveAt(index);
        }

        public void RemoveAllSubRecords(string name)
        {
            for (int i = 0; i < subRecords.Count; ++i)
            {
                var subRecord = subRecords[i];
                if (subRecord.Name != name)
                {
                    continue;
                }

                subRecordIndex[subRecord.Name].Remove(subRecord);
                subRecords.Remove(subRecord);
                --i;
            }
        }

        public void RemoveAllSubRecords(params string[] names)
        {
            for (int i = 0; i < subRecords.Count; ++i)
            {
                var subRecord = subRecords[i];
                if (Array.IndexOf(names, subRecord.Name) == -1)
                {
                    continue;
                }

                subRecordIndex[subRecord.Name].Remove(subRecord);
                subRecords.Remove(subRecord);
                --i;
            }
        }

        public override IEnumerator<SubRecord> GetEnumerator()
        {
            return subRecords.GetEnumerator();
        }

        public NameEnumerable<SubRecord, Record> GetEnumerableFor(params string[] names)
        {
            return new NameEnumerable<SubRecord, Record>(this, names);
        }

      
        public override string ToString()
        {
            return Name == "TES3" ? Name : $"{Name} ({GetIdentifier()})";
        }

        public int GetLastIndex(string name)
        {
            for (var i = subRecords.Count - 1; i >= 0; --i)
            {
                if (subRecords[i].Name == name)
                {
                    return i;
                }
            }

            return -1;
        }

        

    }

    public class RecordHeader
    {
        public const int SIZE = 16;

        public string Name { get; }
        public int Size { get; }
        public int Flags0 { get; }
        public int Flags { get; }

        public RecordHeader(string name, int size, int flags0, int flags)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name", "Name cannot be null.");
            }
            if (name.Length != 4)
            {
                throw new ArgumentOutOfRangeException("name", name.Length, "Record names must be 4 characters.");
            }
            if (size < 0)
            {
                throw new ArgumentOutOfRangeException("Size", size, "Size cannot be less than 0.");
            }

            Name = name;
            Size = size;
            Flags0 = flags0;
            Flags = flags;
        }


        public override string ToString()
        {
            return string.Format("{0}: ({1} bytes) hflags({2:x}) flags({3:x})", Name, Size, Flags0, Flags);
        }
    }

}
