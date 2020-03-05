using System;
using System.Collections.Generic;
using TES3.Records;
using TES3.GameItem.Part;
using TES3.GameItem.TypeConstant;
using TES3.Records.SubRecords;
using TES3.Core;
using TES3.Util;

namespace TES3.GameItem.Item
{

    public abstract class Actor : ServicesProvider
    {
        const int WEAPON_FLAG = 0x00001;
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
        const int REPAIR_ITEMS_FLAG = 0x20000;

        const int MAX_TRAVEL_DESTINATIONS = 4;



        public Actor(string name) : base(name)
        {
            
        }

        public Actor(Record record) : base(record)
        {
            
        }

        public bool Essential 
        { 
            get; 
            set; 
        }

        public bool Respawn 
        { 
            get; 
            set; 
        }

        public int BarterGold 
        { 
            get; 
            set; 
        }


        public string DisplayName
        {
            get;
            set;
        }

        public string Animation
        {
            get;
            set;
        }

        public string Script
        {
            get;
            set;
        }

        public BloodType BloodType
        {
            get;
            set;
        }

        public byte Hello
        {
            get;
            set;
        }

        public byte Fight
        {
            get;
            set;
        }

        public byte Flee
        {
            get;
            set;
        }

        public byte Alarm
        {
            get;
            set;
        }

        public bool Deleted
        {
            get;
            set;
        }

        public bool IsPlayer
        {
            get => Name == "player";
        }

        public IList<InventoryItem> InventoryList
        {
            get;
        } = new List<InventoryItem>();

        public IList<string> SpellList
        {
            get;
        } = new List<string>();

        public IList<AIPackage> AIPackageList
        {
            get;
        } = new List<AIPackage>();

        public IList<TravelDestination> TravelDestinations
        {
            get;
        } = new MaxCapacityList<TravelDestination>(MAX_TRAVEL_DESTINATIONS, "Travel Destinations");


        public bool HasPackage(params AIPackageType[] types)
        {
            foreach (var package in AIPackageList)
            {
                if (Array.IndexOf(types, package.Type) != -1)
                {
                    return true;
                }
            }
            return false;
        }

        static readonly string DefaultAIPackageAddTarget = "AI_W";
        static readonly string[] AIPackageSubRecordNames = new string[] { "AI_W", "AI_T", "AI_F", "AI_E", "AI_A" };
        static readonly string[] TravelDestinationSubRecordNames = new string[] { "DODT", "DNAM" };

        protected override void UpdateRequired(Record record)
        {
            // Required
            record.GetSubRecord<StringSubRecord>("NAME").Data = Name;

            var flags = record.GetSubRecord<IntSubRecord>("FLAG");
            // Clear current blood type, but leave other flags.
            flags.Data &= 0x00FF;
            // Set blood type.
            flags.Data |= (int) BloodType;

            if (!IsPlayer)
            {
                var aiData = record.GetSubRecord<AIData>("AIDT");
                aiData.Hello = Hello;
                aiData.Fight = Fight;
                aiData.Flee = Flee;
                aiData.Alarm = Alarm;
                aiData.Flags = CalculateAIDataFlags();
            }
        }

        

