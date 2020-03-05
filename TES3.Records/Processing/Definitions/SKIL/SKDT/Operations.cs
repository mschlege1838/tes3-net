
using System;

using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.SKIL.SKDT 
{
	
	[HandlesSubRecord("SKIL", "SKDT")]
    static class Operations
	{
		[HandlesOperation(SubRecordOperationType.Read)]
		internal static object Read(SubRecordReadContext context)
		{
        
            var pos = 0;

            var attribute = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var specialization = BitConverter.ToInt32(context.buf, pos); pos += 4;

            var useValue = new float[Constants.SKIL_USE_VALUE_LENGTH];
            for (int i = 0; i < Constants.SKIL_USE_VALUE_LENGTH; ++i)
            {
                useValue[i] = BitConverter.ToSingle(context.buf, pos);
                pos += 4;
            }

            return new SkillData(context.SubRecordName, attribute, specialization, useValue);
		}
		
		[HandlesOperation(SubRecordOperationType.Size)]
		internal static object GetSize(SubRecordLengthContext context)
		{
            return 24;
		}
		
		[HandlesOperation(SubRecordOperationType.Write)]
		internal static object Write(SubRecordWriteContext context)
		{
        
            var skillData = (SkillData) context.subRecord;

            WriteBytes(context.stream, BitConverter.GetBytes(skillData.Attribute));
            WriteBytes(context.stream, BitConverter.GetBytes(skillData.Specialization));

            for (int i = 0; i < Constants.SKIL_USE_VALUE_LENGTH; ++i)
            {
                WriteBytes(context.stream, BitConverter.GetBytes(skillData.UseValue[i]));
            }
			return null;
		}
		
		[HandlesOperation(SubRecordOperationType.AddIndex)]
		internal static object GetAddIndex(SubRecordAddIndexContext context)
		{
			return GetAddIndexOrdered(context.record, "SKDT", Names.SubRecordOrder);
		}
		
	}
	
}
