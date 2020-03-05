
using System;

using TES3.Core;
using TES3.Records.SubRecords;


namespace TES3.Records
{
    public static class CellUtils
    {

        public static readonly string DefaultCellName = "Wilderness";

        public static string GetName(Record record)
        {
            if (record.Name != "CELL")
            {
                throw new ArgumentException($"Record is not a cell: {record}");
            }

            var nameSubRecord = record.GetSubRecord<StringSubRecord>("NAME").Data;
            if (nameSubRecord.Length > 0)
            {
                return nameSubRecord;
            }

            var region = record.TryGetSubRecord<StringSubRecord>("RGNN")?.Data;
            if (!string.IsNullOrEmpty(region))
            {
                return region;
            }

            return DefaultCellName;
        }

        public static bool IsInterior(Record record)
        {
            if (record.Name != "CELL")
            {
                return false;
            }

            return (record.GetSubRecord<CellData>("DATA").Flags & Constants.Cell.INTERIOR_FLAG) != 0;
        }

        public static CellGridKey GetGridKey(Record record)
        {
            if (record.Name != "CELL")
            {
                throw new ArgumentException($"Record is not a cell: {record}");
            }

            var data = record.GetSubRecord<CellData>("DATA");
            return new CellGridKey(data.GridX, data.GridY);
        }


    }
}
