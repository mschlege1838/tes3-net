
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

    [TargetRecord("SPEL")]
    public class Spell : TES3GameItem
    {
        const int AUTO_CALC_FLAG = 0x0001;
        const int PC_START_FLAG = 0x0002;
        const int ALWAYS_SUCCEEDS_FLAG = 0x0004;

        const int MAX_SPELL_EFFECTS = 8;


        public Spell(SpellType type, string name) : base(name)
        {
            Type = type;
        }

        public Spell(Record record) : base(record)
        {

        }

        public override string RecordName => "SPEL";

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

        public SpellType Type
        {
            get;
            set;
        }

        public int Cost
        {
            get;
            set;
        }

        public bool AutoCalc
        {
            get;
            set;
        }

        public bool PCStartSpell
        {
            get;
            set;
        }

        public bool AlwaysSucceeds
        {
            get;
            set;
        }

        public bool Deleted
        {
            get;
            set;
        }

        public IList<SpellEffect> EffectList
        {
            get;
        } = new MaxCapacityList<SpellEffect>(MAX_SPELL_EFFECTS, "Effects", true);

        protected override void OnCreate(IList<SubRecord> subRecords)
        {
            subRecords.Add(new StringSubRecord("NAME", Name));

            var flags = 0;
            if (AutoCalc)
            {
                flags |= AUTO_CALC_FLAG;
            }
            if (PCStartSpell)
            {
                flags |= PC_START_FLAG;
            }
            if (AlwaysSucceeds)
            {
                flags |= ALWAYS_SUCCEEDS_FLAG;
            }
            subRecords.Add(new SpellData("SPDT", (int) Type, Cost, flags));
        }

        protected override void UpdateRequired(Record record)
        {
            record.GetSubRecord<StringSubRecord>("NAME").Data = Name;

            var spellData = record.GetSubRecord<SpellData>("SPDT");
            spellData.Type = (int) Type;
            spellData.SpellCost = Cost;
            FlagSet(spellData, AutoCalc, AUTO_CALC_FLAG, SpellFlagSet, SpellFlagClear);
            FlagSet(spellData, PCStartSpell, PC_START_FLAG, SpellFlagSet, SpellFlagClear);
            FlagSet(spellData, AlwaysSucceeds, ALWAYS_SUCCEEDS_FLAG, SpellFlagSet, SpellFlagClear);
        }

        protected override void UpdateOptional(Record record)
        {
            ProcessOptional(record, "FNAM", DisplayName != null, () => new StringSubRecord("FNAM", DisplayName), (sr) => sr.Data = DisplayName);
            ProcessOptional(record, "DELE", Deleted, () => new IntSubRecord("DELE", 0), (sr) => sr.Data = 0);

            UpdateCollection(record, EffectList, "ENAM",
                delegate (ref int index, SpellEffect item)
                {
                    record.InsertSubRecordAt(index++, item.ToSubRecord());
                }
            );
        }

        protected override void DoSyncWithRecord(Record record)
        {
            //Required
            Id = record.GetSubRecord<StringSubRecord>("NAME").Data;

            var spellData = record.GetSubRecord<SpellData>("SPDT");
            Type = (SpellType) spellData.Type;
            Cost = spellData.SpellCost;
            AutoCalc = HasFlagSet(spellData.Flags, AUTO_CALC_FLAG);
            PCStartSpell = HasFlagSet(spellData.Flags, PC_START_FLAG);
            AlwaysSucceeds = HasFlagSet(spellData.Flags, ALWAYS_SUCCEEDS_FLAG);

            // Optional
            DisplayName = record.TryGetSubRecord<StringSubRecord>("FNAM")?.Data;
            Deleted = record.ContainsSubRecord("DELE");

            // Collection
            EffectList.Clear();
            foreach (SpellItemData effect in record.GetEnumerableFor("ENAM"))
            {
                EffectList.Add(new SpellEffect(effect));
            }
        }


        protected override void DoValidateRecord(Record record, Validator validator)
        {
            validator.CheckRequired(record, "NAME");
            validator.CheckRequired(record, "SPDT");
            validator.CheckCount(record, "ENAM", MAX_SPELL_EFFECTS);
        }

        public override TES3GameItem Clone()
        {
            var result = new Spell(Type, Name)
            {
                DisplayName = DisplayName,
                Cost = Cost,
                AutoCalc = AutoCalc,
                PCStartSpell = PCStartSpell,
                AlwaysSucceeds = AlwaysSucceeds,
                Deleted = Deleted
            };

            CollectionUtils.Copy(EffectList, result.EffectList);
            return result;
        }

        public override void StreamDebug(TextWriter target)
        {
            var writer = new IndentWriter(target);

            writer.WriteLine(ToString());

            writer.IncIndent();
            writer.WriteLine($"Name: {DisplayName}");
            writer.WriteLine($"Type: {Type}");
            writer.WriteLine($"Auto Calc: {AutoCalc}");
            writer.WriteLine($"Cost: {Cost}");
            writer.WriteLine($"PC Start: {PCStartSpell}");
            writer.WriteLine($"Always Succeeds: {AlwaysSucceeds}");

            writer.WriteLine("Effects");
            writer.IncIndent();
            {
                var table = new Table("Type", "AffectedAttribute", "AffectedSkill", "Area", "Range", "Duration", "Min", "Max");
                foreach (var effect in EffectList)
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
            return $"Spell ({Name})";
        }

        private static void SpellFlagSet(SpellData subRecord, int flag)
        {
            subRecord.Flags |= flag;
        }

        private static void SpellFlagClear(SpellData subRecord, int flag)
        {
            subRecord.Flags &= ~flag;
        }
    }
}
