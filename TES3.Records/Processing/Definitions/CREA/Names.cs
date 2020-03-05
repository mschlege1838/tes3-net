
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.CREA
{
    class Names
    {
        internal static readonly string[] InitalSubRecordOrder = new string[]
        {
            "NAME",
            "DELE",
            "MODL",
            "CNAM",
            "FNAM",
            "SCRI",
            "NPDT",
            "FLAG",
            "XSCL",
            "NPCO",
            "NPCS",
            "AIDT"
        };

        internal static readonly string[] TravelDestinationOrder =
        {
            "DODT",
            "DNAM"
        };

        internal static readonly string[] AISubRecords = new string[]
        {
            "AI_W",
            "AI_A",
            "AI_T",
            "AI_E",
            "AI_F",
            "CNDT"
        };

        internal static int GetAddIndexForTravelDestination(SubRecordAddIndexContext context)
        {
            var index = GetAddIndexStrict(context.record, TravelDestinationOrder);
            return index == -1 ? GetAddIndexFirst(context.record, InitalSubRecordOrder) : index + 1;
        }

        internal static int GetAddIndexForAIData(SubRecordAddIndexContext context)
        {
            var index = GetAddIndexUnordered(context.record, AISubRecords);
            if (index != -1)
            {
                return index + 1;
            }

            return GetAddIndexForTravelDestination(context);
        }
    }
}
