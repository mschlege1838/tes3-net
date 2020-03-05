
using System;
using System.Collections.Generic;
using System.IO;
using TES3.GameItem.Part;
using TES3.GameItem.TypeConstant;
using TES3.Records;
using TES3.Records.SubRecords;
using TES3.Util;

namespace TES3.GameItem.Item
{

    [TargetRecord("NPC_")]
    public class NPC : Actor
    {
        const int FEMALE_FLAG = 0x0001;
        const int ESSENTIAL_FLAG = 0x0002;
        const int RESPAWN_FLAG = 0x0004;
        const int NONE_FLAG = 0x0008;
        const int AUTO_CALC_FLAG = 0x0010;

        byte strength;
        byte intelligence;
        byte willpower;
        byte agility;
        byte speed;
        byte endurance;
        byte personality;
        byte luck;

        byte block;
        byte armorer;
        byte mediumArmor;
        byte heavyArmor;
        byte blunt;
        byte longBlade;
        byte axe;
        byte spear;
        byte athletics;
        byte enchant;
        byte destruction;
        byte alteration;
        byte illusion;
        byte conjuration;
        byte mysticism;
        byte restoration;
        byte alchemy;
        byte unarmored;
        byte security;
        byte sneak;
        byte acrobatics;
        byte lightArmor;
        byte shortBlade;
        byte marksman;
        byte mercantile;
        byte speechcraft;
        byte handToHand;

        short health;
        short magicka;
        short fatigue;
        short level;

        string race;
        string faction;
        string characterClass;
        string headModel;
        string hairModel;

        byte disposition;
        public NPC(string name, string race, string characterClass, string headModel, string hairModel) : base(name)
        {
            Race = race;
            Class = characterClass;
            HeadModel = headModel;
            HairModel = hairModel;
        }

        public NPC(Record record) : base(record)
        {

        }

        public override string RecordName => "NPC_";

        public string Race
        {
            get => race;
            set => race = Validation.NotNull(value, "value", "Race");
        }

        public string Faction
        {
            get => faction;
            set => faction = Validation.NotNull(value, "value", "Faction");
        }

        public string HeadModel
        {
            get => headModel;
            set => headModel = Validation.NotNull(value, "value", "Head Model");
        }

        public string Class
        {
            get => characterClass;
            set => characterClass = Validation.NotNull(value, "value", "Class");
        }

        public string HairModel
        {
            get => hairModel;
            set => hairModel = Validation.NotNull(value, "value", "Hair Model");
        }

        public bool Female
        {
            get;
            set;
        }

        public bool AutoCalc
        {
            get;
            set;
        }

        public short Level
        {
            get => level;
            set => level = Validation.Gte(value, (short) 0, "value", "Level");
        }

        public byte Strength
        {
            get => strength;
            set => strength = AutoCalc ? throw AutoCalcStateError("Strength") : Validation.Gte(value, (byte) 0, "value", "Strength");
        }

        public byte Intelligence
        {
            get => intelligence;
            set => intelligence = AutoCalc ? throw AutoCalcStateError("Intelligence") : Validation.Gte(value, (byte) 0, "value", "Intelligence");
        }

        public byte Willpower
        {
            get => willpower;
            set => willpower = AutoCalc ? throw AutoCalcStateError("Willpower") : Validation.Gte(value, (byte) 0, "value", "Willpower");
        }

        public byte Agility
        {
            get => agility;
            set => agility = AutoCalc ? throw AutoCalcStateError("Agility") : Validation.Gte(value, (byte) 0, "value", "Agility");
        }

        public byte Speed
        {
            get => speed;
            set => speed = AutoCalc ? throw AutoCalcStateError("Speed") : Validation.Gte(value, (byte) 0, "value", "Speed");
        }

        public byte Endurance
        {
            get => endurance;
            set => endurance = AutoCalc ? throw AutoCalcStateError("Endurance") : Validation.Gte(value, (byte) 0, "value", "Endurance");
        }

