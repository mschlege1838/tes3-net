

using System.Collections.Generic;
using System.IO;

using TES3.Records;
using TES3.GameItem.TypeConstant;
using TES3.Records.SubRecords;
using TES3.Util;

namespace TES3.GameItem.Item
{

    [TargetRecord("SNDG")]
    public class SoundGenerator : TES3GameItem
    {

        string name;
        string soundName;

        public SoundGenerator(SoundGeneratorKey key) : base(key)
        {
            Name = Key.Name;
        }

        public SoundGenerator(Record record) : base(record)
        {
            
        }

        public override string RecordName => "SNDG";

        [IdField]
        public SoundGeneratorKey Key
        {
            get => (SoundGeneratorKey) Id;
            set => Id = value;
        }

        public string Name
        {
            get => name;
            set => name = Validation.NotNull(value, "value", "Name");
        }

        public SoundGeneratorType Type
        {
            get => Key.Type;
            set => Key = new SoundGeneratorKey(Key.CreatureName, value);
        }

        public string CreatureName
        {
            get => Key.CreatureName;
            set => Key = new SoundGeneratorKey(value, Key.Type);
        }

        public string SoundName
        {
            get => soundName;
            set => soundName = Validation.NotNull(value, "value", "Sound Name");
        }

        public bool Deleted
        {
            get;
            set;
        }

        protected override void OnCreate(IList<SubRecord> subRecords)
        {
            subRecords.Add(new StringSubRecord("NAME", Name));
            subRecords.Add(new IntSubRecord("DATA", (int) Type));
            subRecords.Add(new StringSubRecord("SNAM", SoundName));
        }

        protected override void UpdateRequired(Record record)
        {
            record.GetSubRecord<StringSubRecord>("NAME").Data = Name;
            record.GetSubRecord<IntSubRecord>("DATA").Data = (int) Type;
            record.GetSubRecord<StringSubRecord>("SNAM").Data = SoundName;
        }

        protected override void UpdateOptional(Record record)
        {
            ProcessOptional(record, "CNAM", CreatureName != null, () => new StringSubRecord("CNAM", CreatureName), (sr) => sr.Data = CreatureName);
            ProcessOptional(record, "DELE", Deleted, () => new IntSubRecord("DELE", 0), (sr) => sr.Data = 0);
        }

        protected override void DoSyncWithRecord(Record record)
        {
            Id = new SoundGeneratorKey(record.TryGetSubRecord<StringSubRecord>("CNAM")?.Data, (SoundGeneratorType) record.GetSubRecord<IntSubRecord>("DATA").Data);

            // Required
            Name = record.GetSubRecord<StringSubRecord>("NAME").Data;
            SoundName = record.GetSubRecord<StringSubRecord>("SNAM").Data;

            // Optional
            Deleted = record.ContainsSubRecord("DELE");
        }

        protected override void DoValidateRecord(Record record, Validator validator)
        {
            validator.CheckRequired(record, "NAME");
            validator.CheckRequired(record, "DATA");
            validator.CheckRequired(record, "SNAM");
        }

        public override TES3GameItem Copy()
        {
            return new SoundGenerator(Key)
            {
                Name = Name,
                SoundName = SoundName,
                Deleted = Deleted
            };
        }

        public override void StreamDebug(TextWriter target)
        {
            var writer = new IndentWriter(target);

            writer.WriteLine(ToString());

            writer.IncIndent();
            writer.WriteLine($"Type: {Type}");
            writer.WriteLine($"Sound Name: {SoundName}");
            if (CreatureName != null)
            {
                writer.WriteLine($"Creature Name: {CreatureName}");
            }
            writer.DecIndent();
        }

        public override string ToString()
        {
            return $"Sound Generator ({Name})";
        }
    }

    public class SoundGeneratorKey
    {

        public SoundGeneratorKey(string creatureName, SoundGeneratorType type)
        {
            CreatureName = creatureName;
            Type = type;
        }

        public string CreatureName
        {
            get;
        }

        public SoundGeneratorType Type
        {
            get;
        }

        public string Name
        {
            get => $"{CreatureName}{Type}";
        }

        public override int GetHashCode()
        {
            var result = 1;
            result = result * 31 + (CreatureName is null ? 0 : CreatureName.GetHashCode());
            result = result * 31 + Type.GetHashCode();
            return result;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (SoundGeneratorKey) obj;
            return Type == other.Type && OperatorUtils.Equals(CreatureName, other.CreatureName);
        }

        public static bool operator ==(SoundGeneratorKey a, SoundGeneratorKey b) => OperatorUtils.Equals(a, b);

        public static bool operator !=(SoundGeneratorKey a, SoundGeneratorKey b) => OperatorUtils.NotEquals(a, b);
    }
}
