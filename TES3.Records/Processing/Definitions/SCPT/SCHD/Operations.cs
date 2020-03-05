
using System;

using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.SCPT.SCHD 
{
	
	[HandlesSubRecord("SCPT", "SCHD")]
    static class Operations
	{
		[HandlesOperation(SubRecordOperationType.Read)]
		internal static object Read(SubRecordReadContext context)
		{
        
            var pos = 0;

            var name = GetString(context.buf, pos, 32); pos += 32;

            var numShorts = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var numLongs = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var numFloats = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var dataSize = BitConverter.ToInt32(context.buf, pos); pos += 4;
            var localVarSize = BitConverter.ToInt32(context.buf, pos); pos += 4;

            return new ScriptHeader(context.SubRecordName, name, numShorts, numLongs, numFloats, dataSize, localVarSize);
		}
		
		[HandlesOperation(SubRecordOperationType.Size)]
		internal static object GetSize(SubRecordLengthContext context)
		{
            return 52;
		}
		
		[HandlesOperation(SubRecordOperationType.Write)]
		internal static object Write(SubRecordWriteContext context)
		{
        
            var scriptHeader = (ScriptHeader) context.subRecord;

            WriteBytes(context.stream, GetBytes(scriptHeader.ScriptName, Constants.Script.SCHD_NAME_MAX_LENGTH, "SCPT.SCHD.ScriptName"));
            WriteBytes(context.stream, BitConverter.GetBytes(scriptHeader.NumShorts));
            WriteBytes(context.stream, BitConverter.GetBytes(scriptHeader.NumLongs));
            WriteBytes(context.stream, BitConverter.GetBytes(scriptHeader.NumFloats));
            WriteBytes(context.stream, BitConverter.GetBytes(scriptHeader.DataSize));
            WriteBytes(context.stream, BitConverter.GetBytes(scriptHeader.LocalVarSize));
			return null;
		}
		
		[HandlesOperation(SubRecordOperationType.AddIndex)]
		internal static object GetAddIndex(SubRecordAddIndexContext context)
		{
			return GetAddIndexOrdered(context.record, "SCHD", Names.SubRecordOrder);
		}
		
	}
	
}
