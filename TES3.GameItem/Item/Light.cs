
using System.Collections.Generic;
using System.IO;
using TES3.Core;
using TES3.Records;
using TES3.Records.SubRecords;
using TES3.Util;

namespace TES3.GameItem.Item
{

    
    public enum FlickerType
    {
        Flicker, FlickerSlow, Pulse, PulseSlow, None
    }

    [TargetRecord("LIGH")]
    public class Light : TES3GameItem
    {
        const int DYNAMIC_FLAG = 0x0001;
        const int CARRY_FLAG = 0x0002;
        const int NEGATIVE_FLAG = 0x0004;
        const int FLICKER_FLAG = 0x0008;
        const int FIRE_FLAG = 0x0010;
        const int OFF_DEFAULT_FLAG = 0x0020;
        const int FLICKER_SLOW_FLAG = 0x0040;
        const int PULSE_FLAG = 0x0080;
        const int PULSE_SLOW_FLAG = 0x0100;

        string model;
        ColorRef color;

        public Light(string name) : base(name)
        {
            
        }

        public Light(Record record) : base(record)
        {
            
        }

        public override string RecordName => "LIGH";

        [IdField]
        public string Name
        {
            get => (string) Id;
            set => Id = value;
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

        public int Value
        {
            get;
            set;
        }

        public int LifeSpan
        {
            get;
            set;
        }

        public int Radius
        {
            get;
            set;
        }

        public ColorRef Color
        {
            get => color;
            set => color = Validation.NotNull(value, "value", "Color");
        }

        public bool Dynamic
        {
            get;
            set;
        }

        public bool CanCarry
        {
            get;
            set;
        }

        public bool Negative
        {
            get;
            set;
        }

        public bool Fire
        {
            get;
            set;
        }

        public bool OffByDefault
        {
            get;
            set;
        }

        public FlickerType FlickerEffect
        {
            get;
            set;
        }

        public string Script
        {
            get;
            set;
        }

        public string IconTexture
        {
            get;
            set;
        }

        public string Model
        {
            get => model;
            set => model = Validation.NotNull(value, "value", "Model");
        }

        public string Sound
        {
            get;
            set;
        }

        public bool Deleted
        {
            get;
            set;
        }

        protected override void OnCreate(IList<SubRecord> subRecords)
        {
            subRecords.Add(new StringSubRecord("NAME", Name));
            subRecords.Add(new StringSubRecord("MODL", Model));

            var flags = 0;
            if (Dynamic)
            {
                flags |= DYNAMIC_FLAG;
            }
            if (CanCarry)
            {
                flags |= CARRY_FLAG;
            }
            if (Negative)
            {
                flags |= NEGATIVE_FLAG;
            }
            if (Fire)
            {
                flags |= FIRE_FLAG;
            }
            if (OffByDefault)
            {
                flags |= OFF_DEFAULT_FLAG;
            }
            switch (FlickerEffect)
            {
                case FlickerType.Flicker:
                    flags |= FLICKER_FLAG;
                    break;
                case FlickerType.FlickerSlow:
                    flags |= FLICKER_SLOW_FLAG;
                    break;
                case FlickerType.Pulse:
                    flags |= PULSE_FLAG;
                    break;
                case FlickerType.PulseSlow:
                    flags |= PULSE_SLOW_FLAG;
                    break;
            }
            subRecords.Add(new LightData("LHDT", Weight, Value, LifeSpan, Radius, Color.Copy(), flags));

        }

        protected override void UpdateRequired(Record record)
        {
            record.GetSubRecord<StringSubRecord>("NAME").Data = Name;
            record.GetSubRecord<StringSubRecord>("MODL").Data = Model;

            var lightData = record.GetSubRecord<LightData>("LHDT");
            lightData.Weight = Weight;
            lightData.Value = Value;
            lightData.Time = LifeSpan;
            lightData.Radius = Radius;
            lightData.Color = Color.Copy();
            FlagSet(lightData, Dynamic, DYNAMIC_FLAG, LightDataFlagSet, LightDataFlagClear);
            FlagSet(lightData, CanCarry, CARRY_FLAG, LightDataFlagSet, LightDataFlagClear);
            FlagSet(lightData, Negative, NEGATIVE_FLAG, LightDataFlagSet, LightDataFlagClear);
            FlagSet(lightData, Fire, FIRE_FLAG, LightDataFlagSet, LightDataFlagClear);
            FlagSet(lightData, OffByDefault, OFF_DEFAULT_FLAG, LightDataFlagSet, LightDataFlagClear);

            switch (FlickerEffect)
            {
                case FlickerType.Flicker:
                    lightData.Flags |= FLICKER_FLAG;
                    lightData.Flags &= 0xFFFF & ~FLICKER_SLOW_FLAG & ~PULSE_FLAG & ~PULSE_SLOW_FLAG;
                    break;
                case FlickerType.FlickerSlow:
                    lightData.Flags |= FLICKER_SLOW_FLAG;
                    lightData.Flags &= 0xFFFF & ~FLICKER_FLAG & ~PULSE_FLAG & ~PULSE_SLOW_FLAG;
                    break;
                case FlickerType.Pulse:
                    lightData.Flags |= PULSE_FLAG;
                    lightData.Flags &= 0xFFFF & ~FLICKER_SLOW_FLAG & ~FLICKER_FLAG & ~PULSE_SLOW_FLAG;
                    break;
                case FlickerType.PulseSlow:
                    lightData.Flags |= PULSE_SLOW_FLAG;
                    lightData.Flags &= 0xFFFF & ~FLICKER_SLOW_FLAG & ~PULSE_FLAG & ~FLICKER_FLAG;
                    break;
                case FlickerType.None:
                    lightData.Flags &= 0xFFFF & ~FLICKER_FLAG & ~FLICKER_SLOW_FLAG & ~PULSE_FLAG & ~PULSE_SLOW_FLAG;
                    break;
            }

        }

        protected override void UpdateOptional(Record record)
        {
            ProcessOptional(record, "FNAM", DisplayName != null, () => new StringSubRecord("FNAM", DisplayName), (sr) => sr.Data = DisplayName);
            ProcessOptional(record, "SCRI", Script != null, () => new StringSubRecord("SCRI", Script), (sr) => sr.Data = Script);
            ProcessOptional(record, "ITEX", IconTexture != null, () => new StringSubRecord("ITEX", IconTexture), (sr) => sr.Data = IconTexture);
            ProcessOptional(record, "SNAM", Sound != null, () => new StringSubRecord("SNAM", Sound), (sr) => sr.Data = Sound);
            ProcessOptional(record, "DELE", Deleted, () => new IntSubRecord("DELE", 0), (sr) => sr.Data = 0);
        }

        protected override void DoSyncWithRecord(Record record)
        {
            // Required
            Id = record.GetSubRecord<StringSubRecord>("NAME").Data;
            Model = record.GetSubRecord<StringSubRecord>("MODL").Data;

            var lightData = record.GetSubRecord<LightData>("LHDT");
            Weight = lightData.Weight;
            Value = lightData.Value;
            LifeSpan = lightData.Time;
            Radius = lightData.Radius;
            Color = lightData.Color.Copy();
            Dynamic = HasFlagSet(lightData.Flags, DYNAMIC_FLAG);
            CanCarry = HasFlagSet(lightData.Flags, CARRY_FLAG);
            Negative = HasFlagSet(lightData.Flags, NEGATIVE_FLAG);
            Fire = HasFlagSet(lightData.Flags, FIRE_FLAG);
            OffByDefault = HasFlagSet(lightData.Flags, OFF_DEFAULT_FLAG);

            if (HasFlagSet(lightData.Flags, FLICKER_FLAG))
            {
                FlickerEffect = FlickerType.Flicker;
            }
            else if (HasFlagSet(lightData.Flags, FLICKER_SLOW_FLAG))
            {
                FlickerEffect = FlickerType.FlickerSlow;
            }
            else if (HasFlagSet(lightData.Flags, PULSE_FLAG))
            {
                FlickerEffect = FlickerType.Pulse;
            }
            else if (HasFlagSet(lightData.Flags, PULSE_SLOW_FLAG))
            {
                FlickerEffect = FlickerType.PulseSlow;
            }
            else
            {
                FlickerEffect = FlickerType.None;
            }

            // Optional
            DisplayName = record.TryGetSubRecord<StringSubRecord>("FNAM")?.Data;
            Script = record.TryGetSubRecord<StringSubRecord>("SCRI")?.Data;
            IconTexture = record.TryGetSubRecord<StringSubRecord>("ITEX")?.Data;
            Sound = record.TryGetSubRecord<StringSubRecord>("SNAM")?.Data;
            Deleted = record.ContainsSubRecord("DELE");
        }

        protected override void DoValidateRecord(Record record, Validator validator)
        {
            validator.CheckRequired(record, "NAME");
            validator.CheckRequired(record, "MODL");
            validator.CheckRequired(record, "LHDT");
        }

        public override TES3GameItem Clone()
        {
            return new Light(Name)
            {
                DisplayName = DisplayName,
                Weight = Weight,
                Value = Value,
                LifeSpan = LifeSpan,
                Radius = Radius,
                Color = Color.Copy(),
                Dynamic = Dynamic,
                CanCarry = CanCarry,
                Negative = Negative,
                Fire = Fire,
                OffByDefault = OffByDefault,
                FlickerEffect = FlickerEffect,
                Script = Script,
                IconTexture = IconTexture,
                Model = Model,
                Sound = Sound,
                Deleted = Deleted
            };
        }

        public override void StreamDebug(TextWriter target)
        {
            var writer = new IndentWriter(target);

            writer.WriteLine(ToString());

            writer.IncIndent();
            writer.WriteLine($"Name: {DisplayName}");
            writer.WriteLine($"Model: {Model}");
            writer.WriteLine($"Color: {Color}");
            writer.WriteLine($"Radius: {Radius}");
            writer.WriteLine($"Flicker Effect: {FlickerEffect}");
            writer.WriteLine($"Fire: {Fire}");
            writer.WriteLine($"Negative: {Negative}");
            writer.WriteLine($"Dynamic: {Dynamic}");
            writer.WriteLine($"Sound: {Sound}");
            writer.WriteLine($"Script: {Script}");
            writer.WriteLine($"Can Carry: {CanCarry}");
            writer.WriteLine($"Off By Default: {OffByDefault}");
            writer.WriteLine($"Icon: {IconTexture}");
            writer.WriteLine($"Weight: {Weight}");
            writer.WriteLine($"Life Span: {LifeSpan}");
            writer.WriteLine($"Value: {Value}");
            writer.DecIndent();
        }

        public override string ToString()
        {
            return $"Light ({Name})";
        }

        static void LightDataFlagSet(LightData subRecord, int flag)
        {
            subRecord.Flags |= flag;
        }

        static void LightDataFlagClear(LightData subRecord, int flag)
        {
            subRecord.Flags &= ~flag;
        }
    }



}
