
using System;

using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.TES3.HEDR 
{
	
	[HandlesSubRecord("TES3", "HEDR")]
    static class Operations
	{
		[HandlesOperation(SubRecordOperationType.Read)]
		internal static object Read(SubRecordReadContext context)
		{
        
            var pos = 0;

            var version = BitConverter.ToSingle(context.buf, pos); pos += 4;
            var fileType = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var companyName = GetString(context.buf, pos, Constants.TES3_COMPANY_NAME_LENGTH); pos += Constants.TES3_COMPANY_NAME_LENGTH;
            var description = GetString(context.buf, pos, Constants.TES3_DESCRIPTION_LENGTH); pos += Constants.TES3_DESCRIPTION_LENGTH;
            var numRecords = BitConverter.ToInt32(context.buf, pos); pos += 4;

            return new TES3HeaderData(context.SubRecordName, version, fileType, companyName, description, numRecords);
		}
		
		[HandlesOperation(SubRecordOperationType.Size)]
		internal static object GetSize(SubRecordLengthContext context)
		{
            return 300;
		}
		
		[HandlesOperation(SubRecordOperationType.Write)]
		internal static object Write(SubRecordWriteContext context)
		{
        
            var fileHeader = (TES3HeaderData) context.subRecord;

            WriteBytes(context.stream, BitConverter.GetBytes(fileHeader.Version));
            WriteBytes(context.stream, BitConverter.GetBytes(fileHeader.FileType));
            WriteBytes(context.stream, GetBytes(fileHeader.CompanyName, Constants.TES3_COMPANY_NAME_LENGTH, "TES3.HEDR.CompanyName"));
            WriteBytes(context.stream, GetBytes(fileHeader.Description, Constants.TES3_DESCRIPTION_LENGTH, "TES3.HEDR.Description"));
            WriteBytes(context.stream, BitConverter.GetBytes(fileHeader.NumRecords));
			return null;
		}
		
		[HandlesOperation(SubRecordOperationType.AddIndex)]
		internal static object GetAddIndex(SubRecordAddIndexContext context)
		{
            return context.record.GetLastIndex("HEDR") + 1;
		}
		
	}
	
}
