
using System.Collections.Generic;
using System.IO;
using TES3.Records;
using TES3.Records.SubRecords;
using TES3.Util;

namespace TES3.GameItem.Item
{

    [TargetRecord("SSCR")]
    public class StartScript : TES3GameItem
    {

        string identifier;

        public StartScript(StartScriptKey key, string identifier) : base(key)
        {
            Identifier = identifier;
        }

        public StartScript(string name, string identifier) : this(new StartScriptKey(name), identifier)
        {

        }

        public StartScript(Record record) : base(record)
        {

        }

        public override string RecordName => "SSCR";

        [IdField]
        public StartScriptKey Key
        {
            get => (StartScriptKey) Id;
            set => Id = value;
        }

        public string Name
        {
            get => Key.Name;
            set => Key = new StartScriptKey(value);
        }

        public string Identifier
        {
            get => identifier;
            set => identifier = Validation.NotNull(value, "value", "Identifier");
        }

        public bool Deleted
        {
            get;
            set;
        }

        protected override void OnCreate(IList<SubRecord> subRecords)
        {
            subRecords.Add(new StringSubRecord("DATA", Identifier));
            subRecords.Add(new StringSubRecord("NAME", Name));
        }

        protected override void UpdateRequired(Record record)
        {
            record.GetSubRecord<StringSubRecord>("NAME").Data = Name;
            record.GetSubRecord<StringSubRecord>("DATA").Data = Identifier;
        }

        protected override void UpdateOptional(Record record)
        {
            ProcessOptional(record, "DELE", Deleted, () => new IntSubRecord("DELE", 0), (sr) => sr.Data = 0);
        }

        protected override void DoSyncWithRecord(Record record)
        {
            // Required
            Id = new StartScriptKey(record.GetSubRecord<StringSubRecord>("NAME").Data);
            Identifier = record.GetSubRecord<StringSubRecord>("DATA").Data;

            // Optional
            Deleted = record.ContainsSubRecord("DELE");
        }

        protected override void DoValidateRecord(Record record, Validator validator)
        {
            validator.CheckRequired(record, "NAME");
            validator.CheckRequired(record, "DATA");
        }

        public override TES3GameItem Clone()
        {
            return new StartScript(Key, Identifier)
            {
                Deleted = Deleted
            };
        }

        public override void StreamDebug(TextWriter target)
        {
            var writer = new IndentWriter(target);

            writer.WriteLine(ToString());

            writer.IncIndent();
            writer.WriteLine($"Identifier: {Identifier}");
            writer.DecIndent();
        }

        public override string ToString()
        {
            return $"Start Script ({Name})";
        }


    }

    public class StartScriptKey
    {
        public StartScriptKey(string name)
        {
            Name = Validation.NotNull(name, "name", "Name");
        }

        public string Name
        {
            get;
        }


        public override int GetHashCode()
        {
            var result = 1;
            result = result * 31 + Name.GetHashCode();
            return result;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (StartScriptKey) obj;
            return Name == other.Name;
        }

        public static bool operator ==(StartScriptKey a, StartScriptKey b) => OperatorUtils.Equals(a, b);

        public static bool operator !=(StartScriptKey a, StartScriptKey b) => OperatorUtils.NotEquals(a, b);
    }


}
