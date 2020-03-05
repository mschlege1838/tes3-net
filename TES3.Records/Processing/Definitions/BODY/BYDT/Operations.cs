
using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.BODY.BYDT 
{
	
	[HandlesSubRecord("BODY", "BYDT")]
    static class Operations
	{
		[HandlesOperation(SubRecordOperationType.Read)]
		internal static object Read(SubRecordReadContext context)
		{
        
            var part = context.buf[0];
            var vampire = context.buf[1];
            var flags = context.buf[2];
            var partType = context.buf[3];

            return new BodyPartData(context.SubRecordName, part, vampire, flags, partType);
		}
		
		[HandlesOperation(SubRecordOperationType.Size)]
		internal static object GetSize(SubRecordLengthContext context)
		{
            return 4;
		}
		
		[HandlesOperation(SubRecordOperationType.Write)]
		internal static object Write(SubRecordWriteContext context)
		{
        
            var bodyData = (BodyPartData) context.subRecord;

            context.stream.WriteByte(bodyData.Part);
            context.stream.WriteByte(bodyData.Vampire);
            context.stream.WriteByte(bodyData.Flags);
            context.stream.WriteByte(bodyData.PartType);
			return null;
		}
		
		[HandlesOperation(SubRecordOperationType.AddIndex)]
		internal static object GetAddIndex(SubRecordAddIndexContext context)
		{
			return GetAddIndexOrdered(context.record, "BYDT", Names.SubRecordOrder);
		}
		
	}
	
}
