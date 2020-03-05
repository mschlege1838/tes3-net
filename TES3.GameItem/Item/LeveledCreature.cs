
using System.IO;

using TES3.Records;
using TES3.Util;

namespace TES3.GameItem.Item
{

    [TargetRecord("LEVC")]
    public class LeveledCreature : LeveledList
    {
        public LeveledCreature(string name) : base(name)
        {

        }

        public LeveledCreature(Record record) : base(record)
        {

        }

        protected override string ListItemName => "CNAM";

        public override string RecordName => "LEVC";

        public override TES3GameItem Clone()
        {
            var result = new LeveledCreature(Name);
            CopyClone(result);
            return result;
        }

        public override void StreamDebug(TextWriter target)
        {
            var writer = new IndentWriter(target);

            writer.WriteLine(ToString());

            writer.IncIndent();
            writer.WriteLine($"Calc From All Levels >= PC Level: {CalcFromPCLevel}");
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
            return $"Leveled Creature ({Name})";
        }
    }
}
