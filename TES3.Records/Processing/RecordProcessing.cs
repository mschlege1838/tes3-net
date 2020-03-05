

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using TES3.Records.SubRecords;


namespace TES3.Records.Processing
{

    public static class RecordProcessing
    {

        public static void ReadStream(Stream stream, ref byte[] buf, IList<Record> records)
        {

            if (buf.Length < RecordHeader.SIZE || buf.Length < SubRecordHeader.SIZE)
            {
                var buf0 = new byte[Math.Max(RecordHeader.SIZE, SubRecordHeader.SIZE)];
                Array.Copy(buf, buf0, buf.Length);
                buf = buf0;
            }

            while (stream.Position < stream.Length)
            {
                records.Add(ReadRecord(stream, ref buf));
            }
            
        }


        public static Record ReadRecord(Stream stream, ref byte[] buf)
        {
            var header = ReadRecordHeader(stream, buf);
            var recordName = header.Name;
            var subRecords = new List<SubRecord>();

            var context = new SubRecordReadContext(recordName, stream)
            {
                buf = buf
            };
            int subBytes = 0;
            while (subBytes < header.Size)
            {
                context.header = ReadSubRecordHeader(stream, buf);
                var subRecord = (SubRecord) RawUtils.ProcessSubRecord(SubRecordOperationType.Read, context);

                subRecords.Add(subRecord);

                subBytes += context.header.Size + SubRecordHeader.SIZE;
            }

            buf = context.buf;
            return new Record(header.Name, header.Flags0, header.Flags, subRecords);
        }

        public static int GetCurrentSize(Record record)
        {
            var lengthContext = new SubRecordLengthContext(record.Name);
            var size = 0;
            foreach (var subRecord in record)
            {
                lengthContext.subRecord = subRecord;
                size += ((int)RawUtils.ProcessSubRecord(SubRecordOperationType.Size, lengthContext)) + 8;
            }
            return size + 8;
        }
        
        public static void WriteRecord(Stream stream, Record record)
        {
            var recordName = record.Name;
            RawUtils.WriteBytes(stream, RawUtils.GetBytes(recordName));

            var lengthContext = new SubRecordLengthContext(recordName);
            var writeContext = new SubRecordWriteContext(recordName, stream);

            var size = 0;
            foreach (var subRecord in record)
            {
                lengthContext.subRecord = subRecord;
                size += ((int) RawUtils.ProcessSubRecord(SubRecordOperationType.Size, lengthContext)) + 8;
            }
            RawUtils.WriteBytes(stream, BitConverter.GetBytes(size));

            RawUtils.WriteBytes(stream, BitConverter.GetBytes(record.Flags0));
            RawUtils.WriteBytes(stream, BitConverter.GetBytes(record.Flags));

            foreach (var subRecord in record)
            {
                lengthContext.subRecord = writeContext.subRecord = subRecord;
                var subRecordName = subRecord.Name;

                RawUtils.WriteBytes(stream, RawUtils.GetBytes(subRecordName));
                RawUtils.WriteBytes(stream, BitConverter.GetBytes((int) RawUtils.ProcessSubRecord(SubRecordOperationType.Size, lengthContext)));

                RawUtils.ProcessSubRecord(SubRecordOperationType.Write, writeContext);

                
            }
        }


        static RecordHeader ReadRecordHeader(Stream stream, byte[] buf)
        {
            stream.Read(buf, 0, RecordHeader.SIZE);

            var name = Encoding.ASCII.GetString(buf, 0, 4);
            var size = BitConverter.ToInt32(buf, 4);
            var flags0 = BitConverter.ToInt32(buf, 8);
            var flags = BitConverter.ToInt32(buf, 12);

            return new RecordHeader(name, size, flags0, flags);
        }

        static SubRecordHeader ReadSubRecordHeader(Stream stream, byte[] buf)
        {
            stream.Read(buf, 0, SubRecordHeader.SIZE);

            var name = Encoding.ASCII.GetString(buf, 0, 4);
            var size = BitConverter.ToInt32(buf, 4);

            return new SubRecordHeader(name, size);
        }



    }

    
}