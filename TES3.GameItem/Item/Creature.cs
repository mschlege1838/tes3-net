
using System.Collections.Generic;
using System.IO;
using TES3.GameItem.Part;
using TES3.GameItem.TypeConstant;
using TES3.Records;
using TES3.Records.SubRecords;
using TES3.Util;

namespace TES3.GameItem.Item
{

    [TargetRecord("CREA")]
    public class Creature : Actor
    {
        const int BIPED_FLAG = 0x0001;
        const int RESPAWN_FLAG = 0x0002;
        const int WEAPON_SHIELD_FLAG = 0x0004;
        const int SWIMS_FLAG = 0x0010;
        const int FLIES_FLAG = 0x0020;
        const int WALKS_FLAG = 0x0040;
        const int NONE_FLAG = 0x0008;
        const int ESSENTIAL_FLAG = 0x0080;


        bool swims;
        bool walks;
        bool flies;
        bool biped;
        bool none;

        int level;

        public CreatureDamageRange Attack1 { get; } = new CreatureDamageRange();
        public CreatureDamageRange Attack2 { get; } = new CreatureDamageRange();
        public CreatureDamageRange Attack3 { get; } = new CreatureDamageRange();

        public Creature(string name) : base(name)
        {

        }

        public Creature(Record record) : base(record)
        {
            
        }

        public override string RecordName => "CREA";

        public CreatureType Type
        {
            get;
            set;
        }

        public int Level
        {
            get => level;
            set => level = Validation.Gte(value, 0, "value", "Level");
        }

        public int Strength
        {
            get;
            set;
        }

        public int Intelligence
        {
            get;
            set;
        }

        public int Willpower
        {
            get;
            set;
        }

        public int Agility
        {
            get;
            set;
        }

        public int Speed
        {
            get;
            set;
        }

        public int Endurance
        {
            get;
            set;
        }

        public int Personality
        {
            get;
            set;
        }

        public int Luck
        {
            get;
            set;
        }

        public int Health
        {
            get;
            set;
        }

        public int Magicka
        {
            get;
            set;
        }

        public int Fatigue
        {
            get;
            set;
        }

        public int Soul
        {
            get;
            set;
        }

        public int Combat
        {
            get;
            set;
        }

        public int Magic
        {
            get;
            set;
        }

        public int Stealth
        {
            get;
            set;
        }

        public bool WeaponAndShield
        {
            get;
            set;
        }

        public string SoundGenCreatureName
        {
            get;
            set;
        }

        public float Scale
        {
            get;
            set;
        }

        public bool Swims
        {
            get => swims;
            set
            {
                if (value)
                {
                    Biped = None = false;
                }
                swims = value;
            }
        }

        public bool Walks
        {
            get => walks;
            set
            {
                if (value)
                {
                    Biped = None = false;
                }
                walks = value;
            }
        }

        public bool Flies
        {
            get => flies;
            set
            {
                if (value)
                {
                    Biped = None = false;
                }
                flies = value;
            }
        }

        public bool Biped
        {
            get => biped;
            set
            {
                if (value)
                {
                    Swims = Walks = Flies = None = false;
                }
                biped = value;
            }
        }

        public bool None
        {
            get => none;
            set
            {
                if (value)
                {
                    Swims = Walks = Flies = Biped = false;
                }
                none = value;
            }
        }

        protected override void OnCreate(IList<SubRecord> subRecords)
        {
            subRecords.Add(new StringSubRecord("NAME", Name));
            subRecords.Add(new StringSubRecord("MODL", Animation));
            subRecords.Add(new CreatureData("NPDT", (int) Type, Level, Strength, Intelligence, Willpower, Agility, Speed, Endurance, Personality, Luck, Health, Magicka, Fatigue, Soul, Combat, Magic, Stealth,
                Attack1.Min, Attack1.Max, Attack2.Min, Attack2.Max, Attack3.Min, Attack3.Max, BarterGold));

            var flags = 0;
            if (Essential)
            {
                flags |= ESSENTIAL_FLAG;
            }
            if (Respawn)
            {
                flags |= RESPAWN_FLAG;
            }
            if (WeaponAndShield)
            {
                flags |= WEAPON_SHIELD_FLAG;
            }
            if (Swims)
            {
                flags |= SWIMS_FLAG;
            }
            if (Walks)
            {
                flags |= WALKS_FLAG;
            }
            if (Flies)
            {
                flags |= FLIES_FLAG;
            }
            if (Biped)
            {
                flags |= BIPED_FLAG;
            }
            if (None)
            {
                flags |= NONE_FLAG;
            }
            subRecords.Add(new IntSubRecord("FLAG", flags));

            subRecords.Add(new AIData("AIDT", Hello, 0, Fight, Flee, Alarm, 0, 0, 0, CalculateAIDataFlags()));
        }