        protected override void UpdateOptional(Record record)
        {
            // Optional
            ProcessOptional(record, "FNAM", DisplayName != null, () => new StringSubRecord("FNAM", DisplayName), (sr) => sr.Data = DisplayName);
            ProcessOptional(record, "MODL", Animation != null, () => new StringSubRecord("MODL", Animation), (sr) => sr.Data = Animation);
            ProcessOptional(record, "SCRI", Script != null, () => new StringSubRecord("SCRI", Script), (sr) => sr.Data = Script);
            ProcessOptional(record, "DELE", Deleted, () => new IntSubRecord("DELE", 0), (sr) => sr.Data = 0);

            // Collection
            UpdateCollection(record, InventoryList, "NPCO",
                delegate (ref int index, InventoryItem item)
                {
                    record.InsertSubRecordAt(index++, new InventoryItemSubRecord("NPCO", item.Count, item.Name));
                }
            );

            UpdateCollection(record, SpellList, "NPCS",
                delegate (ref int index, string item)
                {
                    record.InsertSubRecordAt(index++, new StringSubRecord("NPCS", item));
                }
            );

            UpdateCollection(record, AIPackageList, DefaultAIPackageAddTarget, AIPackageSubRecordNames,
                delegate (ref int index, AIPackage item)
                {
                    SubRecord subRecord1, subRecord2 = null;
                    switch (item.Type)
                    {
                        case AIPackageType.Wander:
                            subRecord1 = new AIWanderData("AI_W", item.Distance, item.Distance, item.TimeOfDay,
                                new byte[] {
                                        item.IdleLookingAround,
                                        item.IdleLookingBehind,
                                        item.IdleScratchingHead,
                                        item.IdleReachingForShoulder,
                                        item.IdleRubbingHands,
                                        item.IdleLookingAtHands,
                                        item.IdleDeepThought,
                                        item.IdleReachingForWeapon }, 0);
                            break;
                        case AIPackageType.Travel:
                            subRecord1 = new AITravelData("AI_T", item.X, item.Y, item.Z, 0);
                            break;
                        case AIPackageType.Follow:
                        case AIPackageType.Escort:
                            subRecord1 = new AIFollowEscortData(item.Type == AIPackageType.Follow ? "AI_F" : "AI_E",
                                item.X, item.Y, item.Z, item.Duration, item.ActorID, 0);
                            if (item.CellName != null)
                            {
                                subRecord2 = new StringSubRecord("CNDT", item.CellName);
                            }
                            break;
                        case AIPackageType.Activate:
                            subRecord1 = new AIActivateData("AI_A", item.ObjectID, 0);
                            break;
                        default:
                            throw new InvalidOperationException($"Unexpected AI Package Type: {item.Type}");
                    }

                    record.InsertSubRecordAt(index++, subRecord1);
                    if (subRecord2 != null)
                    {
                        record.InsertSubRecordAt(index++, subRecord2);
                    }
                }
            );

            UpdateCollection(record, TravelDestinations, "DODT", TravelDestinationSubRecordNames,
                delegate (ref int index, TravelDestination item)
                {
                    record.InsertSubRecordAt(index++, new PositionSubRecord("DODT", item.Position.Copy()));
                    if (item.CellName != null)
                    {
                        record.InsertSubRecordAt(index++, new StringSubRecord("DNAM", item.CellName));
                    }
                }
            );

        }