        public byte Personality
        {
            get => personality;
            set => personality = AutoCalc ? throw AutoCalcStateError("Personality") : Validation.Gte(value, (byte) 0, "value", "Personality");
        }

        public byte Luck
        {
            get => luck;
            set => luck = AutoCalc ? throw AutoCalcStateError("Luck") : Validation.Gte(value, (byte) 0, "value", "Luck");
        }


        public byte Block
        {
            get => block;
            set => block = AutoCalc ? throw AutoCalcStateError("Block") : Validation.Gte(value, (byte) 0, "value", "Block");
        }

        public byte Armorer
        {
            get => armorer;
            set => armorer = AutoCalc ? throw AutoCalcStateError("Armorer") : Validation.Gte(value, (byte) 0, "value", "Armorer");
        }

        public byte MediumArmor
        {
            get => mediumArmor;
            set => mediumArmor = AutoCalc ? throw AutoCalcStateError("MediumArmor") : Validation.Gte(value, (byte) 0, "value", "MediumArmor");
        }

        public byte HeavyArmor
        {
            get => heavyArmor;
            set => heavyArmor = AutoCalc ? throw AutoCalcStateError("HeavyArmor") : Validation.Gte(value, (byte) 0, "value", "HeavyArmor");
        }

        public byte Blunt
        {
            get => blunt;
            set => blunt = AutoCalc ? throw AutoCalcStateError("Blunt") : Validation.Gte(value, (byte) 0, "value", "Blunt");
        }

        public byte LongBlade
        {
            get => longBlade;
            set => longBlade = AutoCalc ? throw AutoCalcStateError("LongBlade") : Validation.Gte(value, (byte) 0, "value", "LongBlade");
        }

        public byte Axe
        {
            get => axe;
            set => axe = AutoCalc ? throw AutoCalcStateError("Axe") : Validation.Gte(value, (byte) 0, "value", "Axe");
        }

        public byte Spear
        {
            get => spear;
            set => spear = AutoCalc ? throw AutoCalcStateError("Spear") : Validation.Gte(value, (byte) 0, "value", "Spear");
        }

        public byte Athletics
        {
            get => athletics;
            set => athletics = AutoCalc ? throw AutoCalcStateError("Athletics") : Validation.Gte(value, (byte) 0, "value", "Athletics");
        }

        public byte Enchant
        {
            get => enchant;
            set => enchant = AutoCalc ? throw AutoCalcStateError("Enchant") : Validation.Gte(value, (byte) 0, "value", "Enchant");
        }

        public byte Destruction
        {
            get => destruction;
            set => destruction = AutoCalc ? throw AutoCalcStateError("Destruction") : Validation.Gte(value, (byte) 0, "value", "Destruction");
        }

        public byte Alteration
        {
            get => alteration;
            set => alteration = AutoCalc ? throw AutoCalcStateError("Alteration") : Validation.Gte(value, (byte) 0, "value", "Alteration");
        }

        public byte Illusion
        {
            get => illusion;
            set => illusion = AutoCalc ? throw AutoCalcStateError("Illusion") : Validation.Gte(value, (byte) 0, "value", "Illusion");
        }

        public byte Conjuration
        {
            get => conjuration;
            set => conjuration = AutoCalc ? throw AutoCalcStateError("Conjuration") : Validation.Gte(value, (byte) 0, "value", "Conjuration");
        }

        public byte Mysticism
        {
            get => mysticism;
            set => mysticism = AutoCalc ? throw AutoCalcStateError("Mysticism") : Validation.Gte(value, (byte) 0, "value", "Mysticism");
        }

        public byte Restoration
        {
            get => restoration;
            set => restoration = AutoCalc ? throw AutoCalcStateError("Restoration") : Validation.Gte(value, (byte) 0, "value", "Restoration");
        }

