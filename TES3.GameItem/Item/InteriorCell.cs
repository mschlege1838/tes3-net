
using System.Collections.Generic;

using TES3.Core;
using TES3.Records;
using TES3.Records.SubRecords;
using TES3.Util;

namespace TES3.GameItem.Item
{
    public class InteriorCell : Cell
    {

        public InteriorCell(string name) : base(name)
        {

        }

        public InteriorCell(Record record) : base(record)
        {

        }

        public override bool IsInterior => true;

        [IdField]
        public string Name
        {
            get => (string) Id;
            set => Id = value;
        }

        public override string DisplayName
        {
            get => Name;
        }

        public bool HasWater
        {
            get;
            set;
        }

        public bool LikeExterior
        {
            get;
            set;
        }

        public int WaterHeight
        {
            get;
            set;
        }

        public ColorRef AmbientColor
        {
            get;
            set;
        }

        public ColorRef SunlightColor
        {
            get;
            set;
        }

        public ColorRef FogColor
        {
            get;
            set;
        }

        public float FogDensity
        {
            get;
            set;
        }

        protected override void OnCreate(IList<SubRecord> subRecords)
        {
            base.OnCreate(subRecords);

            subRecords.Add(new StringSubRecord("NAME", Name));

            var flags = Constants.Cell.INTERIOR_FLAG;
            if (HasWater)
            {
                flags |= HAS_WATER_FLAG;
            }
            if (SleepingIllegal)
            {
                flags |= SLEEPING_ILLEGAL_FLAG;
            }
            if (LikeExterior)
            {
                flags |= LIKE_EXTERIOR_FLAG;
            }

            subRecords.Add(new CellData("DATA", flags, 0, 0));
        }

        protected override void UpdateRequired(Record record)
        {
            base.UpdateRequired(record);

            record.GetSubRecord<StringSubRecord>("NAME").Data = Name;

            var data = record.GetSubRecord<CellData>("DATA");
            data.GridX = data.GridY = 0;
            FlagSet(data, IsInterior, Constants.Cell.INTERIOR_FLAG, CellDataFlagSet, CellDataFlagClear);
            FlagSet(data, HasWater, HAS_WATER_FLAG, CellDataFlagSet, CellDataFlagClear);
            FlagSet(data, SleepingIllegal, SLEEPING_ILLEGAL_FLAG, CellDataFlagSet, CellDataFlagClear);
            FlagSet(data, LikeExterior, LIKE_EXTERIOR_FLAG, CellDataFlagSet, CellDataFlagClear);
        }

        protected override void UpdateOptional(Record record)
        {
            base.UpdateOptional(record);

            // Remove Exterior Sub-Records
            record.RemoveAllSubRecords("RGNN");
            record.RemoveAllSubRecords("NAM5");


            // Process Interior Sub-Records
            if (record.ContainsSubRecord("AMBI"))
            {
                var ambientData = record.GetSubRecord<InteriorLightSubRecord>("AMBI");
                ambientData.AmbientColor = AmbientColor.Copy();
                ambientData.SunlightColor = SunlightColor.Copy();
                ambientData.FogColor = FogColor.Copy();
                ambientData.FogDensity = FogDensity;
            }
            else
            {
                record.InsertSubRecordAt(record.GetAddIndex("AMBI"), new InteriorLightSubRecord("AMBI", AmbientColor.Copy(), SunlightColor.Copy(), FogColor.Copy(), FogDensity));
            }

            if (!HasWater)
            {
                record.RemoveAllSubRecords("WHGT");
            }
            else
            {
                if (record.ContainsSubRecord("WHGT"))
                {
                    record.GetSubRecord<IntSubRecord>("WHGT").Data = WaterHeight;
                }
                else
                {
                    record.InsertSubRecordAt(record.GetAddIndex("WHGT"), new FloatSubRecord("WHGT", WaterHeight));
                }
            }
        }

        protected override void DoSyncWithRecord(Record record)
        {
            base.DoSyncWithRecord(record);
            Id = record.GetSubRecord<StringSubRecord>("NAME").Data;

            var data = record.GetSubRecord<CellData>("DATA");
            HasWater = HasFlagSet(data.Flags, HAS_WATER_FLAG);
            SleepingIllegal = HasFlagSet(data.Flags, SLEEPING_ILLEGAL_FLAG);
            LikeExterior = HasFlagSet(data.Flags, LIKE_EXTERIOR_FLAG);

            var ambientData = record.TryGetSubRecord<InteriorLightSubRecord>("AMBI");
            if (ambientData != null)
            {
                AmbientColor = ambientData.AmbientColor.Copy();
                SunlightColor = ambientData.SunlightColor.Copy();
                FogColor = ambientData.FogColor.Copy();
                FogDensity = ambientData.FogDensity;
            }
            else
            {
                AmbientColor = SunlightColor = FogColor = null;
                FogDensity = 0;
            }

            WaterHeight = record.ContainsSubRecord("WHGT") ? record.GetSubRecord<IntSubRecord>("WHGT").Data : 0;
        }

        protected override void DoValidateRecord(Record record, Validator validator)
        {
            base.DoValidateRecord(record, validator);
            if (!CellUtils.IsInterior(record))
            {
                validator.AddError("Record must have interior cell flag set.");
            }
        }

        protected override void StreamSpecific(IndentWriter writer)
        {
            writer.WriteLine($"Name: {Name}");
            writer.WriteLine($"Interior: {IsInterior}");
            writer.WriteLine($"Has Water: {HasWater}");
            writer.WriteLine($"Water Height: {WaterHeight}");
            writer.WriteLine($"Like Exterior: {LikeExterior}");
            writer.WriteLine($"Ambient Color: {AmbientColor}");
            writer.WriteLine($"Sunlight Color: {SunlightColor}");
            writer.WriteLine($"Fog Color: {FogColor}");
            writer.WriteLine($"Fog Density: {FogDensity}");
        }

        public override TES3GameItem Copy()
        {
            var result = new InteriorCell(Name)
            {
                HasWater = HasWater,
                LikeExterior = LikeExterior,
                WaterHeight = WaterHeight,
                AmbientColor = AmbientColor.Copy(),
                SunlightColor = SunlightColor.Copy(),
                FogColor = FogColor.Copy(),
                FogDensity = FogDensity
            };

            CopyClone(result);
            return result;
        }
    }
}
