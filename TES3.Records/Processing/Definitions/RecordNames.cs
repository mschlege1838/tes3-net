
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions
{
    class RecordNames
    {
        internal static readonly string[] InitialRecordOrder = new string[]
        {
            "TES3",
            "GMST",
            "GLOB",
            "CLAS",
            "FACT",
            "RACE",
            "SOUN",
            "SKIL",
            "MGEF",
            "SCPT",
            "REGN",
            "SSCR",
            "BSGN",
            "LTEX"
        };

        internal static readonly string[] GameItemRecords = new string[]
        {
            "STAT",
            "DOOR",
            "MISC",
            "WEAP",
            "CONT",
            "SPEL",
            "CREA",
            "BODY",
            "LIGH",
            "ENCH",
            "NPC_",
            "ARMO",
            "CLOT",
            "REPA",
            "ACTI",
            "APPA",
            "LOCK",
            "PROB",
            "INGR",
            "BOOK",
            "ALCH",
            "LEVI",
            "LEVC"
        };

        internal static readonly string[] CellRecordOrder = new string[]
        {
            "CELL",
            "LAND",
            "PGRD"
        };

        internal static readonly string[] DialogueRecordOrder = new string[]
        {
            "DIAL",
            "INFO"
        };

        internal static int GetAddIndexGameItem(RecordAddIndexContext context)
        {
            var index = GetAddIndexUnordered(context.modFile, GameItemRecords);
            return index == -1 ? GetAddIndexFirst(context.modFile, InitialRecordOrder) : index + 1;
        }
    }
}