        public byte Alchemy
        {
            get => alchemy;
            set => alchemy = AutoCalc ? throw AutoCalcStateError("Alchemy") : Validation.Gte(value, (byte) 0, "value", "Alchemy");
        }

        public byte Unarmored
        {
            get => unarmored;
            set => unarmored = AutoCalc ? throw AutoCalcStateError("Unarmored") : Validation.Gte(value, (byte) 0, "value", "Unarmored");
        }

        public byte Security
        {
            get => security;
            set => security = AutoCalc ? throw AutoCalcStateError("Security") : Validation.Gte(value, (byte) 0, "value", "Security");
        }

        public byte Sneak
        {
            get => sneak;
            set => sneak = AutoCalc ? throw AutoCalcStateError("Sneak") : Validation.Gte(value, (byte) 0, "value", "Sneak");
        }

        public byte Acrobatics
        {
            get => acrobatics;
            set => acrobatics = AutoCalc ? throw AutoCalcStateError("Acrobatics") : Validation.Gte(value, (byte) 0, "value", "Acrobatics");
        }

        public byte LightArmor
        {
            get => lightArmor;
            set => lightArmor = AutoCalc ? throw AutoCalcStateError("LightArmor") : Validation.Gte(value, (byte) 0, "value", "LightArmor");
        }

        public byte ShortBlade
        {
            get => shortBlade;
            set => shortBlade = AutoCalc ? throw AutoCalcStateError("ShortBlade") : Validation.Gte(value, (byte) 0, "value", "ShortBlade");
        }

        public byte Marksman
        {
            get => marksman;
            set => marksman = AutoCalc ? throw AutoCalcStateError("Marksman") : Validation.Gte(value, (byte) 0, "value", "Marksman");
        }

        public byte Mercantile
        {
            get => mercantile;
            set => mercantile = AutoCalc ? throw AutoCalcStateError("Mercantile") : Validation.Gte(value, (byte) 0, "value", "Mercantile");
        }

        public byte Speechcraft
        {
            get => speechcraft;
            set => speechcraft = AutoCalc ? throw AutoCalcStateError("Speechcraft") : Validation.Gte(value, (byte) 0, "value", "Speechcraft");
        }

        public byte HandToHand
        {
            get => handToHand;
            set => handToHand = AutoCalc ? throw AutoCalcStateError("HandToHand") : Validation.Gte(value, (byte) 0, "value", "HandToHand");
        }



        public short Health
        {
            get => health;
            set => health = AutoCalc ? throw AutoCalcStateError("Health") : value >= 0 ? value : Validation.Gte(value, (short) -1, "value", "Health");
        }

        public short Magicka
        {
            get => magicka;
            set => magicka = AutoCalc ? throw AutoCalcStateError("Magicka") : Validation.Gte(value, (short) 0, "value", "Magicka");
        }

        public short Fatigue
        {
            get => fatigue;
            set => fatigue = AutoCalc ? throw AutoCalcStateError("Fatigue") : Validation.Gte(value, (short) 0, "value", "Fatigue");
        }



        public byte Reputation
        {
            get;
            set;
        }

        public byte Disposition
        {
            get => disposition;
            set => disposition = Validation.Range(value, (byte) 0, (byte) 100, "value", "Disposition");
        }

        public byte FactionRank
        {
            get;
            set;
        }

