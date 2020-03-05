
using System;
using System.Collections.Generic;
using System.IO;
using TES3.Records;
using TES3.Records.SubRecords;
using TES3.Util;

namespace TES3.GameItem.Item
{

    public enum GameSettingType
    {
        String, Int, Float
    }

    [TargetRecord("GMST")]
    public class GameSetting : TES3GameItem
    {

        GameSettingType type;
        string stringValue;
        int intValue;
        float floatValue;

        public GameSetting(GameSettingKey key, GameSettingType type) : base(key)
        {
            Type = type;
        }

        public GameSetting(string name, GameSettingType type) : this(new GameSettingKey(name), type)
        {

        }

        public GameSetting(Record record) : base(record)
        {
            
        }

        public override string RecordName => "GMST";
        
        [IdField]
        public GameSettingKey Key
        {
            get => (GameSettingKey) Id;
            set => Id = value;
        }

        public string Name
        {
            get => Key.Name;
            set => Key = new GameSettingKey(value);
        }

        public GameSettingType Type
        {
            get => type;
            set
            {
                switch (value)
                {
                    case GameSettingType.Float:
                        stringValue = null;
                        intValue = default;
                        break;
                    case GameSettingType.Int:
                        stringValue = null;
                        floatValue = default;
                        break;
                    case GameSettingType.String:
                        intValue = default;
                        floatValue = default;
                        break;
                    default:
                        throw new ArgumentException($"Unrecognized game setting type: {value}", "value");
                }
                type = value;
            }
        }

        public string GetString()
        {
            if (Type != GameSettingType.String)
            {
                throw new InvalidOperationException($"Unable to obtain string value from {Type} setting.");
            }
            return stringValue;
        }

        public void SetString(string value)
        {
            if (Type != GameSettingType.String)
            {
                throw new InvalidOperationException($"Unable to set string value on {Type} setting.");
            }
            stringValue = value;
        }

        public int GetInt()
        {
            if (Type != GameSettingType.Int)
            {
                throw new InvalidOperationException($"Unable to obtain int value from {Type} setting.");
            }
            return intValue;
        }

        public void SetInt(int value)
        {
            if (Type != GameSettingType.Int)
            {
                throw new InvalidOperationException($"Unable to set int value on {Type} setting.");
            }
            intValue = value;
        }

        public float GetFloat()
        {
            if (Type != GameSettingType.Float)
            {
                throw new InvalidOperationException($"Unable to obtain float value from {Type} setting.");
            }
            return floatValue;
        }

        public void SetFloat(float value)
        {
            if (Type != GameSettingType.Float)
            {
                throw new InvalidOperationException($"Unable to set float value on {Type} setting.");
            }
            floatValue = value;
        }

        protected override void OnCreate(IList<SubRecord> subRecords)
        {
            subRecords.Add(new StringSubRecord("NAME", Name));

            switch (Type)
            {
                case GameSettingType.Float:
                    subRecords.Add(new FloatSubRecord("FLTV", GetFloat()));
                    break;
                case GameSettingType.Int:
                    subRecords.Add(new IntSubRecord("INTV", GetInt()));
                    break;
                case GameSettingType.String:
                    subRecords.Add(new StringSubRecord("STRV", GetString()));
                    break;
            }

        }

        protected override void UpdateRequired(Record record)
        {
            record.GetSubRecord<StringSubRecord>("NAME").Data = Name;

            record.RemoveAllSubRecords("INTV", "FLTV", "STRV");
            switch (Type)
            {
                case GameSettingType.Float:
                    record.InsertSubRecordAt(record.GetAddIndex("FLTV"), new FloatSubRecord("FLTV", GetFloat()));
                    break;
                case GameSettingType.Int:
                    record.InsertSubRecordAt(record.GetAddIndex("INTV"), new IntSubRecord("INTV", GetInt()));
                    break;
                case GameSettingType.String:
                    if (stringValue != null)
                    {
                        record.InsertSubRecordAt(record.GetAddIndex("STRV"), new StringSubRecord("STRV", GetString()));
                    }
                    break;
            }
        }

        protected override void UpdateOptional(Record record)
        {
            
        }

        protected override void DoSyncWithRecord(Record record)
        {
            // Required
            Id = record.GetSubRecord<StringSubRecord>("NAME").Data;


            // Complex
            // Type
            if (record.ContainsSubRecord("STRV"))
            {
                Type = GameSettingType.String;
            }
            else if (record.ContainsSubRecord("INTV"))
            {
                Type = GameSettingType.Int;
            }
            else if (record.ContainsSubRecord("FLTV"))
            {
                Type = GameSettingType.Float;
            }
            else
            {
                Type = GameSettingType.String;
                return;
            }

            // Value
            switch (Type)
            {
                case GameSettingType.Float:
                    SetFloat(record.GetSubRecord<FloatSubRecord>("FLTV").Data);
                    break;
                case GameSettingType.Int:
                    SetInt(record.GetSubRecord<IntSubRecord>("INTV").Data);
                    break;
                case GameSettingType.String:
                    SetString(record.GetSubRecord<StringSubRecord>("STRV").Data);
                    break;
            }


        }

        
        protected override void DoValidateRecord(Record record, Validator validator)
        {
            validator.CheckRequired(record, "NAME");
        }

        public override TES3GameItem Clone()
        {
            var result = new GameSetting(Key, Type);
            switch (Type)
            {
                case GameSettingType.Float:
                    result.SetFloat(floatValue);
                    break;
                case GameSettingType.Int:
                    result.SetInt(intValue);
                    break;
                case GameSettingType.String:
                    result.SetString(stringValue);
                    break;
            }
            return result;
        }

        public override void StreamDebug(TextWriter target)
        {
            var writer = new IndentWriter(target);

            writer.WriteLine(ToString());

            writer.IncIndent();
            var type = Type;
            writer.WriteLine($"Type: {type}");
            switch (type)
            {
                case GameSettingType.String:
                    writer.WriteLine($"Value: {GetString()}");
                    break;
                case GameSettingType.Int:
                    writer.WriteLine($"Value: {GetInt()}");
                    break;
                case GameSettingType.Float:
                    writer.WriteLine($"Value {GetFloat()}");
                    break;
            }
            writer.DecIndent();
        }

        public override string ToString()
        {
            return $"Game Setting ({Name})";
        }
    }

    public class GameSettingKey
    {
        public GameSettingKey(string name)
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

            var other = (GameSettingKey) obj;
            return Name.Equals(other.Name);
        }

        public static bool operator ==(GameSettingKey a, GameSettingKey b) => OperatorUtils.Equals(a, b);

        public static bool operator !=(GameSettingKey a, GameSettingKey b) => OperatorUtils.NotEquals(a, b);
    }



}