
using System.Collections.Generic;
using System.IO;
using TES3.GameItem.TypeConstant;
using TES3.Records;
using TES3.Records.SubRecords;
using TES3.Util;

namespace TES3.GameItem.Item
{

    [TargetRecord("CLOT")]
    public class Clothing : Apparel
    {

        public Clothing(string name) : base(name)
        {
            
        }

        public Clothing(Record record) : base(record)
        {
            
        }

        public override string RecordName => "CLOT";
        protected override string DataSubRecordName => "CTDT";

        public ClothingType Type
        {
            get;
            set;
        }

        public short Value
        {
            get;
            set;
        }

        public short EnchantCapacity
        {
            get;
            set;
        }

        protected override SubRecord CreateDataSubRecord()
        {
            return new ClothingData("CTDT", (int) Type, Weight, Value, EnchantCapacity);
        }

        protected override void UpdateRequired(Record record)
        {
            base.UpdateRequired(record);

            var clothingData = record.GetSubRecord<ClothingData>("CTDT");
            clothingData.Type = (int) Type;
            clothingData.Weight = Weight;
            clothingData.Value = Value;
            clothingData.EnchantPoints = EnchantCapacity;
        }

        protected override void DoSyncWithRecord(Record record)
        {
            base.DoSyncWithRecord(record);

            var clothingData = record.GetSubRecord<ClothingData>("CTDT");
            Type = (ClothingType) clothingData.Type;
            Weight = clothingData.Weight;
            Value = clothingData.Value;
            EnchantCapacity = clothingData.EnchantPoints;
        }

        public override TES3GameItem Clone()
        {
            var result = new Clothing(Name)
            {
                Type = Type,
                Value = Value,
                EnchantCapacity = EnchantCapacity
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
            writer.WriteLine($"Weight: {Weight}");
            writer.WriteLine($"Value: {Value}");
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
            return $"Clothing ({Name})";
        }
    }
}
