
using System.Collections.Generic;
using System.IO;
using TES3.Core;
using TES3.Records;
using TES3.Records.SubRecords;
using TES3.Util;

namespace TES3.GameItem.Item
{

    [TargetRecord("REGN")]
    public class Region : TES3GameItem
    {

        byte snowChance;
        byte blizzardChance;

        string displayName;
        ColorRef mapColor;

        public Region(string name) : base(name)
        {

        }

        public Region(Record record) : base(record)
        {

        }

        public override string RecordName => "REGN";

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

        public byte ClearChance
        {
            get;
            set;
        }

        public byte CloudyChance
        {
            get;
            set;
        }

        public byte FoggyChance
        {
            get;
            set;
        }

        public byte OvercastChance
        {
            get;
            set;
        }

        public byte RainChance
        {
            get;
            set;
        }

        public byte ThunderChance
        {
            get;
            set;
        }

        public byte AshStormChance
        {
            get;
            set;
        }

        public byte BlightStormChance
        {
            get;
            set;
        }

        public byte SnowChance
        {
            get => snowChance;
            set
            {
                if (!IsExpansion)
                {
                    throw new InvalidDataException("Unable to set snow chance on a non-expansion record; explicitly set IsExpansion to true.");
                }
                snowChance = value;
            }
        }

        public byte BlizzardChance
        {
            get => blizzardChance;
            set
            {
                if (!IsExpansion)
                {
                    throw new InvalidDataException("Unable to set blizzard chance on a non-expansion record; explicitly set IsExpansion to true.");
                }
                blizzardChance = value;
            }
        }

        public string SleepLeveledCreature
        {
            get;
            set;
        }

        public ColorRef MapColor
        {
            get => mapColor;
            set => mapColor = Validation.NotNull(value, "value", "Map Color");
        }

        public bool Deleted
        {
            get;
            set;
        }

        public bool IsExpansion
        {
            get;
            set;
        }

        protected override void OnCreate(IList<SubRecord> subRecords)
        {
            subRecords.Add(new StringSubRecord("NAME", Name));
            subRecords.Add(new StringSubRecord("FNAM", DisplayName));
            
            if (IsExpansion)
            {
                subRecords.Add(new ExpansionWeatherData("WEAT", ClearChance, CloudyChance, FoggyChance, OvercastChance, RainChance, ThunderChance, AshStormChance, BlightStormChance, SnowChance, BlizzardChance));
            }
            else
            {
                subRecords.Add(new WeatherData("WEAT", ClearChance, CloudyChance, FoggyChance, OvercastChance, RainChance, ThunderChance, AshStormChance, BlightStormChance));
            }

            subRecords.Add(new ColorSubRecord("CNAM", MapColor.Copy()));

        }

        protected override void UpdateRequired(Record record)
        {
            record.GetSubRecord<StringSubRecord>("NAME").Data = Name;
            record.GetSubRecord<StringSubRecord>("FNAM").Data = DisplayName;
            record.GetSubRecord<ColorSubRecord>("CNAM").Data = MapColor.Copy();

            var weatherData = record.GetSubRecord<WeatherData>("WEAT");
            if (IsExpansion && !(weatherData is ExpansionWeatherData))
            {
                record[record.GetSubRecordIndex(weatherData)] = new ExpansionWeatherData("WEAT", ClearChance, CloudyChance, FoggyChance, OvercastChance, RainChance, ThunderChance, AshStormChance, BlightStormChance, SnowChance, BlizzardChance);
            }
            else
            {
                weatherData.Clear = ClearChance;
                weatherData.Cloudy = CloudyChance;
                weatherData.Foggy = FoggyChance;
                weatherData.Overcast = OvercastChance;
                weatherData.Rain = RainChance;
                weatherData.Thunder = ThunderChance;
                weatherData.Ash = AshStormChance;
                weatherData.Blight = BlightStormChance;
                if (IsExpansion)
                {
                    var exp = (ExpansionWeatherData) weatherData;
                    exp.Snow = SnowChance;
                    exp.Blizzard = BlizzardChance;
                }
            }


        }

