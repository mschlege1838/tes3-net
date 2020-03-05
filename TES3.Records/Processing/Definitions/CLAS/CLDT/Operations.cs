
using System;

using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.CLAS.CLDT 
{
	
	[HandlesSubRecord("CLAS", "CLDT")]
    static class Operations
	{
		[HandlesOperation(SubRecordOperationType.Read)]
		internal static object Read(SubRecordReadContext context)
		{
        
            var pos = 0;

            var attributeID1 = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var attributeID2 = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var specialization = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var minorID1 = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var majorID1 = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var minorID2 = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var majorID2 = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var minorID3 = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var majorID3 = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var minorID4 = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var majorID4 = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var minorID5 = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var majorID5 = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var flags = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var autoCalcFlags = BitConverter.ToInt32(context.buf, pos); pos += 4;

            return new ClassData(context.SubRecordName, attributeID1, attributeID2, specialization, minorID1,
                    majorID1, minorID2, majorID2, minorID3, majorID3, minorID4, majorID4, minorID5,
                    majorID5, flags, autoCalcFlags);
		}
		
		[HandlesOperation(SubRecordOperationType.Size)]
		internal static object GetSize(SubRecordLengthContext context)
		{
            return 60;
		}
		
		[HandlesOperation(SubRecordOperationType.Write)]
		internal static object Write(SubRecordWriteContext context)
		{
        
            var classData = (ClassData) context.subRecord;

            WriteBytes(context.stream, BitConverter.GetBytes(classData.AttributeId1));
            WriteBytes(context.stream, BitConverter.GetBytes(classData.AttributeId2));
            WriteBytes(context.stream, BitConverter.GetBytes(classData.Specialization));
            WriteBytes(context.stream, BitConverter.GetBytes(classData.MinorId1));
            WriteBytes(context.stream, BitConverter.GetBytes(classData.MajorId1));
            WriteBytes(context.stream, BitConverter.GetBytes(classData.MinorId2));
            WriteBytes(context.stream, BitConverter.GetBytes(classData.MajorId2));
            WriteBytes(context.stream, BitConverter.GetBytes(classData.MinorId3));
            WriteBytes(context.stream, BitConverter.GetBytes(classData.MajorId3));
            WriteBytes(context.stream, BitConverter.GetBytes(classData.MinorId4));
            WriteBytes(context.stream, BitConverter.GetBytes(classData.MajorId4));
            WriteBytes(context.stream, BitConverter.GetBytes(classData.MinorId5));
            WriteBytes(context.stream, BitConverter.GetBytes(classData.MajorId5));
            WriteBytes(context.stream, BitConverter.GetBytes(classData.Flags));
            WriteBytes(context.stream, BitConverter.GetBytes(classData.AutoCalcFlags));
			return null;
		}
		
		[HandlesOperation(SubRecordOperationType.AddIndex)]
		internal static object GetAddIndex(SubRecordAddIndexContext context)
		{
			return GetAddIndexOrdered(context.record, "CLDT", Names.SubRecordOrder);
		}
		
	}
	
}
