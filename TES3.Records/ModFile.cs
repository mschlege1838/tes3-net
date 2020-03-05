
using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using TES3.Util;
using TES3.Records.Processing;

namespace TES3.Records
{
    public class ModFile : IRecord<Record>
    {

        public const int READ_BUF_INITIAL_SIZE = 128;

        public static IList<ModFile> ReadFiles(IEnumerable<string> fileNames)
        {
            var result = new List<ModFile>();
            var buf = new byte[READ_BUF_INITIAL_SIZE];

            foreach (var name in fileNames)
            {
                foreach (var fname in FileUtils.ResolveWildcard(name))
                {
                    var file = new FileInfo(fname);
                    if (!file.Exists)
                    {
                        throw new ConsoleApplicationException($"{file} does not exist.");
                    }

                    var records = new List<Record>();
                    using (var stream = file.OpenRead())
                    {
                        RecordProcessing.ReadStream(stream, ref buf, records);
                    }

                    result.Add(new ModFile(file.Name, records));
                }
            }

            return result;
        }

        public static ModFile ReadFile(string fname)
        {
            var buf = new byte[READ_BUF_INITIAL_SIZE];
            var file = new FileInfo(fname);
            if (!file.Exists)
            {
                throw new ConsoleApplicationException($"{file} does not exist.");
            }

            var records = new List<Record>();
            using (var stream = file.OpenRead())
            {
                RecordProcessing.ReadStream(stream, ref buf, records);
            }

            return new ModFile(file.Name, records);
        }

        



        readonly IList<Record> records;
        readonly IDictionary<string, IList<Record>> recordIndex = new Dictionary<string, IList<Record>>();
        

        public ModFile(string name, IList<Record> records)
        {
            Name = name ?? throw new ArgumentNullException("name", "Name cannot be null.");
            this.records = records ?? throw new ArgumentNullException("records", "Mod Records cannot be null.");

            foreach (var record in records)
            {
                string recordName = record.Name;
                if (recordIndex.ContainsKey(recordName))
                {
                    recordIndex[recordName].Add(record);
                }
                else
                {
                    recordIndex.Add(recordName, new List<Record>() { record });
                }
            }
        }


        public event EventHandler<RecordEventArgs> RecordAdded;

        public event EventHandler<RecordEventArgs> RecordRemoved;


        public Record this[int index]
        {
            get => records[index];
            set
            {
                var currentRecord = records[index];
                var currentName = currentRecord.Name;
                var newName = value.Name;
                if (currentName == newName)
                {
                    var indexList = recordIndex[currentName];
                    indexList[indexList.IndexOf(currentRecord)] = value;
                }
                else
                {
                    recordIndex[currentName].Remove(currentRecord);

                    if (recordIndex.ContainsKey(newName))
                    {
                        recordIndex[newName].Add(value);
                    }
                    else
                    {
                        recordIndex.Add(newName, new List<Record>() { value });
                    }
                }

                records[index] = value;
            }
        }

        public IList<Record> this[string name]
        {
            get => new ReadOnlyList<Record>(recordIndex[name]);
        }

        public string Name
        {
            get;
        }

        public int Count
        {
            get => records.Count;
        }

        public long CurrentSize
        {
            get
            {
                var result = 0L;
                foreach (var record in this)
                {
                    result += record.CurrentSize;
                }
                return result;
            }
        }


        public int GetRecordIndex(Record record)
        {
            return records.IndexOf(record);
        }

        public int GetAddIndex(string name)
        {
            return (int) RawUtils.ProcessRecord(RecordOperationType.AddIndex, new RecordAddIndexContext(this, name));
        }

        public int GetDialogueAddIndex(Record record)
        {
            if (record.Name != "DIAL")
            {
                throw new ArgumentException();
            }

            var index = records.IndexOf(record);
            if (index == -1)
            {
                throw new ArgumentException();
            }

            while (records[++index].Name == "INFO") ;

            return index;
        }

        public void InsertRecordAt(int index, Record record)
        {
            if (record == null)
            {
                throw new ArgumentNullException("record", "Record cannot be null.");
            }
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index", index, $"Index cannot be negative: {index}");
            }
            if (index > records.Count)
            {
                throw new ArgumentOutOfRangeException("index", index, $"Attempt to access record {index} of {records.Count}.");
            }

            if (!recordIndex.ContainsKey(record.Name))
            {
                recordIndex.Add(record.Name, new List<Record>() { record });
            }
            else
            {
                recordIndex[record.Name].Add(record);
            }

            records.Insert(index, record);
            RecordAdded?.Invoke(this, new RecordEventArgs(record));
        }

        public bool RemoveRecord(Record record)
        {
            recordIndex[record.Name].Remove(record);
            
            var result = records.Remove(record);
            if (result)
            {
                RecordRemoved?.Invoke(this, new RecordEventArgs(record));
            }

            return result;
        }

        public bool ContainsRecord(string name)
        {
            return recordIndex.ContainsKey(name);
        }

        public bool ContainsAnyRecords(params string[] names)
        {
            foreach (var name in names)
            {
                if (!ContainsRecord(name))
                {
                    return false;
                }
            }
            return true;
        }

        public IList<Record> GetRecords(params string[] names)
        {
            var records = new List<Record>();
            foreach (var name in names)
            {
                foreach (var record in this[name])
                {
                    records.Add(record);
                }
            }
            return records;
        }

        public void WriteToStream(Stream stream)
        {
            foreach (var record in this)
            {
                RecordProcessing.WriteRecord(stream, record);
            }
        }

        public IEnumerator<Record> GetEnumerator()
        {
            return records.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public NameEnumerable<Record, ModFile> GetEnumerableFor(params string[] names)
        {
            return new NameEnumerable<Record, ModFile>(this, names);
        }
            

        public int GetLastIndex(string name)
        {
            for (var i = records.Count - 1; i >= 0; --i)
            {
                if (records[i].Name == name)
                {
                    return i;
                }
            }
            return -1;
        }
    }


    public class RecordEventArgs
    {

        public RecordEventArgs(Record record)
        {
            Record = record;
        }

        public Record Record { get; }

    }
	
}
