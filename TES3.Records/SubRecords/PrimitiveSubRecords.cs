using System;
using System.Text;
using TES3.Core;

namespace TES3.Records.SubRecords
{
    public class ByteSubRecord : SubRecord
    {
        public byte Data { get; set; }

        public ByteSubRecord(string name, byte data) : base(name)
        {
            Data = data;
        }

        public override string ToString()
        {
            return $"{Name} ({Data})";
        }
    }

    public class ShortSubRecord : SubRecord
    {
        public short Data { get; set; }

        public ShortSubRecord(string name, short data) : base(name)
        {
            Data = data;
        }

        public override string ToString()
        {
            return $"{Name} ({Data})";
        }
    }

    public class StringSubRecord : SubRecord
    {

        string data;

        public StringSubRecord(string name, string data) : base(name)
        {
            Data = data;
        }


        public string Data
        {
            get => data;
            set => data = value ?? throw new ArgumentNullException("data", "Data cannot be null.");
        }

        public override string ToString()
        {
            return $"{Name} ({Data})({Data.Length} chars)";
        }

    }

    public class IntSubRecord : SubRecord
    {
        public int Data { get; set; }

        public IntSubRecord(string name, int data) : base(name)
        {
            Data = data;
        }

        public override string ToString()
        {
            return $"{Name} ({Data})";
        }
    }

    public class FloatSubRecord : SubRecord
    {
        public float Data { get; set; }

        public FloatSubRecord(string name, float data) : base(name)
        {
            Data = data;
        }

        public override string ToString()
        {
            return $"{Name} ({Data})";
        }
    }

    public class LongSubRecord : SubRecord
    {
        public long Data { get; set; }

        public LongSubRecord(string name, long data) : base(name)
        {
            Data = data;
        }

        public override string ToString()
        {
            return $"{Name} ({Data})";
        }

    }

    public class GenericSubRecord : SubRecord
    {
        byte[] data;

        public GenericSubRecord(string name, byte[] data, bool recognized = true) : base(name)
        {
            Data = data;
            Recognized = recognized;
        }

        public byte[] Data
        {
            get => data;
            set => data = value ?? throw new ArgumentNullException("data", "Data cannot be null.");
        }

        public bool Recognized
        {
            get;
            set;
        }

        public override string ToString()
        {
            if (data.Length == 0)
            {
                return Name;
            }

            var builder = new StringBuilder();

            var enumerator = data.GetEnumerator();
            enumerator.MoveNext();
            builder.Append($"0x{enumerator.Current:X2}");
            while (enumerator.MoveNext())
            {
                builder.Append($", 0x{enumerator.Current:X2}");
            }


            return $"{Name} ({builder}) recognized({Recognized})";
        }
    }


    public class ColorSubRecord : SubRecord
    {
        ColorRef data;

        public ColorSubRecord(string name, ColorRef data) : base(name)
        {
            Data = data;
        }

        public ColorRef Data
        {
            get => data;
            set => data = value ?? throw new ArgumentNullException("data", "Data cannot be null.");
        }

        public override string ToString()
        {
            return $"{Name} {Data}";
        }
    }

    public class PositionSubRecord : SubRecord
    {
        PositionRef data;

        public PositionSubRecord(string name, PositionRef data) : base(name)
        {
            Data = data;
        }

        public PositionRef Data
        {
            get => data;
            set => data = value ?? throw new ArgumentNullException("data", "Data cannot be null.");
        }

        public override string ToString()
        {
            return $"{Name} {Data}";
        }
    }

}