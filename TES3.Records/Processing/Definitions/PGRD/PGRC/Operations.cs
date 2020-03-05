
using System;
using System.Collections.Generic;

using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.PGRD.PGRC 
{
	
	[HandlesSubRecord("PGRD", "PGRC")]
    static class Operations
	{
		[HandlesOperation(SubRecordOperationType.Read)]
		internal static object Read(SubRecordReadContext context)
		{
        
            var connections = new List<int>();
            var pos = 0;
            while (pos < context.header.Size)
            {
                connections.Add(BitConverter.ToInt32(context.buf, pos)); pos += 4;
            }

            return new PathGridConnectionSubRecord(context.SubRecordName, connections);
		}
		
		[HandlesOperation(SubRecordOperationType.Size)]
		internal static object GetSize(SubRecordLengthContext context)
		{
            return 4 * ((PathGridConnectionSubRecord) context.subRecord).Connections.Count;
		}
		
		[HandlesOperation(SubRecordOperationType.Write)]
		internal static object Write(SubRecordWriteContext context)
		{
        
            var connectionData = (PathGridConnectionSubRecord) context.subRecord;

            foreach (var connection in connectionData.Connections)
            {
                WriteBytes(context.stream, BitConverter.GetBytes(connection));
            }
			return null;
		}
		
		[HandlesOperation(SubRecordOperationType.AddIndex)]
		internal static object GetAddIndex(SubRecordAddIndexContext context)
		{
			return GetAddIndexOrdered(context.record, "PGRC", Names.SubRecordOrder);
		}
		
	}
	
}