        protected override void UpdateOptional(Record record)
        {
            ProcessOptional(record, "BNAM", SleepLeveledCreature != null, () => new StringSubRecord("BNAM", SleepLeveledCreature), (sr) => sr.Data = SleepLeveledCreature);
            ProcessOptional(record, "DELE", Deleted, () => new IntSubRecord("DELE", 0), (sr) => sr.Data = 0);
        }

        protected override void DoSyncWithRecord(Record record)
        {
            // Required
            Id = record.GetSubRecord<StringSubRecord>("NAME").Data;
            DisplayName = record.GetSubRecord<StringSubRecord>("FNAM").Data;
            MapColor = record.GetSubRecord<ColorSubRecord>("CNAM").Data.Copy();

            var weatherData = record.GetSubRecord<WeatherData>("WEAT");
            ClearChance = weatherData.Clear;
            CloudyChance = weatherData.Cloudy;
            FoggyChance = weatherData.Foggy;
            OvercastChance = weatherData.Overcast;
            RainChance = weatherData.Rain;
            ThunderChance = weatherData.Thunder;
            AshStormChance = weatherData.Ash;
            BlightStormChance = weatherData.Blight;
            if (weatherData is ExpansionWeatherData exp)
            {
                IsExpansion = true;
                SnowChance = exp.Snow;
                BlizzardChance = exp.Blizzard;
            }
            else
            {
                IsExpansion = false;
            }

            // Optional
            SleepLeveledCreature = record.TryGetSubRecord<StringSubRecord>("BNAM")?.Data;
            Deleted = record.ContainsSubRecord("DELE");
        }

        protected override void DoValidateRecord(Record record, Validator validator)
        {
            validator.CheckRequired(record, "NAME");
            validator.CheckRequired(record, "FNAM");
            validator.CheckRequired(record, "WEAT");
            validator.CheckRequired(record, "CNAM");
        }

        public override TES3GameItem Clone()
        {
            var result = new Region(Name)
            {
                DisplayName = DisplayName,
                ClearChance = ClearChance,
                CloudyChance = CloudyChance,
                FoggyChance = FoggyChance,
                OvercastChance = OvercastChance,
                RainChance = RainChance,
                ThunderChance = ThunderChance,
                AshStormChance = AshStormChance,
                BlightStormChance = BlightStormChance,
                IsExpansion = IsExpansion,
                SleepLeveledCreature = SleepLeveledCreature,
                MapColor = MapColor.Copy(),
                Deleted = Deleted
            };

            if (IsExpansion)
            {
                result.SnowChance = SnowChance;
                result.BlizzardChance = BlizzardChance;
            }

            return result;
        }

        public override void StreamDebug(TextWriter target)
        {
            var writer = new IndentWriter(target);

            writer.WriteLine(ToString());

            writer.IncIndent();
            writer.WriteLine($"Name: {DisplayName}");
            writer.WriteLine($"Map Color: {MapColor}");
            writer.WriteLine($"Sleep Leveled Creature: {SleepLeveledCreature}");

            writer.WriteLine("Weather Chances");
            writer.IncIndent();
            writer.WriteLine($"Clear: {ClearChance}");
            writer.WriteLine($"Cloudy: {CloudyChance}");
            writer.WriteLine($"Foggy: {FoggyChance}");
            writer.WriteLine($"Overcast: {OvercastChance}");
            writer.WriteLine($"Rain: {RainChance}");
            writer.WriteLine($"Thunder: {ThunderChance}");
            writer.WriteLine($"Ash Storm: {AshStormChance}");
            writer.WriteLine($"Blight Storm: {BlightStormChance}");
            writer.DecIndent();
            writer.DecIndent();
        }

        public override string ToString()
        {
            return $"Region ({Name})";
        }

    }
}
