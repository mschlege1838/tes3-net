
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.GMST
{
    static class Names
    {
        internal static string[] ValueRecordNames = new string[] { "STRV", "INTV", "FLTV" };

        internal static int GetAddIndexForValue(SubRecordAddIndexContext context)
        {
            var index = GetAddIndexUnordered(context.record, ValueRecordNames);
            return index == -1 ? context.record.GetLastIndex("NAME") + 1 : index + 1;
        }
    }
}
