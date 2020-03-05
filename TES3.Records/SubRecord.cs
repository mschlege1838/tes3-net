
using System;


namespace TES3.Records
{

    public abstract class SubRecord : INamed
    { 
		
		public string Name { get; }

        public SubRecord(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name", "Name cannot be null.");
            }
            if (name.Length != 4)
            {
                throw new ArgumentOutOfRangeException("name", name.Length, "Record names must be 4 characters.");
            }

            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }

    }


    public class SubRecordHeader
    {
        public const int SIZE = 8;

        public string Name { get; }
        public int Size { get; }

        public SubRecordHeader(string name, int size)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name", "Name cannot be null.");
            }
            if (name.Length != 4)
            {
                throw new ArgumentOutOfRangeException("name", name.Length, "Record names must be 4 characters.");
            }
            if (size < 0)
            {
                throw new ArgumentOutOfRangeException("Size", size, "Size cannot be less than 0.");
            }

            Name = name;
            Size = size;
        }


        public override string ToString()
        {
            return string.Format("{0}: ({1} bytes)", Name, Size);
        }
    }


}