        protected override void UpdateRequired(Record record)
        {
            base.UpdateRequired(record);

            var creatureData = record.GetSubRecord<CreatureData>("NPDT");
            creatureData.Type = (int) Type;
            creatureData.Level = Level;
            creatureData.Strength = Strength;
            creatureData.Intelligence = Intelligence;
            creatureData.Willpower = Willpower;
            creatureData.Agility = Agility;
            creatureData.Speed = Speed;
            creatureData.Endurance = Endurance;
            creatureData.Personality = Personality;
            creatureData.Luck = Luck;
            creatureData.Health = Health;
            creatureData.SpellPoints = Magicka;
            creatureData.Fatigue = Fatigue;
            creatureData.Soul = Soul;
            creatureData.Combat = Combat;
            creatureData.Magic = Magic;
            creatureData.Stealth = Stealth;
            creatureData.Gold = BarterGold;

            creatureData.AttackMin1 = Attack1.Min;
            creatureData.AttackMax1 = Attack1.Max;
            creatureData.AttackMin2 = Attack2.Min;
            creatureData.AttackMax2 = Attack2.Max;
            creatureData.AttackMin3 = Attack3.Min;
            creatureData.AttackMax3 = Attack3.Max;



            var flags = record.GetSubRecord<IntSubRecord>("FLAG");
            ActorFlagSet(flags, Essential, ESSENTIAL_FLAG);
            ActorFlagSet(flags, Respawn, RESPAWN_FLAG);
            ActorFlagSet(flags, WeaponAndShield, WEAPON_SHIELD_FLAG);
            ActorFlagSet(flags, Swims, SWIMS_FLAG);
            ActorFlagSet(flags, Walks, WALKS_FLAG);
            ActorFlagSet(flags, Flies, FLIES_FLAG);
            ActorFlagSet(flags, Biped, BIPED_FLAG);
            ActorFlagSet(flags, None, NONE_FLAG);

        }

        protected override void UpdateOptional(Record record)
        {
            base.UpdateOptional(record);
            ProcessOptional(record, "CNAM", SoundGenCreatureName != null, () => new StringSubRecord("CNAM", SoundGenCreatureName), (sr) => sr.Data = SoundGenCreatureName);
            ProcessOptional(record, "XSCL", Scale != 1.0f, () => new FloatSubRecord("XSCL", Scale), (sr) => sr.Data = Scale);

        }

        protected override void DoSyncWithRecord(Record record)
        {
            base.DoSyncWithRecord(record);

            var creatureData = record.GetSubRecord<CreatureData>("NPDT");
            Type = (CreatureType) creatureData.Type;
            Level = creatureData.Level;
            Strength = creatureData.Strength;
            Intelligence = creatureData.Intelligence;
            Willpower = creatureData.Willpower;
            Agility = creatureData.Agility;
            Speed = creatureData.Speed;
            Endurance = creatureData.Endurance;
            Personality = creatureData.Personality;
            Luck = creatureData.Luck;
            Health = creatureData.Health;
            Magicka = creatureData.SpellPoints;
            Fatigue = creatureData.Fatigue;
            Soul = creatureData.Soul;
            Combat = creatureData.Combat;
            Magic = creatureData.Magic;
            Stealth = creatureData.Stealth;
            BarterGold = creatureData.Gold;

            Attack1.SetRange(creatureData.AttackMin1, creatureData.AttackMax1);
            Attack2.SetRange(creatureData.AttackMin2, creatureData.AttackMax2);
            Attack3.SetRange(creatureData.AttackMin3, creatureData.AttackMax3);

            var flags = record.GetSubRecord<IntSubRecord>("FLAG");
            Essential = HasFlagSet(flags.Data, ESSENTIAL_FLAG);
            Respawn = HasFlagSet(flags.Data, RESPAWN_FLAG);
            WeaponAndShield = HasFlagSet(flags.Data, WEAPON_SHIELD_FLAG);
            Swims = HasFlagSet(flags.Data, SWIMS_FLAG);
            Walks = HasFlagSet(flags.Data, WALKS_FLAG);
            Flies = HasFlagSet(flags.Data, FLIES_FLAG);
            Biped = HasFlagSet(flags.Data, BIPED_FLAG);
            None = HasFlagSet(flags.Data, NONE_FLAG);

            SoundGenCreatureName = record.TryGetSubRecord<StringSubRecord>("CNAM")?.Data;
            Scale = record.ContainsSubRecord("XSCL") ? record.GetSubRecord<FloatSubRecord>("XSCL").Data : 1.0f;

        }

