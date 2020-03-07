
using System.Collections.Generic;
using System.IO;
using TES3.GameItem.TypeConstant;
using TES3.Records;
using TES3.Records.SubRecords;
using TES3.Util;

namespace TES3.GameItem.Item
{

    [TargetRecord("MGEF")]
    public class MagicEffect : TES3GameItem
    {
        const int SPELLMAKING_FLAG = 0x0200;
        const int ENCHANTING_FLAG = 0x0400;
        const int NEGATIVE_FLAG = 0x0800;

        public MagicEffect(MagicEffectType type) : base(type)
        {
            
        }

        public MagicEffect(Record record) : base(record)
        {
            
        }

        public override string RecordName => "MGEF";

        [IdField]
        public MagicEffectType Type
        {
            get => (MagicEffectType) Id;
            set => Id = value;
        }

        public SpellSchoolType School
        {
            get;
            set;
        }

        public float BaseCost
        {
            get;
            set;
        }

        public bool Spellmaking
        {
            get;
            set;
        }

        public bool Enchanting
        {
            get;
            set;
        }

        public bool Negative
        {
            get;
            set;
        }

        public int Red
        {
            get;
            set;
        }

        public int Blue
        {
            get;
            set;
        }

        public int Green
        {
            get;
            set;
        }

        public float SpeedX
        {
            get;
            set;
        }

        public float SizeX
        {
            get;
            set;
        }

        public float SizeCap
        {
            get;
            set;
        }

        public string Icon
        {
            get;
            set;
        }

        public string ParticleTexture
        {
            get;
            set;
        }

        public string CastVisual
        {
            get;
            set;
        }

        public string BoltVisual
        {
            get;
            set;
        }

        public string HitVisual
        {
            get;
            set;
        }

        public string AreaVisual
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public string CastSound
        {
            get;
            set;
        }

        public string BoltSound
        {
            get;
            set;
        }

        public string HitSound
        {
            get;
            set;
        }

        public string AreaSound
        {
            get;
            set;
        }

        protected override void OnCreate(IList<SubRecord> subRecords)
        {
            subRecords.Add(new IntSubRecord("INDX", (int) Type));

            var flags = 0;
            if (Spellmaking)
            {
                flags |= SPELLMAKING_FLAG;
            }
            if (Enchanting)
            {
                flags |= ENCHANTING_FLAG;
            }
            if (Negative)
            {
                flags |= NEGATIVE_FLAG;
            }
            subRecords.Add(new MagicEffectData("MEDT", (int) School, BaseCost, flags, Red, Blue, Green, SpeedX, SizeX, SizeCap));
        }

        protected override void UpdateRequired(Record record)
        {
            var magicEffectData = record.GetSubRecord<MagicEffectData>("MEDT");
            magicEffectData.SpellSchool = (int) School;
            magicEffectData.BaseCost = BaseCost;
            magicEffectData.Red = Red;
            magicEffectData.Blue = Blue;
            magicEffectData.Green = Green;
            magicEffectData.SpeedX = SpeedX;
            magicEffectData.SizeX = SizeX;
            magicEffectData.SpeedX = SizeCap;
            FlagSet(magicEffectData, Spellmaking, SPELLMAKING_FLAG, MagicEffectFlagSet, MagicEffectFlagClear);
            FlagSet(magicEffectData, Enchanting, ENCHANTING_FLAG, MagicEffectFlagSet, MagicEffectFlagClear);
            FlagSet(magicEffectData, Negative, NEGATIVE_FLAG, MagicEffectFlagSet, MagicEffectFlagClear);
        }

        protected override void UpdateOptional(Record record)
        {
            ProcessOptional(record, "ITEX", Icon != null, () => new StringSubRecord("ITEX", Icon), (sr) => sr.Data = Icon);
            ProcessOptional(record, "PTEX", ParticleTexture != null, () => new StringSubRecord("PTEX", ParticleTexture), (sr) => sr.Data = ParticleTexture);
            ProcessOptional(record, "CVFX", CastVisual != null, () => new StringSubRecord("CVFX", CastVisual), (sr) => sr.Data = CastVisual);
            ProcessOptional(record, "BVFX", BoltVisual != null, () => new StringSubRecord("BVFX", BoltVisual), (sr) => sr.Data = BoltVisual);
            ProcessOptional(record, "HVFX", HitVisual != null, () => new StringSubRecord("HVFX", HitVisual), (sr) => sr.Data = HitVisual);
            ProcessOptional(record, "AVFX", AreaVisual != null, () => new StringSubRecord("AVFX", AreaVisual), (sr) => sr.Data = AreaVisual);
            ProcessOptional(record, "DESC", Description != null, () => new StringSubRecord("DESC", Description), (sr) => sr.Data = Description);
            ProcessOptional(record, "CSND", CastSound != null, () => new StringSubRecord("CSND", CastSound), (sr) => sr.Data = CastSound);
            ProcessOptional(record, "BSND", BoltSound != null, () => new StringSubRecord("BSND", BoltSound), (sr) => sr.Data = BoltSound);
            ProcessOptional(record, "HSND", HitSound != null, () => new StringSubRecord("HSND", HitSound), (sr) => sr.Data = HitSound);
            ProcessOptional(record, "ASND", AreaSound != null, () => new StringSubRecord("ASND", AreaSound), (sr) => sr.Data = AreaSound);
        }