        protected override void OnCreate(IList<SubRecord> subRecords)
        {
            subRecords.Add(new StringSubRecord("NAME", Name));
            subRecords.Add(new StringSubRecord("RNAM", Race));
            subRecords.Add(new StringSubRecord("CNAM", Class));
            subRecords.Add(new StringSubRecord("ANAM", Faction));
            subRecords.Add(new StringSubRecord("BNAM", HeadModel));
            subRecords.Add(new StringSubRecord("KNAM", HairModel));

            if (AutoCalc)
            {
                subRecords.Add(new NPCData12("NPDT", Level, Disposition, Reputation, FactionRank, 0, 0, 0, BarterGold));
            }
            else
            {
                var skills = new byte[Constants.SKILLS_COUNT];
                ProcessSkills(skills);
                subRecords.Add(new NPCData52("NPDT", Level, Strength, Intelligence, Willpower, Agility, Speed, Endurance, Personality, Luck, skills,
                    Reputation, Health, Magicka, Fatigue, Disposition, Reputation, FactionRank, 0, BarterGold));
            }

            var flags = 0;
            if (Female)
            {
                flags |= FEMALE_FLAG;
            }
            if (Essential)
            {
                flags |= ESSENTIAL_FLAG;
            }
            if (Respawn)
            {
                flags |= RESPAWN_FLAG;
            }
            if (AutoCalc)
            {
                flags |= AUTO_CALC_FLAG;
            }
            if (flags == 0)
            {
                flags |= NONE_FLAG;
            }
            subRecords.Add(new IntSubRecord("FLAG", flags));

            if (!IsPlayer)
            {
                subRecords.Add(new AIData("AIDT", Hello, 0, Fight, Flee, Alarm, 0, 0, 0, CalculateAIDataFlags()));
            }
        }

        protected override void UpdateRequired(Record record)
        {
            base.UpdateRequired(record);

            record.GetSubRecord<StringSubRecord>("RNAM").Data = Race;
            record.GetSubRecord<StringSubRecord>("ANAM").Data = Faction;
            record.GetSubRecord<StringSubRecord>("BNAM").Data = HeadModel;
            record.GetSubRecord<StringSubRecord>("CNAM").Data = Class;
            record.GetSubRecord<StringSubRecord>("KNAM").Data = HairModel;

            var flags = record.GetSubRecord<IntSubRecord>("FLAG");
            ActorFlagSet(flags, Female, FEMALE_FLAG);
            ActorFlagSet(flags, Essential, ESSENTIAL_FLAG);
            ActorFlagSet(flags, Respawn, RESPAWN_FLAG);
            ActorFlagSet(flags, AutoCalc, AUTO_CALC_FLAG);
            if (flags.Data == 0)
            {
                flags.Data |= NONE_FLAG;
            }

            var npcBase = record.GetSubRecord<NPCDataBase>("NPDT");
            npcBase.Level = Level;
            npcBase.FactionId = Reputation;
            npcBase.Disposition = Disposition;
            npcBase.Rank = FactionRank;
            npcBase.Gold = BarterGold;

            if (!AutoCalc)
            {
                var npcData = record.GetSubRecord<NPCData52>("NPDT");

                npcData.Strength = Strength;
                npcData.Intelligence = Intelligence;
                npcData.Willpower = Willpower;
                npcData.Agility = Agility;
                npcData.Speed = Speed;
                npcData.Endurance = Endurance;
                npcData.Personality = Personality;

                ProcessSkills(npcData.Skills);

                npcData.Health = Health;
                npcData.SpellPoints = Magicka;
                npcData.Fatigue = Fatigue;
            }
        }


