using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using TES3.Core;
using TES3.Records.SubRecords;


namespace TES3.Records.Processing
{

    static class RawUtils
    {

        const int REALLOCATION_STEP_SIZE = 128;


        static readonly IDictionary<SubRecordProcessKey, MethodInfo> SubRecordProcessingDefinitions = new Dictionary<SubRecordProcessKey, MethodInfo>();
        static readonly IDictionary<RecordProcessKey, MethodInfo> RecordProcessingDefinitions = new Dictionary<RecordProcessKey, MethodInfo>();

        static RawUtils()
        {
            var types = typeof(RawUtils).Assembly.GetTypes();

            {
                var q = from type in types
                        let typeAttribute = (HandlesSubRecord) Attribute.GetCustomAttribute(type, typeof(HandlesSubRecord))
                        where typeAttribute != null

                        from method in type.GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
                        let methodAttribute = (HandlesOperation) Attribute.GetCustomAttribute(method, typeof(HandlesOperation))
                        where methodAttribute != null
                        select new { typeAttribute, methodAttribute, method }
                        ;

                foreach (var val in q)
                {
                    var key = new SubRecordProcessKey(val.typeAttribute.RecordName, val.typeAttribute.SubRecordName, val.methodAttribute.SubRecordOperation);
                    if (SubRecordProcessingDefinitions.ContainsKey(key))
                    {
                        throw new InvalidOperationException($"Ambiguous Handler: {key}");
                    }

                    SubRecordProcessingDefinitions.Add(key, val.method);
                }
            }

            {
                var q = from type in types
                        let typeAttribute = (HandlesRecord) Attribute.GetCustomAttribute(type, typeof(HandlesRecord))
                        where typeAttribute != null

                        from method in type.GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
                        let methodAttribute = (HandlesOperation) Attribute.GetCustomAttribute(method, typeof(HandlesOperation))
                        where methodAttribute != null
                        select new { typeAttribute, methodAttribute, method }
                        ;

                foreach (var val in q)
                {
                    var key = new RecordProcessKey(val.typeAttribute.RecordName, val.methodAttribute.RecordOperation);
                    if (RecordProcessingDefinitions.ContainsKey(key))
                    {
                        throw new InvalidOperationException($"Ambiguous Handler: {key}");
                    }

                    RecordProcessingDefinitions.Add(key, val.method);
                }
            }
        }


        internal static object ProcessSubRecord(SubRecordOperationType operation, ISubRecordOperationContext context)
        {
            var key = new SubRecordProcessKey(context.RecordName, context.SubRecordName, operation);
            if (!SubRecordProcessingDefinitions.ContainsKey(key))
            {
                throw new ArgumentException($"Unrecognized sub-record operation: {key}");
            }

            if (operation == SubRecordOperationType.Read)
            {
                var readContext = (SubRecordReadContext) context;
                if (readContext.buf.Length < readContext.header.Size)
                {
                    var buf0 = new byte[readContext.header.Size + REALLOCATION_STEP_SIZE];
                    Array.Copy(readContext.buf, buf0, readContext.buf.Length);
                    readContext.buf = buf0;
                }

                readContext.stream.Read(readContext.buf, 0, readContext.header.Size);
            }

            
            return SubRecordProcessingDefinitions[key].Invoke(null, new object[] { context });
        }

        internal static object ProcessRecord(RecordOperationType operation, IRecordOperationContext context)
        {
            var key = new RecordProcessKey(context.RecordName, operation);
            if (!RecordProcessingDefinitions.ContainsKey(key))
            {
                throw new ArgumentException($"Unrecognized record operation: {key}");
            }

            return RecordProcessingDefinitions[key].Invoke(null, new object[] { context });
        }


        internal static string GetStrictMessage(string name)
        {
            return $"Request for strictly ordered sub-record that is not the first: {name}";
        }

        internal static int GetAddIndexUnordered<T>(IRecord<T> record, string[] order) where T : INamed
        {
            var max = -1;
            foreach (var name in order)
            {
                var index = record.GetLastIndex(name);
                if (index > max)
                {
                    max = index;
                }
            }

            return max;
        }

        internal static int GetAddIndexStrict<T>(IRecord<T> record, string[] order) where T : INamed
        {
            for (var i = order.Length - 1; i >= 0; --i)
            {
                var lastIndex = record.GetLastIndex(order[i]);
                if (lastIndex != -1)
                {
                    return lastIndex + 1;
                }
            }

            return -1;
        }

        internal static int GetAddIndexOrdered<T>(IRecord<T> record, string name, string[] order) where T : INamed
        {
            var startIndex = Array.IndexOf(order, name);
            if (startIndex == order.Length - 1)
            {
                return record.Count;
            }
            for (var i = startIndex; i >= 0; --i)
            {
                var lastIndex = record.GetLastIndex(order[i]);
                if (lastIndex != -1)
                {
                    return lastIndex + 1;
                }
            }

            return 0;
        }

        internal static int GetAddIndexFirst<T>(IRecord<T> record, string[] order) where T : INamed
        {
            for (var i = order.Length - 1; i >= 0; --i)
            {
                var lastIndex = record.GetLastIndex(order[i]);
                if (lastIndex != -1)
                {
                    return lastIndex + 1;
                }
            }

            return 0;
        }