        protected override void DoSyncWithRecord(Record record)
        {
            Id = record.GetSubRecord<StringSubRecord>("NAME").Data;

            var flags = record.GetSubRecord<IntSubRecord>("FLAG").Data & 0xFF00;
            if (!Enum.IsDefined(typeof(BloodType), flags))
            {
                throw new InvalidOperationException($"Unexpected Blood Flag: {flags}");
            }
            BloodType = (BloodType) flags;

            if (!IsPlayer)
            {
                var aiData = record.GetSubRecord<AIData>("AIDT");

                Hello = aiData.Hello;
                Fight = aiData.Fight;
                Flee = aiData.Flee;
                Alarm = aiData.Alarm;
                BuysWeapons = HasFlagSet(aiData.Flags, WEAPON_FLAG);
                BuysArmor = HasFlagSet(aiData.Flags, ARMOR_FLAG);
                BuysClothing = HasFlagSet(aiData.Flags, CLOTHING_FLAG);
                BuysBooks = HasFlagSet(aiData.Flags, BOOKS_FLAG);
                BuysIngredients = HasFlagSet(aiData.Flags, INGREDIENT_FLAG);
                BuysPicks = HasFlagSet(aiData.Flags, PICKS_FLAG);
                BuysProbes = HasFlagSet(aiData.Flags, PROBES_FLAG);
                BuysLights = HasFlagSet(aiData.Flags, LIGHTS_FLAG);
                BuysApparatus = HasFlagSet(aiData.Flags, APPARATUS_FLAG);
                OffersRepair = HasFlagSet(aiData.Flags, REPAIR_FLAG);
                BuysMiscItems = HasFlagSet(aiData.Flags, MISC_FLAG);
                SellsSpells = HasFlagSet(aiData.Flags, SPELLS_FLAG);
                BuysMagicItems = HasFlagSet(aiData.Flags, MAGIC_ITEMS_FLAG);
                BuysPotions = HasFlagSet(aiData.Flags, POTIONS_FLAG);
                OffersTraining = HasFlagSet(aiData.Flags, TRAINING_FLAG);
                OffersSpellmaking = HasFlagSet(aiData.Flags, SPELLMAKING_FLAG);
                OffersEnchanting = HasFlagSet(aiData.Flags, ENCHANTING_FLAG);
                BuysRepairItems = HasFlagSet(aiData.Flags, REPAIR_ITEMS_FLAG);
            }

            // Optional
            DisplayName = record.TryGetSubRecord<StringSubRecord>("FNAM")?.Data;
            Animation = record.TryGetSubRecord<StringSubRecord>("MODL")?.Data;
            Script = record.TryGetSubRecord<StringSubRecord>("SCRI")?.Data;
            Deleted = record.ContainsSubRecord("DELE");

            // Collection
            InventoryList.Clear();
            foreach (InventoryItemSubRecord subRecord in record.GetEnumerableFor("NPCO"))
            {
                InventoryList.Add(new InventoryItem(subRecord.ItemName, subRecord.Count));
            }

            SpellList.Clear();
            foreach (StringSubRecord subRecord in record.GetEnumerableFor("NPCS"))
            {
                SpellList.Add(subRecord.Data);
            }

            AIPackageList.Clear();
            {
                var enumerator = record.GetEnumerableFor("AI_W", "AI_T", "AI_F", "AI_E", "AI_A", "CNDT");
                while (enumerator.MoveNext())
                {
                    var subRecord = enumerator.Current;
                    switch (subRecord.Name)
                    {
                        case "AI_W":
                        {
                            var aiSubRecord = (AIWanderData) subRecord;
                            AIPackageList.Add(new AIWanderPackage(aiSubRecord.Distance, aiSubRecord.Duration, aiSubRecord.TimeOfDay,
                                    aiSubRecord.Idle[0], aiSubRecord.Idle[1], aiSubRecord.Idle[2], aiSubRecord.Idle[3], aiSubRecord.Idle[4],
                                    aiSubRecord.Idle[5], aiSubRecord.Idle[6], aiSubRecord.Idle[7]));
                            break;
                        }
                        case "AI_T":
                        {
                            var aiSubRecord = (AITravelData) subRecord;
                            AIPackageList.Add(new AITravelPackage(aiSubRecord.X, aiSubRecord.Y, aiSubRecord.Z));
                            break;
                        }
                        case "AI_F":
                            AIPackageList.Add(NewFollowEscort(enumerator, AIPackageType.Follow, (AIFollowEscortData) subRecord));
                            break;
                        case "AI_E":
                            AIPackageList.Add(NewFollowEscort(enumerator, AIPackageType.Escort, (AIFollowEscortData) subRecord));
                            break;
                        case "AI_A":
                            AIPackageList.Add(new AIActivatePackage(((AIActivateData) subRecord).TargetName));
                            break;

                    }
                }
            }

            TravelDestinations.Clear();
            {
                var enumerator = record.GetEnumerableFor("DODT", "DNAM");
                while (enumerator.MoveNext())
                {
                    var positionSubRecord = (PositionSubRecord) enumerator.Current;
                    StringSubRecord cellNameSubRecord = null;
                    if (enumerator.HasNext())
                    {
                        var next = enumerator.PeekNext();
                        if (next.Name == "DNAM")
                        {
                            cellNameSubRecord = (StringSubRecord) next;
                            enumerator.MoveNext();
                        }
                    }

                    TravelDestinations.Add(new TravelDestination(positionSubRecord.Data.Copy(), cellNameSubRecord?.Data));
                }
            }


        }