        protected override void DoSyncWithRecord(Record record)
        {
            base.DoSyncWithRecord(record);
            Race = record.GetSubRecord<StringSubRecord>("RNAM").Data;
            Faction = record.GetSubRecord<StringSubRecord>("ANAM").Data;
            HeadModel = record.GetSubRecord<StringSubRecord>("BNAM").Data;
            Class = record.GetSubRecord<StringSubRecord>("CNAM").Data;
            HairModel = record.GetSubRecord<StringSubRecord>("KNAM").Data;

            var flags = record.GetSubRecord<IntSubRecord>("FLAG");
            Female = HasFlagSet(flags.Data, FEMALE_FLAG);
            Essential = HasFlagSet(flags.Data, ESSENTIAL_FLAG);
            Respawn = HasFlagSet(flags.Data, RESPAWN_FLAG);
            AutoCalc = HasFlagSet(flags.Data, AUTO_CALC_FLAG);

            var npcData = record.GetSubRecord<NPCDataBase>("NPDT");
            Level = npcData.Level;
            Reputation = npcData.FactionId;
            Disposition = npcData.Disposition;
            FactionRank = npcData.Rank;
            BarterGold = npcData.Gold;
            if (!AutoCalc)
            {
                var npcDataExt = (NPCData52) npcData;

                Strength = npcDataExt.Strength;
                Intelligence = npcDataExt.Intelligence;
                Willpower = npcDataExt.Willpower;
                Agility = npcDataExt.Agility;
                Speed = npcDataExt.Speed;
                Endurance = npcDataExt.Endurance;
                Luck = npcDataExt.Luck;

                Block = npcDataExt.Skills[(int) SkillType.Block];
                Armorer = npcDataExt.Skills[(int) SkillType.Armorer];
                MediumArmor = npcDataExt.Skills[(int) SkillType.MediumArmor];
                HeavyArmor = npcDataExt.Skills[(int) SkillType.HeavyArmor];
                Blunt = npcDataExt.Skills[(int) SkillType.Blunt];
                LongBlade = npcDataExt.Skills[(int) SkillType.LongBlade];
                Axe = npcDataExt.Skills[(int) SkillType.Axe];
                Spear = npcDataExt.Skills[(int) SkillType.Spear];
                Athletics = npcDataExt.Skills[(int) SkillType.Athletics];
                Enchant = npcDataExt.Skills[(int) SkillType.Enchant];
                Destruction = npcDataExt.Skills[(int) SkillType.Destruction];
                Alteration = npcDataExt.Skills[(int) SkillType.Alteration];
                Illusion = npcDataExt.Skills[(int) SkillType.Illusion];
                Conjuration = npcDataExt.Skills[(int) SkillType.Conjuration];
                Mysticism = npcDataExt.Skills[(int) SkillType.Mysticism];
                Restoration = npcDataExt.Skills[(int) SkillType.Restoration];
                Alchemy = npcDataExt.Skills[(int) SkillType.Alchemy];
                Unarmored = npcDataExt.Skills[(int) SkillType.Unarmored];
                Security = npcDataExt.Skills[(int) SkillType.Security];
                Sneak = npcDataExt.Skills[(int) SkillType.Sneak];
                Acrobatics = npcDataExt.Skills[(int) SkillType.Acrobatics];
                LightArmor = npcDataExt.Skills[(int) SkillType.LightArmor];
                ShortBlade = npcDataExt.Skills[(int) SkillType.ShortBlade];
                Marksman = npcDataExt.Skills[(int) SkillType.Marksman];
                Mercantile = npcDataExt.Skills[(int) SkillType.Mercantile];
                Speechcraft = npcDataExt.Skills[(int) SkillType.Speechcraft];
                HandToHand = npcDataExt.Skills[(int) SkillType.HandToHand];

                Health = npcDataExt.Health;
                Magicka = npcDataExt.SpellPoints;
                Fatigue = npcDataExt.Fatigue;
            }
        }

        protected override void DoValidateRecord(Record record, Validator validator)
        {
            base.DoValidateRecord(record, validator);
            validator.CheckRequired(record, "RNAM");
            validator.CheckRequired(record, "CNAM");
            validator.CheckRequired(record, "ANAM");
            validator.CheckRequired(record, "BNAM");
            validator.CheckRequired(record, "KNAM");
        }

