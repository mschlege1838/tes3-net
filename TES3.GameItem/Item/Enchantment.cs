
using System.Collections.Generic;
using System.IO;
using TES3.Core;
using TES3.GameItem.Part;
using TES3.GameItem.TypeConstant;
using TES3.Records;
using TES3.Records.SubRecords;
using TES3.Util;

namespace TES3.GameItem.Item
{

    [TargetRecord("ENCH")]
    public class Enchantment : TES3GameItem
    {

        const int MAX_ENCHANTMENT_EFFECTS = 8;

        public Enchantment(string name) : base(name)
        {
            
        }

        public Enchantment(Record record) : base(record)
        {
            
        }

        public override string RecordName => "ENCH";

        [IdField]
        public string Name
        {
            get => (string) Id;
            set => Id = value;
        }

        public EnchantmentType Type
        {
            get;
            set;
        }

        public int Cost
        {
            get;
            set;
        }

        public int Charge
        {
            get;
            set;
        }

        public bool AutoCalc
        {
            get;
            set;
        }

        public bool Deleted
        {
            get;
            set;
        }

        public IList<SpellEffect> Effects
        {
            get;
        } = new MaxCapacityList<SpellEffect>(MAX_ENCHANTMENT_EFFECTS, "Effects");

        protected override void OnCreate(IList<SubRecord> subRecords)
        {
            subRecords.Add(new StringSubRecord("NAME", Name));
            subRecords.Add(new EnchantData("ENDT", (int) Type, Cost, Charge, CalculateAutoCalc(0)));
        }

        protected override void UpdateRequired(Record record)
        {
            record.GetSubRecord<StringSubRecord>("NAME").Data = Name;

            var enchantData = record.GetSubRecord<EnchantData>("ENDT");
            enchantData.Type = (int) Type;
            enchantData.Cost = Cost;
            enchantData.Charge = Charge;
            enchantData.AutoCalc = CalculateAutoCalc(enchantData.AutoCalc);
        }

        protected override void UpdateOptional(Record record)
        {
            ProcessOptional(record, "DELE", Deleted, () => new IntSubRecord("DELE", 0), (sr) => sr.Data = 0);
            
            UpdateCollection(record, Effects, "ENAM",
                delegate (ref int index, SpellEffect item)
                {
                    record.InsertSubRecordAt(index++, item.ToSubRecord());
                }
            );

        }

        protected override void DoSyncWithRecord(Record record)
        {
            // Required
            Id = record.GetSubRecord<StringSubRecord>("NAME").Data;

            var enchantData = record.GetSubRecord<EnchantData>("ENDT");
            Type = (EnchantmentType) enchantData.Type;
            Cost = enchantData.Cost;
            Charge = enchantData.Charge;
            AutoCalc = IsAutoCalc(enchantData.AutoCalc);

            // Optional
            Deleted = record.ContainsSubRecord("DELE");

            // Collection
            Effects.Clear();
            foreach (SpellItemData subRecord in record.GetEnumerableFor("ENAM"))
            {
                Effects.Add(new SpellEffect(subRecord));
            }
        }

        protected override void DoValidateRecord(Record record, Validator validator)
        {
            validator.CheckRequired(record, "NAME");
            validator.CheckRequired(record, "ENDT");
            validator.CheckCount(record, "ENAM", MAX_ENCHANTMENT_EFFECTS);
        }

        public override TES3GameItem Copy()
        {
            var result = new Enchantment(Name)
            {
                Type = Type,
                Cost = Cost,
                Charge = Charge,
                AutoCalc = AutoCalc,
                Deleted = Deleted
            };

            CollectionUtils.Copy(Effects, result.Effects);
            return result;
        }

        public override void StreamDebug(TextWriter target)
        {
            var writer = new IndentWriter(target);

            writer.WriteLine(ToString());

            writer.IncIndent();
            writer.WriteLine($"Type: {Type}");
            writer.WriteLine($"Charge: {Charge}");
            writer.WriteLine($"Cost: {Cost}");
            writer.WriteLine($"Auto Calc: {AutoCalc}");

            writer.WriteLine("Effects");
            writer.IncIndent();
            {
                var table = new Table("Type", "AffectedAttribute", "AffectedSkill", "Area", "Range", "Duration", "Min", "Max");
                foreach (var effect in Effects)
                {
                    table.AddRow(effect.Effect, effect.AffectedAttribute, effect.AffectedSkill, effect.Area, effect.Range, effect.Duration, effect.MinMagnitude, effect.MaxMagnitude);
                }
                table.Print(writer);
            }
            writer.DecIndent();
            writer.DecIndent();
        }

        public override string ToString()
        {
            return $"Enchantment ({Name})";
        }


        // Apparent bug for whom data was not fixed; the AutoCalc is considered to be in
        // effect if the raw value is either 1 or 65535. I suspect this was introduced (and later
        // corrected) with the introduction of the right click -> Toggle Auto Calc feature of the CS.
        //
        // At the time the feature was implemented, AutoCalc was evidently a signed 16 bit integer,
        // and the toggle functionality somehow subtracted 1 while the AutoCalc value for the record
        // was 0, likely neglecting to consider the flag's current state, and assuming all AutoCalc flags
        // to be initially set. (Rather than explicit assignment, it would appear the toggle functionality
        // used addition to transition from "no" to "yes", and subtraction from "yes" to "no")
        //
        // Thus if a record had the flag set to "no", but the programming logic of the CS incorrectly
        // had it's state sored in memory as "yes", if its state was later flipped to "yes" using the
        // Toggle Auto Calc function, under the scenes, 1 would be subtracted from the current raw value
        // (0), resulting in integer overflow.
        //
        // That is, the logical value would be flipped to "yes", but the underlying value
        // set to -32768 (32 bit 65535). If the flag was subsequently flipped to "no", 1 was added to to
        // -32768 for -32767 (32 bit 65534).
        //
        // There are only 85 of these odd records (5 "yes", 80 "no"), and all are in Morrowind.esm. Only
        // 1 (for "yes"), and 0 (for "no") have been observed in the other official master/plugin files.
        static bool IsAutoCalc(int rawAutoCalc)
        {
            return rawAutoCalc == 1 || rawAutoCalc == 65535;
        }

        int CalculateAutoCalc(int autoCalc)
        {
            var isAutoCalc = IsAutoCalc(autoCalc);
            if (AutoCalc && !isAutoCalc)
            {
                autoCalc += 1;
            }
            else if (!AutoCalc && isAutoCalc)
            {
                autoCalc -= 1;
            }
            return autoCalc;
        }

    }
}
