using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TES3.Util
{
    public enum BinaryPrinterFormat
    {
        Bin, Hex, Char
    }

    public class BinaryPrinter
    {
        static readonly Encoding Enc = Encoding.ASCII;

        int bytesPerGroup = 4;
        int groupsPerRow = 4;
        public BinaryPrinterFormat Format { get; set; } = BinaryPrinterFormat.Hex;

        public int BytesPerGroup
        {
            get => bytesPerGroup;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("value", value, "Bytes Per Group must be greater than 0.");
                }
                bytesPerGroup = value;
            }
        }

        public int GroupsPerRow
        {
            get => groupsPerRow;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("value", value, "Groups Per Row must be greater than 0.");
                }
                groupsPerRow = value;
            }
        }

        public void Print(byte[] source, int start, int stop, TextWriter target)
        {
            Print(ForRange(source, start, stop), target);
        }

        public void Print(IEnumerable<byte> source, TextWriter target)
        {
            string prefix;
            int convertBase;
            int printLength;
            byte[] buf;
            switch (Format)
            {
                case BinaryPrinterFormat.Bin:
                    prefix = "0b";
                    convertBase = 2;
                    printLength = 8;
                    buf = null;
                    break;
                case BinaryPrinterFormat.Hex:
                    prefix = "0x";
                    convertBase = 16;
                    printLength = 2;
                    buf = null;
                    break;
                case BinaryPrinterFormat.Char:
                    prefix = "";
                    convertBase = -1;
                    printLength = -1;
                    buf = new byte[1];
                    break;
                default:
                    throw new InvalidOperationException($"Unexpected format: {Format}");
            }


            var byteGroupCount = 0;
            var groupRowCount = 0;
            foreach (var b in source)
            {
                if (byteGroupCount == 0)
                {
                    target.Write(prefix);
                }

                if (Format == BinaryPrinterFormat.Bin || Format == BinaryPrinterFormat.Hex)
                {
                    var s = Convert.ToString(b, convertBase);
                    var padLength = printLength - s.Length;
                    for (int i = 0; i < padLength; ++i)
                    {
                        target.Write("0");
                    }
                    target.Write(s);
                }
                else if (Format == BinaryPrinterFormat.Char)
                {
                    buf[0] = b;
                    target.Write(Enc.GetString(buf));
                }


                if (++byteGroupCount >= BytesPerGroup)
                {
                    target.Write(' ');
                    byteGroupCount = 0;
                    if (++groupRowCount >= GroupsPerRow)
                    {
                        target.WriteLine();
                        groupRowCount = 0;
                    }
                }
            }

            target.WriteLine();
        }


        static IEnumerable<byte> ForRange(byte[] arr, int start, int stop)
        {
            for (var i = start; i < stop; ++i)
            {
                yield return arr[i];
            }
        }
    }
}
