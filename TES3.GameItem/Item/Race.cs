
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

    [TargetRecord("RACE")]
    public class Race : TES3GameItem
    {
        const int PLAYABLE_FLAG = 0X01;
        const int BEAST_RACE_FLAG = 0X02;

        string displayName;

        public Race(string name) : base(name)
        {
            
        }

        public Race(Record record) : base(record)
        {
            
        }

        public override string RecordName => "RACE";
        
        [IdField]
        public string Name
        {
            get => (string) Id;
            set => Id = value;
        }

        public string DisplayName
        {
            get => displayName;
            set => displayName = Validation.NotNull(value, "value", "Display Name");
        }

        public bool Playable
        {
            get;
            set;
        }

        public bool BeastRace
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

        public RaceSexAttributes MaleAttributes
        {
            get;
        } = new RaceSexAttributes();
        public RaceSexAttributes FemaleAttributes
        {
            get;
        } = new RaceSexAttributes();

        public IList<RaceSkillBonus> SkillBonuses
        {
            get;
        } = new MaxCapacityList<RaceSkillBonus>(Constants.RACE_MAX_SKILL_BONUSES, "Bonuses");

        public IList<string> Specials
        {
            get;
        } = new List<string>();

        protected override void OnCreate(IList<SubRecord> subRecords)
        {
            subRecords.Add(new StringSubRecord("NAME", Name));
            subRecords.Add(new StringSubRecord("FNAM", DisplayName));
            


            var skillBonuses = new SkillBonus[Constants.RACE_MAX_SKILL_BONUSES];
            for (var i = 0; i < Constants.RACE_MAX_SKILL_BONUSES; ++i)
            {
                var rawBonus = skillBonuses[i];
                if (i < SkillBonuses.Count)
                {
                    var bonus = SkillBonuses[i];

                    rawBonus.SkillId = (int) bonus.Skill;
                    rawBonus.Bonus = bonus.Bonus;
                }
                else
                {
                    rawBonus.SkillId = -1;
                    rawBonus.Bonus = 0;
                }
            }
            var flags = 0;
            if (Playable)
            {
                flags |= PLAYABLE_FLAG;
            }
            if (BeastRace)
            {
                flags |= BEAST_RACE_FLAG;
            }
            subRecords.Add(new RaceData("RADT", skillBonuses,
                new int[] { MaleAttributes.Strength, FemaleAttributes.Strength },
                new int[] { MaleAttributes.Intelligence, FemaleAttributes.Intelligence },
                new int[] { MaleAttributes.Willpower, FemaleAttributes.Willpower },
                new int[] { MaleAttributes.Agility, FemaleAttributes.Agility },
                new int[] { MaleAttributes.Speed, FemaleAttributes.Speed },
                new int[] { MaleAttributes.Endurance, FemaleAttributes.Endurance },
                new int[] { MaleAttributes.Personality, FemaleAttributes.Personality },
                new int[] { MaleAttributes.Luck, FemaleAttributes.Luck },
                new float[] { MaleAttributes.Height, FemaleAttributes.Height },
                new float[] { MaleAttributes.Weight, FemaleAttributes.Weight },
                flags
            ));

        }

        protected override void UpdateRequired(Record record)
        {
            record.GetSubRecord<StringSubRecord>("NAME").Data = Name;
            record.GetSubRecord<StringSubRecord>("FNAM").Data = DisplayName;
            

            var raceData = record.GetSubRecord<RaceData>("RADT");
            FlagSet(raceData, Playable, PLAYABLE_FLAG, RaceDataSet, RaceDataClear);
            FlagSet(raceData, BeastRace, BEAST_RACE_FLAG, RaceDataSet, RaceDataClear);
            

            raceData.Strength[0] = MaleAttributes.Strength;
            raceData.Intelligence[0] = MaleAttributes.Intelligence;
            raceData.Willpower[0] = MaleAttributes.Willpower;
            raceData.Agility[0] = MaleAttributes.Agility;
            raceData.Speed[0] = MaleAttributes.Speed;
            raceData.Endurance[0] = MaleAttributes.Endurance;
            raceData.Personality[0] = MaleAttributes.Personality;
            raceData.Luck[0] = MaleAttributes.Luck;
            raceData.Height[0] = MaleAttributes.Height;
            raceData.Weight[0] = MaleAttributes.Weight;

            raceData.Strength[1] = FemaleAttributes.Strength;
            raceData.Intelligence[1] = FemaleAttributes.Intelligence;
            raceData.Willpower[1] = FemaleAttributes.Willpower;
            raceData.Agility[1] = FemaleAttributes.Agility;
            raceData.Speed[1] = FemaleAttributes.Speed;
            raceData.Endurance[1] = FemaleAttributes.Endurance;
            raceData.Personality[1] = FemaleAttributes.Personality;
            raceData.Luck[1] = FemaleAttributes.Luck;
            raceData.Height[1] = FemaleAttributes.Height;
            raceData.Weight[1] = FemaleAttributes.Weight;

            for (var i = 0; i < Constants.RACE_MAX_SKILL_BONUSES; ++i)
            {
                var rawBonus = raceData.SkillBonuses[i];
                if (i < SkillBonuses.Count)
                {
                    var bonus = SkillBonuses[i];

                    rawBonus.SkillId = (int) bonus.Skill;
                    rawBonus.Bonus = bonus.Bonus;
                }
                else
                {
                    rawBonus.SkillId = -1;
                    rawBonus.Bonus = 0;
                }
            }
        }

        protected override void UpdateOptional(Record record)
        {
            ProcessOptional(record, "DESC", Description != null, () => new StringSubRecord("DATA", Description), (sr) => sr.Data = Description);
            ProcessOptional(record, "DELE", Deleted, () => new IntSubRecord("DELE", 0), (sr) => sr.Data = 0);

            UpdateCollection(record, Specials, "NPCS",
                delegate (ref int index, string item)
                {
                    record.InsertSubRecordAt(index++, new StringSubRecord("NPCS", item));
                }
            );


        }

        protected override void DoSyncWithRecord(Record record)
        {
            // Required
            Id = record.GetSubRecord<StringSubRecord>("NAME").Data;
            DisplayName = record.GetSubRecord<StringSubRecord>("FNAM").Data;
            

            var raceData = record.GetSubRecord<RaceData>("RADT");
            Playable = HasFlagSet(raceData.Flags, PLAYABLE_FLAG);
            BeastRace = HasFlagSet(raceData.Flags, BEAST_RACE_FLAG);

            Description = record.TryGetSubRecord<StringSubRecord>("DESC")?.Data;

            MaleAttributes.Strength = raceData.Strength[0];
            MaleAttributes.Intelligence = raceData.Intelligence[0];
            MaleAttributes.Willpower = raceData.Willpower[0];
            MaleAttributes.Agility = raceData.Agility[0];
            MaleAttributes.Speed = raceData.Speed[0];
            MaleAttributes.Endurance = raceData.Endurance[0];
            MaleAttributes.Personality = raceData.Personality[0];
            MaleAttributes.Luck = raceData.Luck[0];
            MaleAttributes.Height = raceData.Height[0];
            MaleAttributes.Weight = raceData.Weight[0];

            FemaleAttributes.Strength = raceData.Strength[1];
            FemaleAttributes.Intelligence = raceData.Intelligence[1];
            FemaleAttributes.Willpower = raceData.Willpower[1];
            FemaleAttributes.Agility = raceData.Agility[1];
            FemaleAttributes.Speed = raceData.Speed[1];
            FemaleAttributes.Endurance = raceData.Endurance[1];
            FemaleAttributes.Personality = raceData.Personality[1];
            FemaleAttributes.Luck = raceData.Luck[1];
            FemaleAttributes.Height = raceData.Height[1];
            FemaleAttributes.Weight = raceData.Weight[1];

            SkillBonuses.Clear();
            for (var i = 0; i < Constants.RACE_MAX_SKILL_BONUSES; ++i)
            {
                var bonus = raceData.SkillBonuses[i];
                if (bonus.SkillId != -1)
                {
                    SkillBonuses.Add(new RaceSkillBonus((SkillType) bonus.SkillId, bonus.Bonus));
                }
                else
                {
                    break;
                }
            }

            // Optional
            Deleted = record.ContainsSubRecord("DELE");

            // Collection
            Specials.Clear();
            foreach (StringSubRecord subRecord in record.GetEnumerableFor("NPCS"))
            {
                Specials.Add(subRecord.Data);
            }



        }

        protected override void DoValidateRecord(Record record, Validator validator)
        {
            validator.CheckRequired(record, "NAME");
            validator.CheckRequired(record, "FNAM");
            validator.CheckRequired(record, "RADT");
        }

        public override TES3GameItem Copy()
        {
            var result = new Race(Name)
            {
                DisplayName = DisplayName,
                Playable = Playable,
                BeastRace = BeastRace,
                Description = Description,
                Deleted = Deleted
            };

            result.MaleAttributes.SetProperties(MaleAttributes);
            result.FemaleAttributes.SetProperties(FemaleAttributes);
            CollectionUtils.Copy(SkillBonuses, result.SkillBonuses);
            CollectionUtils.Copy(Specials, result.Specials);

            return result;
        }


        public override void StreamDebug(TextWriter target)
        {
            var writer = new IndentWriter(target);

            writer.WriteLine(ToString());

            writer.IncIndent();
            writer.WriteLine($"Name: {DisplayName}");
            writer.WriteLine($"Playable: {Playable}");
            writer.WriteLine($"Beast Race: {BeastRace}");
            writer.WriteLine($"Description: {Description}");

            writer.WriteLine("Male Attributes");
            writer.IncIndent();
            {
                var table = new Table("Attribute", "Value");
                table.AddRow("Strength", MaleAttributes.Strength);
                table.AddRow("Intelligence", MaleAttributes.Intelligence);
                table.AddRow("Willpower", MaleAttributes.Willpower);
                table.AddRow("Agility", MaleAttributes.Agility);
                table.AddRow("Speed", MaleAttributes.Speed);
                table.AddRow("Endurance", MaleAttributes.Endurance);
                table.AddRow("Personality", MaleAttributes.Personality);
                table.AddRow("Luck", MaleAttributes.Luck);
                table.AddRow("Height", MaleAttributes.Height);
                table.AddRow("Weight", MaleAttributes.Weight);
                table.Print(writer);
            }
            writer.DecIndent();

            writer.WriteLine("Female Attributes");
            writer.IncIndent();
            {
                var table = new Table("Attribute", "Value");
                table.AddRow("Strength", FemaleAttributes.Strength);
                table.AddRow("Intelligence", FemaleAttributes.Intelligence);
                table.AddRow("Willpower", FemaleAttributes.Willpower);
                table.AddRow("Agility", FemaleAttributes.Agility);
                table.AddRow("Speed", FemaleAttributes.Speed);
                table.AddRow("Endurance", FemaleAttributes.Endurance);
                table.AddRow("Personality", FemaleAttributes.Personality);
                table.AddRow("Luck", FemaleAttributes.Luck);
                table.AddRow("Height", FemaleAttributes.Height);
                table.AddRow("Weight", FemaleAttributes.Weight);
                table.Print(writer);
            }
            writer.DecIndent();

            writer.WriteLine("Skill Bonuses");
            writer.IncIndent();
            {
                var table = new Table("Skill", "Bonus");
                foreach (var bonus in SkillBonuses)
                {
                    var skill = bonus.Skill;
                    if (skill == SkillType.None)
                    {
                        continue;
                    }

                    table.AddRow(skill, bonus.Bonus);
                }
            }
            writer.DecIndent();

            writer.WriteLine("Specials");
            writer.IncIndent();
            foreach (var special in Specials)
            {
                writer.WriteLine(special);
            }
            writer.DecIndent();

            writer.DecIndent();
        }

        public override string ToString()
        {
            return $"Race ({Name})";
        }


        static void RaceDataSet(RaceData subRecord, int value)
        {
            subRecord.Flags |= value;
        }
        static void RaceDataClear(RaceData subRecord, int value)
        {
            subRecord.Flags &= ~value;
        }

    }

}
