
using System;

using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.CELL.DATA 
{
	
	[HandlesSubRecord("CELL", "DATA")]
    static class Operations
	{
		[HandlesOperation(SubRecordOperationType.Read)]
		internal static object Read(SubRecordReadContext context)
		{
            // Cell context.Header
            if (context.header.Size == 12)
            {
                var pos = 0;

                var flags = BitConverter.ToInt32(context.buf, pos); pos += 4;
                var gridX = BitConverter.ToInt32(context.buf, pos); pos += 4;
                var gridY = BitConverter.ToInt32(context.buf, pos); pos += 4;

                return new CellData(context.SubRecordName, flags, gridX, gridY);
            }
            // Reference Object
            else
            {
                var pos = 0;
                return new PositionSubRecord(context.SubRecordName, ReadPositionRef(context.buf, ref pos));
            }
		}
		
		[HandlesOperation(SubRecordOperationType.Size)]
		internal static object GetSize(SubRecordLengthContext context)
		{
            return context.subRecord is CellData ? 12 : 24;
		}
		
		[HandlesOperation(SubRecordOperationType.Write)]
		internal static object Write(SubRecordWriteContext context)
		{
            if (context.subRecord is CellData cellData)
            {
                WriteBytes(context.stream, BitConverter.GetBytes(cellData.Flags));
                WriteBytes(context.stream, BitConverter.GetBytes(cellData.GridX));
                WriteBytes(context.stream, BitConverter.GetBytes(cellData.GridY));
            }
            else if (context.subRecord is PositionSubRecord position)
            {
                WritePositionRef(context.stream, position.Data);
            }
			return null;
		}
		
		[HandlesOperation(SubRecordOperationType.AddIndex)]
		internal static object GetAddIndex(SubRecordAddIndexContext context)
		{
            return Names.GetAddIndexAmbiguous(context, "DATA");
		}
		
	}
	
}
