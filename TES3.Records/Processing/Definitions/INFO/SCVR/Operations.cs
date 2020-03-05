
using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.INFO.SCVR 
{
	
	[HandlesSubRecord("INFO", "SCVR")]
    static class Operations
	{
        static readonly string[] FunctionValueNames = new string[] { "INTV", "FLTV" };

		[HandlesOperation(SubRecordOperationType.Read)]
		internal static object Read(SubRecordReadContext context)
		{
            return new StringSubRecord(context.SubRecordName, GetString(context.buf, 0, context.header.Size));
		}
		
		[HandlesOperation(SubRecordOperationType.Size)]
		internal static object GetSize(SubRecordLengthContext context)
		{
            return GetByteLength(context.subRecord, false);
		}
		
		[HandlesOperation(SubRecordOperationType.Write)]
		internal static object Write(SubRecordWriteContext context)
		{
            WriteBytes(context.stream, GetBytes(((StringSubRecord) context.subRecord).Data, false));
			return null;
		}
		
		[HandlesOperation(SubRecordOperationType.AddIndex)]
		internal static object GetAddIndex(SubRecordAddIndexContext context)
		{
            var index = GetAddIndexStrict(context.record, Names.FunctionSubRecordOrder);
            return index == -1 ? GetAddIndexFirst(context.record, Names.InitialSubRecordOrder) : index + 1;
		}
		
	}
	
}
