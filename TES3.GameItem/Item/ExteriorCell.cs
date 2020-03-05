
using System.Collections.Generic;

using TES3.Core;
using TES3.Records;
using TES3.Records.SubRecords;
using TES3.Util;

namespace TES3.GameItem.Item
{
    public class ExteriorCell : Cell
    {

        string name;

        public ExteriorCell(CellGridKey grid) : base(grid)
        {

        }

        public ExteriorCell(Record record) : base(record)
        {

        }

        public override bool IsInterior => false;

        public string Name
        {
            get => name;
            set => name = value ?? "";
        }

        public override string DisplayName
        {
            get
            {
                string name;
                if (Name.Length > 0)
                {
                    name = Name;
                }
                else if (!string.IsNullOrEmpty(Region))
                {
                    name = Region;
                }
                else
                {
                    name = CellUtils.DefaultCellName;
                }

                return $"{name}, {Grid}";
            }
        }

        [IdField]
        public CellGridKey Grid
        {
            get => (CellGridKey) Id;
            set => Id = value;
        }

        public string Region
        {
            get;
            set;
        }

        public ColorRef MapColor
        {
            get;
            set;
        }

        protected override void OnCreate(IList<SubRecord> subRecords)
        {
            base.OnCreate(subRecords);
            subRecords.Add(new StringSubRecord("NAME", Name));
            subRecords.Add(new CellData("DATA", 0, Grid.GridX, Grid.GridY));
        }

        protected override void UpdateRequired(Record record)
        {
            base.UpdateRequired(record);

            record.GetSubRecord<StringSubRecord>("NAME").Data = Name;

            var data = record.GetSubRecord<CellData>("DATA");
            data.GridX = Grid.GridX;
            data.GridY = Grid.GridY;
            FlagSet(data, IsInterior, Constants.Cell.INTERIOR_FLAG, CellDataFlagSet, CellDataFlagClear);
            FlagSet(data, false, HAS_WATER_FLAG, CellDataFlagSet, CellDataFlagClear);
            FlagSet(data, SleepingIllegal, SLEEPING_ILLEGAL_FLAG, CellDataFlagSet, CellDataFlagClear);
            FlagSet(data, false, LIKE_EXTERIOR_FLAG, CellDataFlagSet, CellDataFlagClear);
        }

        protected override void UpdateOptional(Record record)
        {
            base.UpdateOptional(record);
            
            ProcessOptional(record, "RGNN", Region != null, () => new StringSubRecord("RGNN", Region), (sr) => sr.Data = Region);

            // Remove Interior Sub-Records
            record.RemoveAllSubRecords("AMBI", "WHGT");


            // Process Exterior Sub-Records
            if (MapColor == null)
            {
                record.RemoveAllSubRecords("NAM5");
            }
            else
            {
                if (record.ContainsSubRecord("NAM5"))
                {
                    record.GetSubRecord<ColorSubRecord>("NAM5").Data = MapColor.Copy();
                }
                else
                {
                    record.InsertSubRecordAt(record.GetAddIndex("NAM5"), new ColorSubRecord("NAM5", MapColor.Copy()));
                }
            }
        }

        protected override void DoSyncWithRecord(Record record)
        {
            base.DoSyncWithRecord(record);

            Name = record.GetSubRecord<StringSubRecord>("NAME").Data;

            var data = record.GetSubRecord<CellData>("DATA");
            Id = new CellGridKey(data.GridX, data.GridY);
            SleepingIllegal = HasFlagSet(data.Flags, SLEEPING_ILLEGAL_FLAG);

            Region = record.TryGetSubRecord<StringSubRecord>("RGNN")?.Data;
            MapColor = record.ContainsSubRecord("NAM5") ? record.GetSubRecord<ColorSubRecord>("NAM5").Data.Copy() : null;
        }

        protected override void DoValidateRecord(Record record, Validator validator)
        {
            base.DoValidateRecord(record, validator);
            if (CellUtils.IsInterior(record))
            {
                validator.AddError("Record must have interior cell flag cleared.");
            }
        }

        protected override void StreamSpecific(IndentWriter writer)
        {
            writer.WriteLine($"Name: {Name}");
            writer.WriteLine($"Region: {Region}");
            writer.WriteLine($"Interior: {IsInterior}");
            writer.WriteLine($"Grid: ({Grid.GridX}, {Grid.GridY})");
            writer.WriteLine($"Map Color: {MapColor}");
        }

        public override TES3GameItem Clone()
        {
            var result = new ExteriorCell(Grid)
            {
                Name = Name,
                Region = Region,
                MapColor = MapColor.Copy()
            };

            CopyClone(result);
            return result;
        }

    }
}
