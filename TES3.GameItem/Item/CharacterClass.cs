using System;
using System.Collections.Generic;
using System.IO;
using TES3.GameItem.TypeConstant;
using TES3.Records;
using TES3.Records.SubRecords;
using TES3.Util;

namespace TES3.GameItem.Item
{

    [TargetRecord("CLAS")]
    public class CharacterClass : ServicesProvider
    {

        const int WEAPONS_FLAG = 0x00001;
        const int ARMOR_FLAG = 0x00002;
        const int CLOTHING_FLAG = 0x00004;
        const int BOOKS_FLAG = 0x00008;
        const int INGREDIENT_FLAG = 0x00010;
        const int PICKS_FLAG = 0x00020;
        const int PROBES_FLAG = 0x00040;
        const int LIGHTS_FLAG = 0x00080;
        const int APPARATUS_FLAG = 0x00100;
        const int REPAIR_FLAG = 0x00200;
        const int MISC_FLAG = 0x00400;
        const int SPELLS_FLAG = 0x00800;
        const int MAGIC_ITEMS_FLAG = 0x01000;
        const int POTIONS_FLAG = 0x02000;
        const int TRAINING_FLAG = 0x04000;
        const int SPELLMAKING_FLAG = 0x08000;
        const int ENCHANTING_FLAG = 0x10000;
        const int REPAIR_ITEM_FLAG = 0x20000;


        public AttributeType[] Attributes { get; } = new AttributeType[2];
        public SkillType[] MajorSkills { get; } = new SkillType[5];
        public SkillType[] MinorSkills { get; } = new SkillType[5];

        public CharacterClass(string name) : base(name)
        {
            
        }

        public CharacterClass(Record record) : base(record)
        {
            
        }

        public override string RecordName => "CLAS";

        public string DisplayName
        {
            get;
            set;
        }

        public SpecializationType Specialization
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public bool Deleted
        {
            get;
            set;
        }

        public bool Playable
        {
            get;
            set;
        }

        protected override void OnCreate(IList<SubRecord> subRecords)
        {
            subRecords.Add(new StringSubRecord("NAME", Name));
            subRecords.Add(new StringSubRecord("FNAM", DisplayName));

            var autoCalcFlags = 0;
            if (BuysWeapons)
            {
                autoCalcFlags |= WEAPONS_FLAG;
            }
            if (BuysArmor)
            {
                autoCalcFlags |= ARMOR_FLAG;
            }
            if (BuysClothing)
            {
                autoCalcFlags |= CLOTHING_FLAG;
            }
            if (BuysBooks)
            {
                autoCalcFlags |= BOOKS_FLAG;
            }
            if (BuysIngredients)
            {
                autoCalcFlags |= INGREDIENT_FLAG;
            }
            if (BuysPicks)
            {
                autoCalcFlags |= PICKS_FLAG;
            }
            if (BuysProbes)
            {
                autoCalcFlags |= PROBES_FLAG;
            }
            if (BuysLights)
            {
                autoCalcFlags |= LIGHTS_FLAG;
            }
            if (BuysApparatus)
            {
                autoCalcFlags |= APPARATUS_FLAG;
            }
            if (OffersRepair)
            {
                autoCalcFlags |= REPAIR_FLAG;
            }
            if (BuysMiscItems)
            {
                autoCalcFlags |= MISC_FLAG;
            }
            if (SellsSpells)
            {
                autoCalcFlags |= SPELLS_FLAG;
            }
            if (BuysMagicItems)
            {
                autoCalcFlags |= MAGIC_ITEMS_FLAG;
            }
            if (BuysPotions)
            {
                autoCalcFlags |= POTIONS_FLAG;
            }
            if (OffersTraining)
            {
                autoCalcFlags |= TRAINING_FLAG;
            }
            if (OffersSpellmaking)
            {
                autoCalcFlags |= SPELLMAKING_FLAG;
            }
            if (OffersEnchanting)
            {
                autoCalcFlags |= ENCHANTING_FLAG;
            }
            if (BuysRepairItems)
            {
                autoCalcFlags |= REPAIR_ITEM_FLAG;
            }

            subRecords.Add(new ClassData("CLDT", (int) Attributes[0], (int) Attributes[1], (int) Specialization,
                (int) MinorSkills[0], (int) MajorSkills[0],
                (int) MinorSkills[1], (int) MajorSkills[1],
                (int) MinorSkills[2], (int) MajorSkills[2],
                (int) MinorSkills[3], (int) MajorSkills[3],
                (int) MinorSkills[4], (int) MajorSkills[4], Playable ? 1 : 0, autoCalcFlags));
        }

