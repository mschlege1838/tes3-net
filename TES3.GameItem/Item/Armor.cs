
using System.IO;
using TES3.GameItem.TypeConstant;
using TES3.Records;
using TES3.Records.SubRecords;
using TES3.Util;

namespace TES3.GameItem.Item
{

    public enum ArmorClassificationType
    {
        Light, Medium, Heavy
    }


    [TargetRecord("ARMO")]
    public class Armor : Apparel
    {

        public Armor(string name) : base(name)
        {
			
        }

        public Armor(Record record) : base(record)
        {
            
        }

        public override string RecordName => "ARMO";
        protected override string DataSubRecordName => "AODT";

        public ArmorType Type
        {
            get;
            set;
        }

        public int Value
        {
            get;
            set;
        }

        public int Health
        {
            get;
            set;
        }

        public int EnchantCapacity
        {
            get;
            set;
        }

        public int Rating
        {
            get;
            set;
        }

        public ArmorClassificationType Classification
        {
            get
            {
                float mediumCutoff = Type.GetMediumCutoff();
                if (Weight > mediumCutoff)
                {
                    return ArmorClassificationType.Heavy;
                }

                float lightCutoff = Type.GetLightCutoff();
                if (Weight <= lightCutoff)
                {
                    return ArmorClassificationType.Light;
                }

                return ArmorClassificationType.Medium;
            }
        }

        protected override SubRecord CreateDataSubRecord()
        {
            return new ArmorData("AODT", (int) Type, Weight, Value, Health, EnchantCapacity, Rating);
        }

        protected override void UpdateRequired(Record record)
        {
            base.UpdateRequired(record);

            var armorData = record.GetSubRecord<ArmorData>("AODT");
            armorData.Type = (int) Type;
            armorData.Weight = Weight;
            armorData.Value = Value;
            armorData.Health = Health;
            armorData.EnchantPoints = EnchantCapacity;
            armorData.Armor = Rating;

        }

        protected override void DoSyncWithRecord(Record record)
        {
            base.DoSyncWithRecord(record);

            var armorData = record.GetSubRecord<ArmorData>("AODT");
            Type = (ArmorType) armorData.Type;
            Weight = armorData.Weight;
            Value = armorData.Value;
            Health = armorData.Health;
            EnchantCapacity = armorData.EnchantPoints;
            Rating = armorData.Armor;
        }

        protected override void DoValidateRecord(Record record, Validator validator)
        {
            base.DoValidateRecord(record, validator);

            validator.CheckRequired(record, "FNAM");
        }

        public override TES3GameItem Clone()
        {
            var result = new Armor(Name)
            {
                Type = Type,
                Value = Value,
                Health = Health,
                EnchantCapacity = EnchantCapacity,
                Rating = Rating
            };
            CopyClone(result);
            return result;
        }

        public override void StreamDebug(TextWriter target)
        {
            var writer = new IndentWriter(target);

            writer.WriteLine(ToString());

            writer.IncIndent();
            writer.WriteLine($"Name: {DisplayName}");
            writer.WriteLine($"Type: {Type}");
            writer.WriteLine($"Classification: {Classification}");
            writer.WriteLine($"Weight: {Weight}");
            writer.WriteLine($"Value: {Value}");
            writer.WriteLine($"Health: {Health}");
            writer.WriteLine($"Rating: {Rating}");
            writer.WriteLine($"Enchant Capacity: {EnchantCapacity}");
            writer.WriteLine($"Model: {Model}");
            writer.WriteLine($"Icon: {Icon}");
            writer.WriteLine($"Script: {Script}");
            writer.WriteLine($"Enchantment: {Enchantment}");

            if (BipedParts.Count > 0)
            {
                writer.WriteLine("Biped Data");
                writer.IncIndent();

                var table = new Table("Type", "Male", "Female");
                foreach (var part in BipedParts)
                {
                    table.AddRow(part.Type, part.MaleBodyPart, part.FemaleBodyPart);
                }
                table.Print(writer);

                writer.DecIndent();
            }
            writer.DecIndent();
        }

        public override string ToString()
        {
            return $"Armor ({Name})";
        }
    }


}
