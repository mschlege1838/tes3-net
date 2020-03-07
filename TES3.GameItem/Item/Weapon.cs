
using System.Collections.Generic;
using System.IO;
using TES3.GameItem.Part;
using TES3.GameItem.TypeConstant;
using TES3.Records;
using TES3.Records.SubRecords;
using TES3.Util;

namespace TES3.GameItem.Item
{

    [TargetRecord("WEAP")]
    public class Weapon : TES3GameItem
    {
        const int IGNORE_NORMAL_WEAPON_RESISTANCE_FLAG = 0x01;
        const int SILVER_WEAPON_FLAG = 0x02;


        string model;

        public Weapon(string name, WeaponType type) : base(name)
        {
            Type = type;
        }

        public Weapon(Record record) : base(record)
        {

        }

        public override string RecordName => "WEAP";

        [IdField]
        public string Name
        {
            get => (string) Id;
            set => Id = value;
        }

        public string Model
        {
            get => model;
            set => model = Validation.NotNull(value, "value", "Model");
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

        public WeaponType Type
        {
            get;
            set;
        }

        public short Health
        {
            get;
            set;
        }

        public float Speed
        {
            get;
            set;
        }

        public float Reach
        {
            get;
            set;
        }

        public short EnchantCapacity
        {
            get;
            set;
        }

        public bool IgnoreNormalWeaponResistance
        {
            get;
            set;
        }

        public bool Silver
        {
            get;
            set;
        }

        public string Icon
        {
            get;
            set;
        }

        public string Enchantment
        {
            get;
            set;
        }

        public string Script
        {
            get;
            set;
        }

        public bool Deleted
        {
            get;
            set;
        }
        public WeaponDamageRange ChopDamage
        {
            get;
        } = new WeaponDamageRange(1, 5);

        public WeaponDamageRange SlashDamage
        {
            get;
        } = new WeaponDamageRange(1, 5);

        public WeaponDamageRange ThrustDamage
        {
            get;
        } = new WeaponDamageRange(1, 5);


        public bool IsSame(Weapon other)
        {
            return Name == other.Name
                && Model == other.Model
                && DisplayName == other.DisplayName
                && Weight == other.Weight
                && Value == other.Value
                && Type == other.Type
                && Health == other.Health
                && Speed == other.Speed
                && Reach == other.Reach
                && EnchantCapacity == other.EnchantCapacity
                && IgnoreNormalWeaponResistance == other.IgnoreNormalWeaponResistance
                && Silver == other.Silver
                && Icon == other.Icon
                && Enchantment == other.Enchantment
                && Script == other.Script
                && Deleted == other.Deleted
                && ChopDamage.Max == other.ChopDamage.Max && ChopDamage.Min == other.ChopDamage.Min
                && SlashDamage.Max == other.SlashDamage.Max && SlashDamage.Min == other.SlashDamage.Min
                && ThrustDamage.Max == other.ThrustDamage.Max && ThrustDamage.Min == other.ThrustDamage.Min
                ;
        }

        protected override void OnCreate(IList<SubRecord> subRecords)
        {
            subRecords.Add(new StringSubRecord("NAME", Name));
            subRecords.Add(new StringSubRecord("MODL", Model));

            var flags = 0;
            if (IgnoreNormalWeaponResistance)
            {
                flags |= IGNORE_NORMAL_WEAPON_RESISTANCE_FLAG;
            }
            if (Silver)
            {
                flags |= SILVER_WEAPON_FLAG;
            }
            subRecords.Add(new WeaponData("WPDT", Weight, Value, (short) Type, Health, Speed, Reach, EnchantCapacity, ChopDamage.Min, ChopDamage.Max, SlashDamage.Min, SlashDamage.Max, ThrustDamage.Min, ThrustDamage.Max, flags));
        }

        protected override void UpdateRequired(Record record)
        {
            record.GetSubRecord<StringSubRecord>("NAME").Data = Name;
            record.GetSubRecord<StringSubRecord>("MODL").Data = Model;

            var weaponData = record.GetSubRecord<WeaponData>("WPDT");
            weaponData.Weight = Weight;
            weaponData.Value = Value;
            weaponData.Type = (short) Type;
            weaponData.Health = Health;
            weaponData.Speed = Speed;
            weaponData.Reach = Reach;
            weaponData.EnchantPoints = EnchantCapacity;
            weaponData.ChopMin = ChopDamage.Min;
            weaponData.ChopMax = ChopDamage.Max;
            weaponData.SlashMin = SlashDamage.Min;
            weaponData.SlashMax = SlashDamage.Max;
            weaponData.ThrustMin = ThrustDamage.Min;
            weaponData.ThrustMax = ThrustDamage.Max;
            FlagSet(weaponData, IgnoreNormalWeaponResistance, IGNORE_NORMAL_WEAPON_RESISTANCE_FLAG, WeaponFlagSet, WeaponFlagClear);
            FlagSet(weaponData, Silver, SILVER_WEAPON_FLAG, WeaponFlagSet, WeaponFlagClear);
        }

        protected override void UpdateOptional(Record record)
        {
            ProcessOptional(record, "FNAM", DisplayName != null, () => new StringSubRecord("FNAM", DisplayName), (sr) => sr.Data = DisplayName);
            ProcessOptional(record, "ITEX", Icon != null, () => new StringSubRecord("ITEX", Icon), (sr) => sr.Data = Icon);
            ProcessOptional(record, "ENAM", Enchantment != null, () => new StringSubRecord("ENAM", Enchantment), (sr) => sr.Data = Enchantment);
            ProcessOptional(record, "SCRI", Script != null, () => new StringSubRecord("SCRI", Script), (sr) => sr.Data = Script);
            ProcessOptional(record, "DELE", Deleted, () => new IntSubRecord("DELE", 0), (sr) => sr.Data = 0);
        }

        protected override void DoSyncWithRecord(Record record)
        {
            // Required
            Id = record.GetSubRecord<StringSubRecord>("NAME").Data;
            Model = record.GetSubRecord<StringSubRecord>("MODL").Data;
            
            var weaponData = record.GetSubRecord<WeaponData>("WPDT");
            Weight = weaponData.Weight;
            Value = weaponData.Value;
            Type = (WeaponType) weaponData.Type;
            Health = weaponData.Health;
            Speed = weaponData.Speed;
            Reach = weaponData.Reach;
            EnchantCapacity = weaponData.EnchantPoints;
            ChopDamage.SetRange(weaponData.ChopMin, weaponData.ChopMax);
            SlashDamage.SetRange(weaponData.SlashMin, weaponData.SlashMax);
            ThrustDamage.SetRange(weaponData.ThrustMin, weaponData.ThrustMax);

            // Optional
            DisplayName = record.TryGetSubRecord<StringSubRecord>("FNAM")?.Data;
            IgnoreNormalWeaponResistance = HasFlagSet(weaponData.Flags, IGNORE_NORMAL_WEAPON_RESISTANCE_FLAG);
            Silver = HasFlagSet(weaponData.Flags, SILVER_WEAPON_FLAG);
            Icon = record.TryGetSubRecord<StringSubRecord>("ITEX")?.Data;
            Enchantment = record.TryGetSubRecord<StringSubRecord>("ENAM")?.Data;
            Script = record.TryGetSubRecord<StringSubRecord>("SCRI")?.Data;
            Deleted = record.ContainsSubRecord("DELE");
        }

        protected override void DoValidateRecord(Record record, Validator validator)
        {
            validator.CheckRequired(record, "NAME");
            validator.CheckRequired(record, "MODL");
            validator.CheckRequired(record, "WPDT");
        }

        public override TES3GameItem Copy()
        {
            var result = new Weapon(Name, Type)
            {
                Model = Model,
                DisplayName = DisplayName,
                Weight = Weight,
                Value = Value,
                Type = Type,
                Health = Health,
                Speed = Speed,
                Reach = Reach,
                EnchantCapacity = EnchantCapacity,
                IgnoreNormalWeaponResistance = IgnoreNormalWeaponResistance,
                Silver = Silver,
                Icon = Icon,
                Enchantment = Enchantment,
                Script = Script,
                Deleted = Deleted
            };

            result.ChopDamage.SetRange(ChopDamage);
            result.SlashDamage.SetRange(SlashDamage);
            result.ThrustDamage.SetRange(ThrustDamage);

            return result;
        }

        public override void StreamDebug(TextWriter target)
        {
            var writer = new IndentWriter(target);

            writer.WriteLine(ToString());

            writer.IncIndent();
            writer.WriteLine($"Name: {DisplayName}");
            writer.WriteLine($"Model: {Model}");
            writer.WriteLine($"Icon: {Icon}");
            writer.WriteLine($"Type: {Type}");
            writer.WriteLine($"Weight: {Weight}");
            writer.WriteLine($"Value: {Value}");
            writer.WriteLine($"Health: {Health}");
            writer.WriteLine($"Reach: {Reach}");
            writer.WriteLine($"Speed: {Speed}");
            writer.WriteLine($"Ignores Normal Weapon Resistance: {IgnoreNormalWeaponResistance}");
            writer.WriteLine($"Silver: {Silver}");
            writer.WriteLine($"Enchant Capacity: {EnchantCapacity}");
            writer.WriteLine($"Enchantment: {Enchantment}");
            writer.WriteLine($"Script: {Script}");

            writer.WriteLine("Damage Ranges");
            writer.IncIndent();
            {
                var table = new Table("Attack", "Min", "Max");
                table.AddRow("Chop", ChopDamage.Min, ChopDamage.Max);
                table.AddRow("Slash", SlashDamage.Min, SlashDamage.Max);
                table.AddRow("Thrust", ThrustDamage.Max, ThrustDamage.Max);
                table.Print(writer);
            }
            writer.DecIndent();
            writer.DecIndent();
        }

        public override string ToString()
        {
            return $"Weapon ({Name})";
        }

        static void WeaponFlagSet(WeaponData subRecord, int flag)
        {
            subRecord.Flags |= flag;
        }

        static void WeaponFlagClear(WeaponData subRecord, int flag)
        {
            subRecord.Flags &= ~flag;
        }
    }

}