        public override TES3GameItem Clone()
        {
            var result = new NPC(Name, Race, Class, HeadModel, HairModel)
            {
                Faction = Faction,
                Female = Female,
                AutoCalc = AutoCalc,
                Level = Level,
                Reputation = Reputation,
                Disposition = Disposition,
                FactionRank = FactionRank,
                strength = strength,
                intelligence = intelligence,
                willpower = willpower,
                agility = agility,
                speed = speed,
                endurance = endurance,
                personality = personality,
                luck = luck,
                block = block,
                armorer = armorer,
                mediumArmor = mediumArmor,
                heavyArmor = heavyArmor,
                blunt = blunt,
                longBlade = longBlade,
                axe = axe,
                spear = spear,
                athletics = athletics,
                enchant = enchant,
                destruction = destruction,
                alteration = alteration,
                illusion = illusion,
                conjuration = conjuration,
                mysticism = mysticism,
                restoration = restoration,
                alchemy = alchemy,
                unarmored = unarmored,
                security = security,
                sneak = sneak,
                acrobatics = acrobatics,
                lightArmor = lightArmor,
                shortBlade = shortBlade,
                marksman = marksman,
                mercantile = mercantile,
                speechcraft = speechcraft,
                handToHand = handToHand,
                health = health,
                magicka = magicka,
                fatigue = fatigue
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
            writer.WriteLine($"Class: {Class}");
            writer.WriteLine($"Level: {Level}");
            writer.WriteLine($"Race: {Race}");
            writer.WriteLine($"Female: {Female}");
            writer.WriteLine($"Head Model: {HeadModel}");
            writer.WriteLine($"Hair Model: {HairModel}");
            writer.WriteLine($"Essential: {Essential}");
            writer.WriteLine($"Respawn: {Respawn}");
            writer.WriteLine($"Disposition: {Disposition}");
            writer.WriteLine($"Reputation: {Reputation}");
            writer.WriteLine($"Faction: {Faction}");
            writer.WriteLine($"Faction Rank: {FactionRank}");
            writer.WriteLine($"Script: {Script}");

            var autoCalc = AutoCalc;
            writer.WriteLine($"Auto Calc: {autoCalc}");
            if (!autoCalc)
            {
                writer.WriteLine("Stats");
                writer.IncIndent();
                writer.WriteLine($"Health: {Health}");
                writer.WriteLine($"Magicka: {Magicka}");
                writer.WriteLine($"Fatigue: {Fatigue}");
                writer.DecIndent();

                writer.WriteLine("Attributes");
                writer.IncIndent();
                writer.WriteLine($"Strength: {Strength}");
                writer.WriteLine($"Intelligence: {Intelligence}");
                writer.WriteLine($"Willpower: {Willpower}");
                writer.WriteLine($"Agility: {Agility}");
                writer.WriteLine($"Speed: {Speed}");
                writer.WriteLine($"Endurance: {Endurance}");
                writer.WriteLine($"Personality: {Personality}");
                writer.WriteLine($"Luck: {Luck}");
                writer.DecIndent();

                writer.WriteLine("Skills");
                writer.IncIndent();
                writer.WriteLine($"Heavy Armor: {HeavyArmor}");
                writer.WriteLine($"Medium Armor: {MediumArmor}");
                writer.WriteLine($"Spear: {Spear}");
                writer.WriteLine($"Acrobatics: {Acrobatics}");
                writer.WriteLine($"Armorer: {Armorer}");
                writer.WriteLine($"Axe: {Axe}");
                writer.WriteLine($"Blunt: {Blunt}");
                writer.WriteLine($"Long Blade: {LongBlade}");
                writer.WriteLine($"Block: {Block}");
                writer.WriteLine($"Light Armor: {LightArmor}");
                writer.WriteLine($"Marksman: {Marksman}");
                writer.WriteLine($"Sneak: {Sneak}");
                writer.WriteLine($"Athletics: {Athletics}");
                writer.WriteLine($"Hand-to-Hand: {HandToHand}");
                writer.WriteLine($"Short Blade: {ShortBlade}");
                writer.WriteLine($"Unarmored: {Unarmored}");
                writer.WriteLine($"Illusion: {Illusion}");
                writer.WriteLine($"Mercantile: {Mercantile}");
                writer.WriteLine($"Speechcraft: {Speechcraft}");
                writer.WriteLine($"Alchemy: {Alchemy}");
                writer.WriteLine($"Conjuration: {Conjuration}");
                writer.WriteLine($"Enchant: {Enchant}");
                writer.WriteLine($"Security: {Security}");
                writer.WriteLine($"Alteration: {Alteration}");
                writer.WriteLine($"Destruction: {Destruction}");
                writer.WriteLine($"Mysticism: {Mysticism}");
                writer.WriteLine($"Restoration: {Restoration}");
                writer.DecIndent();
            }

            if (InventoryList.Count > 0)
            {
                writer.WriteLine("Inventory");
                writer.IncIndent();
                var table = new Table("Name", "Count");
                foreach (var item in InventoryList)
                {
                    table.AddRow(item.Name, item.Count);
                }
                table.Print(target, writer.Indent);
                writer.DecIndent();
            }

            if (SpellList.Count > 0)
            {
                writer.WriteLine("Spells");
                writer.IncIndent();
                foreach (var spell in SpellList)
                {
                    writer.WriteLine(spell);
                }
                writer.DecIndent();
            }

            if (!IsPlayer)
            {
                writer.WriteLine("Services Information");
                writer.IncIndent();
                writer.WriteLine($"Spells: {SellsSpells}");
                writer.WriteLine($"Spellmaking: {OffersSpellmaking}");
                writer.WriteLine($"Enchanting: {OffersEnchanting}");
                writer.WriteLine($"Repair: {OffersRepair}");
                writer.WriteLine($"Training: {OffersTraining}");
                var travelDestinations = TravelDestinations;
                if (travelDestinations.Count > 0)
                {
                    writer.WriteLine("Travel");
                    writer.IncIndent();
                    foreach (var destination in travelDestinations)
                    {
                        writer.WriteLine($"{destination.CellName} {destination.Position}");
                    }
                    writer.DecIndent();
                }
                writer.DecIndent();

                if (IsMerchant)
                {
                    writer.WriteLine("Barter Information");
                    writer.IncIndent();
                    writer.WriteLine($"Barter Gold: {BarterGold}");
                    writer.WriteLine($"Buys/Sells");
                    writer.IncIndent();
                    writer.WriteLine($"Apparatus: {BuysApparatus}");
                    writer.WriteLine($"Armor: {BuysArmor}");
                    writer.WriteLine($"Books: {BuysBooks}");
                    writer.WriteLine($"Clothing: {BuysClothing}");
                    writer.WriteLine($"Ingredients: {BuysIngredients}");
                    writer.WriteLine($"Lights: {BuysLights}");
                    writer.WriteLine($"Magic Items: {BuysMagicItems}");
                    writer.WriteLine($"Misc Items: {BuysMiscItems}");
                    writer.WriteLine($"Picks: {BuysPicks}");
                    writer.WriteLine($"Potions: {BuysPotions}");
                    writer.WriteLine($"Probes: {BuysProbes}");
                    writer.WriteLine($"Repair Items: {BuysRepairItems}");
                    writer.WriteLine($"Weapons: {BuysWeapons}");
                    writer.DecIndent();
                    writer.DecIndent();
                }

                writer.WriteLine("AI Information");
                writer.IncIndent();
                writer.WriteLine($"Alarm: {Alarm}");
                writer.WriteLine($"Fight: {Fight}");
                writer.WriteLine($"Flee: {Flee}");
                writer.WriteLine($"Hello: {Hello}");

                var packageList = AIPackageList;
                if (packageList.Count > 0)
                {
                    writer.WriteLine("Package List");
                    writer.IncIndent();
                    foreach (var package in packageList)
                    {
                        switch (package.Type)
                        {
                            case AIPackageType.Wander:
                                writer.WriteLine("Wander");
                                writer.IncIndent();
                                writer.WriteLine($"Distance: {package.Distance}");
                                writer.WriteLine($"Duration: {package.Duration}");
                                writer.WriteLine($"Time Of Day: {package.TimeOfDay}");
                                writer.WriteLine($"Looking Around: {package.IdleLookingAround}");
                                writer.WriteLine($"Looking Behind: {package.IdleLookingBehind}");
                                writer.WriteLine($"Scratching Head: {package.IdleScratchingHead}");
                                writer.WriteLine($"Reaching For Shoulder: {package.IdleReachingForShoulder}");
                                writer.WriteLine($"Rubbing Hands: {package.IdleRubbingHands}");
                                writer.WriteLine($"Deep Thought: {package.IdleDeepThought}");
                                writer.WriteLine($"Reaching For Weapon: {package.IdleReachingForWeapon}");
                                writer.DecIndent();
                                break;
                            case AIPackageType.Travel:
                                writer.WriteLine("Travel");
                                writer.IncIndent();
                                writer.WriteLine($"X: {package.X}");
                                writer.WriteLine($"Y: {package.Y}");
                                writer.WriteLine($"Z: {package.Z}");
                                writer.DecIndent();
                                break;
                            case AIPackageType.Follow:
                            case AIPackageType.Escort:
                                writer.WriteLine(package.Type);
                                writer.IncIndent();
                                writer.WriteLine($"X: {package.X}");
                                writer.WriteLine($"Y: {package.Y}");
                                writer.WriteLine($"Z: {package.Z}");
                                writer.WriteLine($"Duration: {package.Duration}");
                                writer.WriteLine($"Actor ID: {package.ActorID}");
                                writer.WriteLine($"Cell: {package.CellName}");
                                writer.DecIndent();
                                break;
                            case AIPackageType.Activate:
                                writer.WriteLine("Activate");
                                writer.IncIndent();
                                writer.WriteLine($"Object ID: {package.ObjectID}");
                                writer.DecIndent();
                                break;
                        }
                    }
                    writer.DecIndent();
                }
                writer.DecIndent();

            }

            writer.WriteLine($"Blood: {BloodType}");
            writer.WriteLine($"Animation: {Animation}");
        }

        void ProcessSkills(byte[] skills)
        {
            skills[(int) SkillType.Block] = Block;
            skills[(int) SkillType.Armorer] = Armorer;
            skills[(int) SkillType.MediumArmor] = MediumArmor;
            skills[(int) SkillType.HeavyArmor] = HeavyArmor;
            skills[(int) SkillType.Blunt] = Blunt;
            skills[(int) SkillType.LongBlade] = LongBlade;
            skills[(int) SkillType.Axe] = Axe;
            skills[(int) SkillType.Spear] = Spear;
            skills[(int) SkillType.Athletics] = Athletics;
            skills[(int) SkillType.Enchant] = Enchant;
            skills[(int) SkillType.Destruction] = Destruction;
            skills[(int) SkillType.Alteration] = Alteration;
            skills[(int) SkillType.Illusion] = Illusion;
            skills[(int) SkillType.Conjuration] = Conjuration;
            skills[(int) SkillType.Mysticism] = Mysticism;
            skills[(int) SkillType.Restoration] = Restoration;
            skills[(int) SkillType.Alchemy] = Alchemy;
            skills[(int) SkillType.Unarmored] = Unarmored;
            skills[(int) SkillType.Security] = Security;
            skills[(int) SkillType.Sneak] = Sneak;
            skills[(int) SkillType.Acrobatics] = Acrobatics;
            skills[(int) SkillType.LightArmor] = LightArmor;
            skills[(int) SkillType.ShortBlade] = ShortBlade;
            skills[(int) SkillType.Marksman] = Marksman;
            skills[(int) SkillType.Mercantile] = Mercantile;
            skills[(int) SkillType.Speechcraft] = Speechcraft;
            skills[(int) SkillType.HandToHand] = HandToHand;
        }

        public override string ToString()
        {
            return $"NPC ({Name})";
        }

        static InvalidOperationException AutoCalcStateError(string attributeName)
		{
            return new InvalidOperationException($"Attempt to set value on Auto Calculated field: {attributeName}");
		}

    }


}