        protected override void UpdateRequired(Record record)
        {
            record.GetSubRecord<StringSubRecord>("NAME").Data = Name;
            record.GetSubRecord<StringSubRecord>("FNAM").Data = DisplayName;

            var classData = record.GetSubRecord<ClassData>("CLDT");
            classData.Flags = Playable ? 1 : 0;
            classData.Specialization = (int) Specialization;
            classData.AttributeId1 = (int) Attributes[0];
            classData.AttributeId2 = (int) Attributes[1];

            classData.MinorId1 = (int) MinorSkills[0];
            classData.MinorId2 = (int) MinorSkills[1];
            classData.MinorId3 = (int) MinorSkills[2];
            classData.MinorId4 = (int) MinorSkills[3];
            classData.MinorId5 = (int) MinorSkills[4];

            classData.MajorId1 = (int) MajorSkills[0];
            classData.MajorId2 = (int) MajorSkills[1];
            classData.MajorId3 = (int) MajorSkills[2];
            classData.MajorId4 = (int) MajorSkills[3];
            classData.MajorId5 = (int) MajorSkills[4];
            
            FlagSet(classData, BuysWeapons, WEAPONS_FLAG, ClassDataFlagSet, ClassDataFlagClear);
            FlagSet(classData, BuysArmor, ARMOR_FLAG, ClassDataFlagSet, ClassDataFlagClear);
            FlagSet(classData, BuysClothing, CLOTHING_FLAG, ClassDataFlagSet, ClassDataFlagClear);
            FlagSet(classData, BuysBooks, BOOKS_FLAG, ClassDataFlagSet, ClassDataFlagClear);
            FlagSet(classData, BuysIngredients, INGREDIENT_FLAG, ClassDataFlagSet, ClassDataFlagClear);
            FlagSet(classData, BuysPicks, PICKS_FLAG, ClassDataFlagSet, ClassDataFlagClear);
            FlagSet(classData, BuysProbes, PROBES_FLAG, ClassDataFlagSet, ClassDataFlagClear);
            FlagSet(classData, BuysLights, LIGHTS_FLAG, ClassDataFlagSet, ClassDataFlagClear);
            FlagSet(classData, BuysApparatus, APPARATUS_FLAG, ClassDataFlagSet, ClassDataFlagClear);
            FlagSet(classData, OffersRepair, REPAIR_FLAG, ClassDataFlagSet, ClassDataFlagClear);
            FlagSet(classData, BuysMiscItems, MISC_FLAG, ClassDataFlagSet, ClassDataFlagClear);
            FlagSet(classData, SellsSpells, SPELLS_FLAG, ClassDataFlagSet, ClassDataFlagClear);
            FlagSet(classData, BuysMagicItems, MAGIC_ITEMS_FLAG, ClassDataFlagSet, ClassDataFlagClear);
            FlagSet(classData, BuysPotions, POTIONS_FLAG, ClassDataFlagSet, ClassDataFlagClear);
            FlagSet(classData, OffersTraining, TRAINING_FLAG, ClassDataFlagSet, ClassDataFlagClear);
            FlagSet(classData, OffersSpellmaking, SPELLMAKING_FLAG, ClassDataFlagSet, ClassDataFlagClear);
            FlagSet(classData, OffersEnchanting, ENCHANTING_FLAG, ClassDataFlagSet, ClassDataFlagClear);
            FlagSet(classData, BuysRepairItems, REPAIR_ITEM_FLAG, ClassDataFlagSet, ClassDataFlagClear);

        }

        protected override void UpdateOptional(Record record)
        {
            ProcessOptional(record, "DESC", Description != null, () => new StringSubRecord("DESC", Description), (sr) => sr.Data = Description);
            ProcessOptional(record, "DELE", Deleted, () => new IntSubRecord("DELE", 0), (sr) => sr.Data = 0);
        }

        protected override void DoSyncWithRecord(Record record)
        {
            // Required
            Id = record.GetSubRecord<StringSubRecord>("NAME").Data;
            DisplayName = record.GetSubRecord<StringSubRecord>("FNAM").Data;
            
            var classData = record.GetSubRecord<ClassData>("CLDT");
            Playable = classData.Flags == 1;
            Specialization = (SpecializationType) classData.Specialization;
            Attributes[0] = (AttributeType) classData.AttributeId1;
            Attributes[1] = (AttributeType) classData.AttributeId2;

            MinorSkills[0] = (SkillType) classData.MinorId1;
            MinorSkills[1] = (SkillType) classData.MinorId2;
            MinorSkills[2] = (SkillType) classData.MinorId3;
            MinorSkills[3] = (SkillType) classData.MinorId4;
            MinorSkills[4] = (SkillType) classData.MinorId5;

            MajorSkills[0] = (SkillType) classData.MajorId1;
            MajorSkills[1] = (SkillType) classData.MajorId2;
            MajorSkills[2] = (SkillType) classData.MajorId3;
            MajorSkills[3] = (SkillType) classData.MajorId4;
            MajorSkills[4] = (SkillType) classData.MajorId5;


            BuysWeapons = HasFlagSet(classData.AutoCalcFlags, WEAPONS_FLAG);
            BuysArmor = HasFlagSet(classData.AutoCalcFlags, ARMOR_FLAG);
            BuysClothing = HasFlagSet(classData.AutoCalcFlags, CLOTHING_FLAG);
            BuysBooks = HasFlagSet(classData.AutoCalcFlags, BOOKS_FLAG);
            BuysIngredients = HasFlagSet(classData.AutoCalcFlags, INGREDIENT_FLAG);
            BuysPicks = HasFlagSet(classData.AutoCalcFlags, PICKS_FLAG);
            BuysProbes = HasFlagSet(classData.AutoCalcFlags, PROBES_FLAG);
            BuysLights = HasFlagSet(classData.AutoCalcFlags, LIGHTS_FLAG);
            BuysApparatus = HasFlagSet(classData.AutoCalcFlags, APPARATUS_FLAG);
            OffersRepair = HasFlagSet(classData.AutoCalcFlags, REPAIR_FLAG);
            BuysMiscItems = HasFlagSet(classData.AutoCalcFlags, MISC_FLAG);
            SellsSpells = HasFlagSet(classData.AutoCalcFlags, SPELLS_FLAG);
            BuysMagicItems = HasFlagSet(classData.AutoCalcFlags, MAGIC_ITEMS_FLAG);
            BuysPotions = HasFlagSet(classData.AutoCalcFlags, POTIONS_FLAG);
            OffersTraining = HasFlagSet(classData.AutoCalcFlags, TRAINING_FLAG);
            OffersSpellmaking = HasFlagSet(classData.AutoCalcFlags, SPELLMAKING_FLAG);
            OffersEnchanting = HasFlagSet(classData.AutoCalcFlags, ENCHANTING_FLAG);
            BuysRepairItems = HasFlagSet(classData.AutoCalcFlags, REPAIR_ITEM_FLAG);


            // Optional
            Description = record.TryGetSubRecord<StringSubRecord>("DESC")?.Data;
            Deleted = record.ContainsSubRecord("DELE");
            
        }