        internal static string GetString(byte[] buf, int index, int count)
        {
            var result = GlobalConfig.TextEncoding.GetString(buf, index, count);
            return GlobalConfig.TrimNullChars ? result.TrimEnd('\0') : result;
        }

        internal static ColorRef ReadColorRef(byte[] data, ref int pos)
        {
            byte red = data[pos++];
            byte green = data[pos++];
            byte blue = data[pos++];
            byte alpha = data[pos++];

            return new ColorRef(red, green, blue, alpha);
        }

        internal static void WriteColorRef(Stream stream, ColorRef data)
        {
            stream.WriteByte(data.Red);
            stream.WriteByte(data.Green);
            stream.WriteByte(data.Blue);
            stream.WriteByte(data.Alpha);
        }


        internal static PositionRef ReadPositionRef(byte[] data, ref int pos)
        {
            float xPos = BitConverter.ToSingle(data, pos); pos += 4;
            float yPos = BitConverter.ToSingle(data, pos); pos += 4;
            float zPos = BitConverter.ToSingle(data, pos); pos += 4;
            float xRot = BitConverter.ToSingle(data, pos); pos += 4;
            float yRot = BitConverter.ToSingle(data, pos); pos += 4;
            float zRot = BitConverter.ToSingle(data, pos); pos += 4;

            return new PositionRef(xPos, yPos, zPos, xRot, yRot, zRot);
        }

        internal static void WritePositionRef(Stream stream, PositionRef position)
        {
            WriteBytes(stream, BitConverter.GetBytes(position.XPos));
            WriteBytes(stream, BitConverter.GetBytes(position.YPos));
            WriteBytes(stream, BitConverter.GetBytes(position.ZPos));
            WriteBytes(stream, BitConverter.GetBytes(position.XRot));
            WriteBytes(stream, BitConverter.GetBytes(position.YRot));
            WriteBytes(stream, BitConverter.GetBytes(position.ZRot));
        }

        internal static byte[] GetBytes(string s, int size, string name)
        {
            if (s.Length > size)
            {
                Console.Error.WriteLine($"Warning: The field {name} is limited to {size} characters, but given value has a size of {s.Length}. The written value will be truncated.");
            }

            var result = new byte[size];
            var offset = GlobalConfig.TextEncoding.GetBytes(s, 0, Math.Min(size, s.Length), result, 0);
            for (; offset < size; ++offset)
            {
                result[offset] = 0;
            }

            return result;
        }

        internal static byte[] GetBytes(string s, bool nullTerminated = false)
        {
            var byteCount = GlobalConfig.TextEncoding.GetByteCount(s);
            var append = nullTerminated && !s.EndsWith("\0");
            if (append)
            {
                byteCount += 1;
            }


            var result = new byte[byteCount];
            GlobalConfig.TextEncoding.GetBytes(s, 0, s.Length, result, 0);
            if (append)
            {
                result[byteCount - 1] = 0;
            }
            return result;
        }

        internal static void WriteBytes(Stream stream, byte[] buf)
        {
            stream.Write(buf, 0, buf.Length);
        }

        internal static int GetByteLength(SubRecord subRecord, bool nullTerminated = false)
        {
            var s = ((StringSubRecord) subRecord).Data;
            var len = GlobalConfig.TextEncoding.GetByteCount(s);
            return nullTerminated && !s.EndsWith("\0") ? len + 1 : len;
        }



        class SubRecordProcessKey
        {
            readonly string recordName;
            readonly string subRecordName;
            readonly SubRecordOperationType type;

            internal SubRecordProcessKey(string recordName, string subRecordName, SubRecordOperationType type)
            {
                this.recordName = recordName;
                this.subRecordName = subRecordName;
                this.type = type;
            }

            public override int GetHashCode()
            {
                var result = 1;
                result = result * 31 + recordName.GetHashCode();
                result = result * 31 + subRecordName.GetHashCode();
                result = result * 31 + (int) type;
                return result;
            }

            public override bool Equals(object obj)
            {
                if (obj == null || GetType() != obj.GetType())
                {
                    return false;
                }

                var other = (SubRecordProcessKey) obj;
                return type == other.type && recordName == other.recordName && subRecordName == other.subRecordName;
            }

            public override string ToString()
            {
                return $"{recordName}.{subRecordName}({type})";
            }
        }

        class RecordProcessKey
        {
            readonly string recordName;
            readonly RecordOperationType type;

            internal RecordProcessKey(string recordName, RecordOperationType type)
            {
                this.recordName = recordName;
                this.type = type;
            }

            public override int GetHashCode()
            {
                var result = 1;
                result = result * 31 + recordName.GetHashCode();
                result = result * 31 + (int) type;
                return result;
            }

            public override bool Equals(object obj)
            {
                if (obj == null || GetType() != obj.GetType())
                {
                    return false;
                }

                var other = (RecordProcessKey) obj;
                return type == other.type && recordName == other.recordName;
            }

            public override string ToString()
            {
                return $"{recordName}({type})";
            }
        }
    }


    
}
