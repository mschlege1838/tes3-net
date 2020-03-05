using System;
using System.IO;

namespace TES3.Util
{
    public class IndentWriter
    {
        public TextWriter BackingWriter { get; }
        int indent = 0;

        public IndentWriter(TextWriter writer)
        {
            BackingWriter = writer;
        }

        public int Indent
        {
            get => indent;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value", value, "Indent must be non-negative.");
                }
                indent = value;
            }
        }

        public void IncIndent()
        {
            ++indent;
        }

        public void DecIndent()
        {
            if (indent == 0)
            {
                throw new InvalidOperationException("Indent is currently 0.");
            }
            --indent;
        }

        public void WriteLine(object o)
        {
            for (var i = 0; i < indent; ++i)
            {
                BackingWriter.Write('\t');
            }
            BackingWriter.WriteLine(o);
        }

        public void WriteLine()
        {
            BackingWriter.WriteLine();
        }

    }
}