        protected override void DoValidateRecord(Record record, Validator validator)
        {
            validator.CheckRequired(record, "NAME");
            validator.CheckRequired(record, "FNAM");
            validator.CheckRequired(record, "CLDT");
        }

        public override TES3GameItem Clone()
        {
            var result = new CharacterClass(Name)
            {
                DisplayName = DisplayName,
                Specialization = Specialization,
                Description = Description,
                Deleted = Deleted,
                Playable = Playable
            };
            Array.Copy(Attributes, result.Attributes, Attributes.Length);
            Array.Copy(MajorSkills, result.MajorSkills, MajorSkills.Length);
            Array.Copy(MinorSkills, result.MinorSkills, MinorSkills.Length);
            CopyClone(result);
            return result;
        }

        public override void StreamDebug(TextWriter target)
        {
            var writer = new IndentWriter(target);

            writer.WriteLine(ToString());

            writer.IncIndent();
            writer.WriteLine($"Name: {DisplayName}");
            writer.WriteLine($"Description: {Description}");
            writer.WriteLine($"Specialization: {Specialization}");

            writer.WriteLine("Favored Attributes");
            writer.IncIndent();
            foreach (var attribute in Attributes)
            {
                writer.WriteLine(attribute);
            }
            writer.DecIndent();

            writer.WriteLine("Major Skills");
            writer.IncIndent();
            foreach (var skill in MajorSkills)
            {
                writer.WriteLine(skill);
            }
            writer.DecIndent();

            writer.WriteLine("Minor Skills");
            writer.IncIndent();
            foreach (var skill in MinorSkills)
            {
                writer.WriteLine(skill);
            }
            writer.DecIndent();

            if (OffersServices)
            {
                writer.WriteLine("Default Services");
                writer.IncIndent();
                writer.WriteLine($"Enchanting: {OffersEnchanting}");
                writer.WriteLine($"Repair: {OffersRepair}");
                writer.WriteLine($"Spells: {SellsSpells}");
                writer.WriteLine($"Spellmaking: {OffersSpellmaking}");
                writer.WriteLine($"Training: {OffersTraining}");
                writer.DecIndent();
            }

            if (IsMerchant)
            {
                writer.WriteLine("Default Barter");
                writer.IncIndent();
                writer.WriteLine($"Apparatus: {BuysApparatus}");
                writer.WriteLine($"Armor: {BuysArmor}");
                writer.WriteLine($"Books: {BuysBooks}");
                writer.WriteLine($"Clothing: {BuysClothing}");
                writer.WriteLine($"Ingredients: {BuysIngredients}");
                writer.WriteLine($"Lights: {BuysLights}");
                writer.WriteLine($"Magic Items: {BuysMagicItems}");
                writer.WriteLine($"Misc: {BuysMiscItems}");
                writer.WriteLine($"Picks: {BuysPicks}");
                writer.WriteLine($"Potions: {BuysPotions}");
                writer.WriteLine($"Probes: {BuysProbes}");
                writer.WriteLine($"Repair Items: {BuysRepairItems}");
                writer.WriteLine($"Weapons: {BuysWeapons}");
                writer.DecIndent();
            }

            writer.DecIndent();
        }

        public override string ToString()
        {
            return $"Character Class ({Name})";
        }


        static void ClassDataFlagSet(ClassData subRecord, int flag)
        {
            subRecord.AutoCalcFlags |= flag;
        }

        static void ClassDataFlagClear(ClassData subRecord, int flag)
        {
            subRecord.AutoCalcFlags &= ~flag;
        }
    }


}