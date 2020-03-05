
using System;
using System.Collections.Generic;

using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.PGRD.PGRP 
{
	
	[HandlesSubRecord("PGRD", "PGRP")]
    static class Operations
	{
		[HandlesOperation(SubRecordOperationType.Read)]
		internal static object Read(SubRecordReadContext context)
		{
        
            var points = new List<RawPathGridPoint>();
            var pos = 0;
            while (pos < context.header.Size)
            {
                var x = BitConverter.ToInt32(context.buf, pos); pos += 4;
                var y = BitConverter.ToInt32(context.buf, pos); pos += 4;
                var z = BitConverter.ToInt32(context.buf, pos); pos += 4;

                var generated = (context.buf[pos++] & 0x01) == 0;
                var connectionCount = context.buf[pos++];
                var unknown = BitConverter.ToInt16(context.buf, pos); pos += 2;

                points.Add(new RawPathGridPoint(x, y, z, generated, connectionCount, unknown));
            }

            return new PathGridPointSubRecord(context.SubRecordName, points);
		}
		
		[HandlesOperation(SubRecordOperationType.Size)]
		internal static object GetSize(SubRecordLengthContext context)
		{
            return 16 * ((PathGridPointSubRecord) context.subRecord).Points.Count;
		}
		
		[HandlesOperation(SubRecordOperationType.Write)]
		internal static object Write(SubRecordWriteContext context)
		{
        
            var pointData = (PathGridPointSubRecord) context.subRecord;

            foreach (var point in pointData.Points)
            {
                WriteBytes(context.stream, BitConverter.GetBytes(point.X));
                WriteBytes(context.stream, BitConverter.GetBytes(point.Y));
                WriteBytes(context.stream, BitConverter.GetBytes(point.Z));

                context.stream.WriteByte((byte) (point.Generated ? 0x00 : 0x01));
                context.stream.WriteByte(point.ConnectionCount);
                WriteBytes(context.stream, BitConverter.GetBytes(point.Unknown));
            }
			return null;
		}
		
		[HandlesOperation(SubRecordOperationType.AddIndex)]
		internal static object GetAddIndex(SubRecordAddIndexContext context)
		{
			return GetAddIndexOrdered(context.record, "PGRP", Names.SubRecordOrder);
		}
		
	}
	
}
