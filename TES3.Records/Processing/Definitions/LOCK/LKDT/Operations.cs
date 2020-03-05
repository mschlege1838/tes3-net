using System;

using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.LOCK.LKDT
{
    [HandlesSubRecord("LOCK", "LKDT")]
    static class Operations
    {
        [HandlesOperation(SubRecordOperationType.Read)]
        internal static object Read(SubRecordReadContext context)
        {

            var pos = 0;

            var weight = BitConverter.ToSingle(context.buf, pos); pos += 4;
            var value = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var quality = BitConverter.ToSingle(context.buf, pos); pos += 4;
            var uses = BitConverter.ToInt32(context.buf, pos); pos += 4;

            return new LockpickData(context.SubRecordName, weight, value, quality, uses);
        }

        [HandlesOperation(SubRecordOperationType.Size)]
        internal static object GetSize(SubRecordLengthContext context)
        {
            return 16;
        }

        [HandlesOperation(SubRecordOperationType.Write)]
        internal static object Write(SubRecordWriteContext context)
        {

            var lockData = (LockpickData) context.subRecord;

            WriteBytes(context.stream, BitConverter.GetBytes(lockData.Weight));
            WriteBytes(context.stream, BitConverter.GetBytes(lockData.Value));
            WriteBytes(context.stream, BitConverter.GetBytes(lockData.Quality));
            WriteBytes(context.stream, BitConverter.GetBytes(lockData.Uses));
            return null;
        }

        [HandlesOperation(SubRecordOperationType.AddIndex)]
        internal static object GetAddIndex(SubRecordAddIndexContext context)
        {
            return GetAddIndexOrdered(context.record, "LKDT", Names.SubRecordOrder);
        }
    }
}
