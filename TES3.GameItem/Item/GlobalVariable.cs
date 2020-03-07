
using System;
using System.Collections.Generic;
using System.IO;
using TES3.GameItem.TypeConstant;
using TES3.Records;
using TES3.Records.SubRecords;
using TES3.Util;

namespace TES3.GameItem.Item
{

    [TargetRecord("GLOB")]
    public class GlobalVariable : TES3GameItem
    {

        float value;

        public GlobalVariable(GlobalVariableKey key, GlobalVariableType type) : base(key)
        {
            Type = type;
        }

        public GlobalVariable(string name, GlobalVariableType type) : this(new GlobalVariableKey(name), type)
        {

        }

        public GlobalVariable(Record record) : base(record)
        {
            
        }

        public override string RecordName => "GLOB";
        
        [IdField]
        public GlobalVariableKey Key
        {
            get => (GlobalVariableKey) Id;
            set => Id = value;
        }

        public string Name
        {
            get => Key.Name;
            set => Key = new GlobalVariableKey(value);
        }

        public GlobalVariableType Type
        {
            get;
            set;
        }
        
        public bool Deleted
        {
            get;
            set;
        }

        public short GetShort()
        {
            if (Type != GlobalVariableType.Short)
            {
                throw new InvalidOperationException($"Unable to obtain short value from {Type} global.");
            }
            return (short) value;
        }

        public void SetShort(short value)
        {
            if (Type != GlobalVariableType.Short)
            {
                throw new InvalidOperationException($"Unable to set short value on {Type} global.");
            }
            this.value = value;
        }

        public int GetLong()
        {
            if (Type != GlobalVariableType.Long)
            {
                throw new InvalidOperationException($"Unable to obtain long value from {Type} global.");
            }
            return (int) value;
        }

        public void SetLong(int value)
        {
            if (Type != GlobalVariableType.Long)
            {
                throw new InvalidOperationException($"Unable to set long value on {Type} global.");
            }
            this.value = value;
        }

        public float GetFloat()
        {
            if (Type != GlobalVariableType.Float)
            {
                throw new InvalidOperationException($"Unable to obtain float value from {Type} global.");
            }
            return value;
        }

        public void SetFloat(float value)
        {
            if (Type != GlobalVariableType.Float)
            {
                throw new InvalidOperationException($"Unable to set float value on {Type} global.");
            }
            this.value = value;
        }

        protected override void OnCreate(IList<SubRecord> subRecords)
        {
            subRecords.Add(new StringSubRecord("NAME", Name));
            subRecords.Add(new StringSubRecord("FNAM", ((char) Type).ToString()));
            subRecords.Add(new FloatSubRecord("FLTV", value));
        }

        protected override void UpdateRequired(Record record)
        {
            record.GetSubRecord<StringSubRecord>("NAME").Data = Name;
            record.GetSubRecord<StringSubRecord>("FNAM").Data = ((char) Type).ToString();
            record.GetSubRecord<FloatSubRecord>("FLTV").Data = value;
        }

        protected override void UpdateOptional(Record record)
        {
            ProcessOptional(record, "DELE", Deleted, () => new IntSubRecord("DELE", 0), (sr) => sr.Data = 0);
        }

        protected override void DoSyncWithRecord(Record record)
        {
            // Required
            Id = new GlobalVariableKey(record.GetSubRecord<StringSubRecord>("NAME").Data);
            Type = (GlobalVariableType) record.GetSubRecord<StringSubRecord>("FNAM").Data[0];
            value = record.GetSubRecord<FloatSubRecord>("FLTV").Data;

            // Optional
            Deleted = record.ContainsSubRecord("DELE");
        }

        protected override void DoValidateRecord(Record record, Validator validator)
        {
            validator.CheckRequired(record, "NAME");
            validator.CheckRequired(record, "FNAM");
            validator.CheckRequired(record, "FLTV");
        }

        public override TES3GameItem Copy()
        {
            return new GlobalVariable(Key, Type)
            {
                value = value
            };
        }

        public override void StreamDebug(TextWriter target)
        {
            var writer = new IndentWriter(target);

            writer.WriteLine(ToString());

            writer.IncIndent();
            writer.WriteLine($"Type: {Type}");
            switch (Type)
            {
                case GlobalVariableType.Short:
                    writer.WriteLine($"Value: {GetShort()}");
                    break;
                case GlobalVariableType.Long:
                    writer.WriteLine($"Value: {GetLong()}");
                    break;
                case GlobalVariableType.Float:
                    writer.WriteLine($"Value: {GetFloat()}");
                    break;
            }
            writer.DecIndent();
        }

        public override string ToString()
        {
            return $"Global Variable ({Name})";
        }
    }


    public class GlobalVariableKey
    {
        public GlobalVariableKey(string name)
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

            var other = (GlobalVariableKey) obj;
            return Name == other.Name;
        }

        public static bool operator ==(GlobalVariableKey a, GlobalVariableKey b) => OperatorUtils.Equals(a, b);

        public static bool operator !=(GlobalVariableKey a, GlobalVariableKey b) => OperatorUtils.NotEquals(a, b);

    }



}