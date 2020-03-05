
using System.IO;
using TES3.Records;
using TES3.Records.SubRecords;
using TES3.Util;

namespace TES3.GameItem.Item
{

    [TargetRecord("LEVI")]
    public class LeveledItem : LeveledList
    {
        const int CALC_FOR_EACH_ITEM_FLAG = 0x02;

        public LeveledItem(string name) : base(name)
        {
            
        }

        public LeveledItem(Record record) : base(record)
        {

        }

        public bool CalcForEachItem
        {
            get;
            set;
        }

        protected override string ListItemName => "INAM";

        public override string RecordName => "LEVI";


        protected override void UpdateData(IntSubRecord subRecord)
        {
            FlagSet(subRecord, CalcForEachItem, CALC_FOR_EACH_ITEM_FLAG, IntFlagSet, IntFlagClear);
        }

        protected override void ProcessData(IntSubRecord subRecord)
        {
            CalcForEachItem = HasFlagSet(subRecord.Data, CALC_FOR_EACH_ITEM_FLAG);
        }

        public override TES3GameItem Clone()
        {
            var result = new LeveledItem(Name);
            CopyClone(result);
            return result;
        }

        public override void StreamDebug(TextWriter target)
        {
            var writer = new IndentWriter(target);

            writer.WriteLine(ToString());

            writer.IncIndent();
            writer.WriteLine($"Calc From All Levels >= PC Level: {CalcFromPCLevel}");
            writer.WriteLine($"Calc For Each Item: {CalcForEachItem}");
            writer.WriteLine($"Chance None: {ChanceNone}");

            if (ItemList.Count > 0)
            {
                writer.WriteLine("Items");
                writer.IncIndent();

                var table = new Table("Name", "PC Level");
                foreach (var item in ItemList)
                {
                    table.AddRow(item.Name, item.PCLevel);
                }
                table.Print(writer);

                writer.DecIndent();
            }
            writer.DecIndent();
        }

        public override string ToString()
        {
            return $"Leveled Item ({Name})";
        }
    }
}
