
using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.REGN.CNAM 
{
	
	[HandlesSubRecord("REGN", "CNAM")]
	class Operations
	{
		[HandlesOperation(SubRecordOperationType.Read)]
		internal static object Read(SubRecordReadContext context)
		{
            var pos = 0;
            return new ColorSubRecord(context.SubRecordName, ReadColorRef(context.buf, ref pos));
		}
		
		[HandlesOperation(SubRecordOperationType.Size)]
		internal static object GetSize(SubRecordLengthContext context)
		{
            return 4;
		}
		
		[HandlesOperation(SubRecordOperationType.Write)]
		internal static object Write(SubRecordWriteContext context)
		{
            WriteColorRef(context.stream, ((ColorSubRecord) context.subRecord).Data);
			return null;
		}
		
		[HandlesOperation(SubRecordOperationType.AddIndex)]
		internal static object GetAddIndex(SubRecordAddIndexContext context)
		{
			return GetAddIndexOrdered(context.record, "CNAM", Names.SubRecordOrder);
		}
		
	}
	
}