        protected override void DoSyncWithRecord(Record record)
        {
            // Required
            Icon = record.TryGetSubRecord<StringSubRecord>("ITEX")?.Data;
            ParticleTexture = record.TryGetSubRecord<StringSubRecord>("PTEX")?.Data;
            Id = (MagicEffectType) record.GetSubRecord<IntSubRecord>("INDX").Data;

            var magicEffectData = record.GetSubRecord<MagicEffectData>("MEDT");
            School = (SpellSchoolType) magicEffectData.SpellSchool;
            BaseCost = magicEffectData.BaseCost;
            Spellmaking = HasFlagSet(magicEffectData.Flags, SPELLMAKING_FLAG);
            Enchanting = HasFlagSet(magicEffectData.Flags, ENCHANTING_FLAG);
            Negative = HasFlagSet(magicEffectData.Flags, NEGATIVE_FLAG);
            Red = magicEffectData.Red;
            Blue = magicEffectData.Blue;
            Green = magicEffectData.Green;
            SpeedX = magicEffectData.SpeedX;
            SizeX = magicEffectData.SizeX;
            SizeCap = magicEffectData.SpeedX;

            // Optional
            CastVisual = record.TryGetSubRecord<StringSubRecord>("CVFX")?.Data;
            BoltVisual = record.TryGetSubRecord<StringSubRecord>("BVFX")?.Data;
            HitVisual = record.TryGetSubRecord<StringSubRecord>("HVFX")?.Data;
            AreaVisual = record.TryGetSubRecord<StringSubRecord>("AVFX")?.Data;
            Description = record.TryGetSubRecord<StringSubRecord>("DESC")?.Data;
            CastSound = record.TryGetSubRecord<StringSubRecord>("CSND")?.Data;
            BoltSound = record.TryGetSubRecord<StringSubRecord>("BSND")?.Data;
            HitSound = record.TryGetSubRecord<StringSubRecord>("HSND")?.Data;
            AreaSound = record.TryGetSubRecord<StringSubRecord>("ASND")?.Data;
        }

        protected override void DoValidateRecord(Record record, Validator validator)
        {
            validator.CheckRequired(record, "INDX");
            validator.CheckRequired(record, "MEDT");
        }

        public override TES3GameItem Copy()
        {
            return new MagicEffect(Type)
            {
                School = School,
                BaseCost = BaseCost,
                Spellmaking = Spellmaking,
                Enchanting = Enchanting,
                Negative = Negative,
                Red = Red,
                Blue = Blue,
                Green = Green,
                SpeedX = SpeedX,
                SizeX = SizeX,
                SizeCap = SizeCap,
                Icon = Icon,
                ParticleTexture = ParticleTexture,
                CastVisual = CastVisual,
                BoltVisual = BoltVisual,
                HitVisual = HitVisual,
                AreaVisual = AreaVisual,
                Description = Description,
                CastSound = CastSound,
                BoltSound = BoltSound,
                HitSound = HitSound,
                AreaSound = AreaSound
            };
        }

        public override void StreamDebug(TextWriter target)
        {
            var writer = new IndentWriter(target);

            writer.WriteLine(ToString());

            writer.IncIndent();
            writer.WriteLine($"School: {School}");
            writer.WriteLine($"Base Cost: {BaseCost}");
            writer.WriteLine($"Particle Texture: {ParticleTexture}");
            writer.WriteLine($"Effect Icon: {Icon}");
            writer.WriteLine($"Description: {Description}");
            writer.WriteLine($"Spellmaking: {Spellmaking}");
            writer.WriteLine($"Enchanting: {Enchanting}");

            writer.WriteLine("Visuals/Sounds");
            writer.IncIndent();
            writer.WriteLine($"Cast Sound: {CastSound}");
            writer.WriteLine($"Cast Visual: {CastVisual}");
            writer.WriteLine($"Bolt Sound: {BoltSound}");
            writer.WriteLine($"Bolt Visual: {BoltVisual}");
            writer.WriteLine($"Hit Sound: {HitSound}");
            writer.WriteLine($"Hit Visual: {HitVisual}");
            writer.WriteLine($"Area Sound: {AreaSound}");
            writer.WriteLine($"Area Visual: {AreaVisual}");
            writer.WriteLine($"SpeedX: {SpeedX}");
            writer.WriteLine($"SizeX: {SizeX}");
            writer.WriteLine($"Size Cap: {SizeCap}");
            writer.DecIndent();

            writer.WriteLine("Lighting");
            writer.IncIndent();
            writer.WriteLine($"Red: {Red}");
            writer.WriteLine($"Blue: {Blue}");
            writer.WriteLine($"Green: {Green}");
            writer.WriteLine($"Negative: {Negative}");
            writer.DecIndent();
            writer.DecIndent();

        }

        public override string ToString()
        {
            return $"Magic Effect ({Type})";
        }

        private static void MagicEffectFlagSet(MagicEffectData subRecord, int flag)
        {
            subRecord.Flags |= flag;
        }

        private static void MagicEffectFlagClear(MagicEffectData subRecord, int flag)
        {
            subRecord.Flags &= ~flag;
        }


    }


}
