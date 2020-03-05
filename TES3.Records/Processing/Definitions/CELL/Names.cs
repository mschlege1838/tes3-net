using System;

using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.CELL
{
    static class Names
    {

        internal static readonly string[] InitialSubRecordOrder =
        {
            "NAME",
            "DELE",
            "DATA",
            "RGNN",
            "WHGT",
            "AMBI",
            "NAM0",
            "NAM5"
        };

        internal static readonly string[] CellReferenceSubRecords =
        {
            "MVRF",
            "CNDT",
            "FRMR",
            "NAME",
            "DATA",
            "XSCL",
            "DODT",
            "DNAM",
            "ANAM",
            "INTV",
            "NAM9",
            "FLTV",
            "KNAM",
            "CNAM",
            "INDX",
            "TNAM",
            "XSOL",
            "BNAM",
            "XCHG",
            "UNAM"
        };

        internal static int GetAddIndexAmbiguous(SubRecordAddIndexContext context, string name)
        {
            if (context.additionalContext == null)
            {
                throw new InvalidOperationException("Must specify additional context for obtaining cell add target indicies.");
            }
            switch ((CellObjectType) context.additionalContext)
            {
                case CellObjectType.Cell:
                    return GetAddIndexOrdered(context.record, name, InitialSubRecordOrder);
                case CellObjectType.MovedReference:
                case CellObjectType.Reference:
                    throw new InvalidOperationException(GetStrictMessage(name));
                default:
                    throw new InvalidOperationException($"Unrecognized cell object type: {context.additionalContext}");
            }

        }

        internal static int GetAddIndexCellReference(SubRecordAddIndexContext context)
        {
            var targetIndex = -1;
            foreach (var subRecord in context.record)
            {
                ++targetIndex;

                var name = subRecord.Name;
                if (name == "FRMR" || name == "MVRF")
                {
                    break;
                }
            }
            if (targetIndex == -1)
            {
                return context.record.Count;
            }

            for (var i = context.record.Count - 1; i >= targetIndex; --i)
            {
                if (Array.IndexOf(CellReferenceSubRecords, context.record[i].Name) != -1)
                {
                    return i + 1;
                }
            }
            return context.record.Count;
        }

    }
}