        protected override void DoValidateRecord(Record record, Validator validator)
        {
            validator.CheckRequired(record, "NAME");
            validator.CheckRequired(record, "FLAG");
            validator.CheckRequired(record, "NPDT");
            if (!"player".Equals(record.GetIdentifier()))
            {
                validator.CheckRequired(record, "AIDT");
            }
            validator.CheckCount(record, "DODT", MAX_TRAVEL_DESTINATIONS);
        }

        protected static void ActorFlagSet(IntSubRecord subRecord, bool value, int flag)
        {
            if (value)
            {
                subRecord.Data |= flag;
            }
            else
            {
                subRecord.Data &= (~flag | 0x00ff);
            }
        }

        protected int CalculateAIDataFlags()
        {
            var result = 0;
            if (BuysWeapons)
            {
                result |= WEAPON_FLAG;
            }
            if (BuysArmor)
            {
                result |= ARMOR_FLAG;
            }
            if (BuysClothing)
            {
                result |= CLOTHING_FLAG;
            }
            if (BuysBooks)
            {
                result |= BOOKS_FLAG;
            }
            if (BuysIngredients)
            {
                result |= INGREDIENT_FLAG;
            }
            if (BuysPicks)
            {
                result |= PICKS_FLAG;
            }
            if (BuysProbes)
            {
                result |= PROBES_FLAG;
            }
            if (BuysLights)
            {
                result |= LIGHTS_FLAG;
            }
            if (BuysApparatus)
            {
                result |= APPARATUS_FLAG;
            }
            if (OffersRepair)
            {
                result |= REPAIR_FLAG;
            }
            if (BuysMiscItems)
            {
                result |= MISC_FLAG;
            }
            if (SellsSpells)
            {
                result |= SPELLS_FLAG;
            }
            if (BuysMagicItems)
            {
                result |= MAGIC_ITEMS_FLAG;
            }
            if (BuysPotions)
            {
                result |= POTIONS_FLAG;
            }
            if (OffersTraining)
            {
                result |= TRAINING_FLAG;
            }
            if (OffersSpellmaking)
            {
                result |= SPELLMAKING_FLAG;
            }
            if (OffersEnchanting)
            {
                result |= ENCHANTING_FLAG;
            }
            if (BuysRepairItems)
            {
                result |= REPAIR_ITEMS_FLAG;
            }

            return result;
        }


        protected void CopyClone(Actor clone)
        {
            clone.Essential = Essential;
            clone.Respawn = Respawn;
            clone.BarterGold = BarterGold;
            clone.DisplayName = DisplayName;
            clone.Animation = Animation;
            clone.Script = Script;
            clone.BloodType = BloodType;
            clone.Hello = Hello;
            clone.Fight = Fight;
            clone.Flee = Flee;
            clone.Alarm = Alarm;
            clone.Deleted = Deleted;

            CollectionUtils.Copy(InventoryList, clone.InventoryList);
            CollectionUtils.Copy(SpellList, clone.SpellList);
            CollectionUtils.Copy(AIPackageList, clone.AIPackageList);
            CollectionUtils.Copy(TravelDestinations, clone.TravelDestinations);
        }


        static AIFollowEscortPackage NewFollowEscort(NameEnumerable<SubRecord, Record> enumerator, AIPackageType packageType, AIFollowEscortData subRecord)
        {
            StringSubRecord cellNameSubRecord = null;
            if (enumerator.HasNext())
            {
                var nextSubRecord = enumerator.PeekNext();
                if (nextSubRecord.Name == "CNDT")
                {
                    cellNameSubRecord = (StringSubRecord) nextSubRecord;
                    enumerator.MoveNext();
                }
            }

            return new AIFollowEscortPackage(packageType, subRecord.X, subRecord.Y, subRecord.Z, subRecord.Duration,
                subRecord.Id, cellNameSubRecord?.Data);
        }

    }
}