        protected override void DoValidateRecord(Record record, Validator validator)
        {
            base.DoValidateRecord(record, validator);
            validator.CheckRequired(record, "MODL");
        }

        public override TES3GameItem Clone()
        {
            var result = new Creature(Name)
            {
                Type = Type,
                Level = Level,
                Strength = Strength,
                Intelligence = Intelligence,
                Willpower = Willpower,
                Agility = Agility,
                Speed = Speed,
                Endurance = Endurance,
                Personality = Personality,
                Luck = Luck,
                Health = Health,
                Magicka = Magicka,
                Fatigue = Fatigue,
                Soul = Soul,
                Combat = Combat,
                Magic = Magic,
                Stealth = Stealth,
                WeaponAndShield = WeaponAndShield,
                SoundGenCreatureName = SoundGenCreatureName,
                Scale = Scale,
                swims = swims,
                walks = walks,
                flies = flies,
                biped = biped,
                none = none
            };
            result.Attack1.SetRange(Attack1);
            result.Attack2.SetRange(Attack2);
            result.Attack3.SetRange(Attack3);
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
            writer.WriteLine($"Level: {Level}");
            writer.WriteLine($"Essential: {Essential}");
            writer.WriteLine($"Respawn: {Respawn}");
            writer.WriteLine($"Script: {Script}");


            writer.WriteLine("Stats");
            writer.IncIndent();
            writer.WriteLine($"Health: {Health}");
            writer.WriteLine($"Magicka: {Magicka}");
            writer.WriteLine($"Fatigue: {Fatigue}");
            writer.DecIndent();

            writer.WriteLine("Attack");
            writer.IncIndent();
            {
                var table = new Table("", "Min", "Max");
                table.AddRow("Attack1", Attack1.Min, Attack1.Max);
                table.AddRow("Attack2", Attack2.Min, Attack2.Max);
                table.AddRow("Attack3", Attack3.Min, Attack3.Max);
                table.Print(writer);
            }
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
            writer.WriteLine($"Combat: {Combat}");
            writer.WriteLine($"Magic: {Magic}");
            writer.WriteLine($"Stealth: {Stealth}");
            writer.DecIndent();

            if (InventoryList.Count > 0)
            {
                writer.WriteLine("Inventory");
                writer.IncIndent();
                var table = new Table("Name", "Count");
                foreach (var item in InventoryList)
                {
                    table.AddRow(item.Name, item.Count);
                }
                table.Print(writer);
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

            writer.WriteLine($"Blood: {BloodType}");
            writer.WriteLine($"Animation: {Animation}");
            writer.WriteLine($"Scale: {Scale}");

            writer.WriteLine("Movement");
            writer.IncIndent();
            writer.WriteLine($"Flies: {Flies}");
            writer.WriteLine($"Swims: {Swims}");
            writer.WriteLine($"Walks: {Walks}");
            writer.WriteLine($"Biped: {Biped}");
            writer.WriteLine($"None: {None}");
            writer.DecIndent();

            writer.WriteLine("Misc");
            writer.IncIndent();
            writer.WriteLine($"Weapon & Shield: {WeaponAndShield}");
            writer.WriteLine($"Soul: {Soul}");
            writer.DecIndent();

            writer.DecIndent();
        }

        public override string ToString()
        {
            return $"Creature ({Name})";
        }


    }
}
