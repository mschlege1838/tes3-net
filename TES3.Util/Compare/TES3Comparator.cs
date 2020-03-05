
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace TES3.Util.Compare
{
    public class TES3Comparator
    {

        static readonly ISet<string> RecordNames = new HashSet<string> { "TES3", "GMST", "GLOB", "CLAS", "FACT", "RACE", "SOUN", "SKIL", "MGEF", "SCPT", "REGN", "SSCR", "BSGN",
            "LTEX", "STAT", "DOOR", "MISC", "WEAP", "CONT", "SPEL", "CREA", "BODY", "LIGH", "ENCH", "NPC_", "ARMO", "CLOT", "REPA", "ACTI", "APPA", "LOCK", "PROB", "INGR", "BOOK",
            "ALCH", "LEVI", "LEVC", "CELL", "LAND", "PGRD", "SNDG", "DIAL", "INFO" };

        static readonly ISet<string> SubRecordNames = new HashSet<string> { "HEDR", "MAST", "DATA", "NAME", "STRV", "INTV", "FLTV", "DELE", "FNAM", "CLDT", "DESC", "RNAM", "FADT",
            "ANAM", "RADT", "NPCS", "INDX", "SKDT", "MEDT", "ITEX", "PTEX", "BSND", "CSND", "HSND", "ASND", "CVFX", "BVFX", "HVFX", "AVFX", "SCHD", "SCVR", "SCDT", "SCTX", "WEAT",
            "BNAM", "CNAM", "SNAM", "TNAM", "MODL", "SCRI", "MCDT", "WPDT", "ENAM", "CNDT", "FLAG", "NPCO", "SPDT", "NPDT", "XSCL", "AIDT", "DODT", "DNAM", "AI_W", "AI_A", "AI_T",
            "AI_E", "AI_F", "BYDT", "LHDT", "ENDT", "KNAM", "AODT", "CTDT", "RIDT", "AADT", "LKDT", "PBDT", "IRDT", "BKDT", "TEXT", "ALDT", "NNAM", "INAM", "RGNN", "WHGT", "AMBI",
            "NAM0", "MVRF", "FRMR", "NAM5", "NAM9", "XSOL", "XCHG", "UNAM", "VNML", "VHGT", "WNAM", "VCLR", "VTEX", "PGRP", "PGRC", "PNAM", "ONAM", "QSTN", "QSTF", "QSTR" };


        static readonly IDictionary<SubRecordName, int> NullTerminatedSubRecords = new Dictionary<SubRecordName, int>
        {
            [new SubRecordName("REGN", "SNAM")] = 0,
            [new SubRecordName("NPC_", "AI_F")] = 0,
            [new SubRecordName("CREA", "AI_F")] = 15,
            [new SubRecordName("CREA", "AI_E")] = 15
        };

        static readonly Encoding textEncoding = Encoding.GetEncoding("Windows-1252");



        int columns;
        int columnWidth;

        string datumFormat;
        string textFormat;
        string blankValue;
        string noneValue;

        public TES3Comparator()
        {
            Columns = 8;
            ColumnWidth = 5;
        }


        public int Columns
        {
            get => columns;
            set => columns = value < 0 ? throw new ArgumentOutOfRangeException("value", value, $"Columns must be greater than 0: {value}") : value;
        }

        public int ColumnWidth
        {
            get => columnWidth;
            set
            {
                if (value < 5)
                {
                    throw new ArgumentOutOfRangeException("value", value, $"Column Width must be at least 5: {value}");
                }

                columnWidth = value;

                datumFormat = $"0x{{0,-{value - 2}:X2}}";
                textFormat = $"{{0,-{value}}}";
                blankValue = string.Format($".{{0,-{value - 1}}}", "");
                noneValue = string.Format($"{{0,-{value}}}", "");
            }
        }

        public IList<Comparison> Compare(FileInfo a, FileInfo b, ComparisonRestriction restriction = null)
        {
            IList<RawRecord> aRecords;
            using (var stream = a.OpenRead())
            {
                aRecords = ReadRaw(stream, restriction);
            }

            IList<RawRecord> bRecords;
            using (var stream = b.OpenRead())
            {
                bRecords = ReadRaw(stream, restriction);
            }

            return Compare(aRecords, bRecords);
        }

        IList<Comparison> Compare(IList<RawRecord> a, IList<RawRecord> b)
        {
            var result = new List<Comparison>();

            var recordsLen = Math.Max(a.Count, b.Count);
            for (var i = 0; i < recordsLen; ++i)
            {
                var aRecord = i < a.Count ? a[i] : null;
                var bRecord = i < b.Count ? b[i] : null;


                var recordComparison = new RecordComparison(aRecord?.name, bRecord?.name);
                Compare0(aRecord?.data, bRecord?.data, recordComparison, -1);
                if (recordComparison.HasAnomoly)
                {
                    result.Add(recordComparison);
                }


                var aSubRecords = aRecord?.subRecords;
                var bSubRecords = bRecord?.subRecords;
                var subRecordsLen = Math.Max(aSubRecords == null ? 0 : aSubRecords.Count, bSubRecords == null ? 0 : bSubRecords.Count);
                for (var j = 0; j < subRecordsLen; ++j)
                {
                    var aSubRecord = aSubRecords != null && j < aSubRecords.Count ? aSubRecords[j] : null;
                    var bSubRecord = bSubRecords != null && j < bSubRecords.Count ? bSubRecords[j] : null;

                    var subRecordComparison = new SubRecordComparison(aRecord?.name, aSubRecord?.name, bRecord?.name, bSubRecord?.name);

                    var aName = aRecord != null && aSubRecord != null ? new SubRecordName(aRecord.name, aSubRecord.name) : null;
                    var bName = bRecord != null && bSubRecord != null ? new SubRecordName(bRecord.name, bSubRecord.name) : null;
                    var nullTerminatorIndex = aName != null && bName != null && NullTerminatedSubRecords.ContainsKey(aName) && NullTerminatedSubRecords.ContainsKey(bName) ? NullTerminatedSubRecords[aName] : -1;

                    Compare0(aSubRecord?.data, bSubRecord?.data, subRecordComparison, nullTerminatorIndex);
                    if (subRecordComparison.HasAnomoly)
                    {
                        result.Add(subRecordComparison);
                    }
                }
            }


            return result;
        }

        void Compare0(byte[] aData, byte[] bData, Comparison comparison, int nullTerminatedIndex)
        {
            var noA = aData == null;
            var noB = bData == null;

            var len = Math.Max(noA ? 0 : aData.Length, noB ? 0 : bData.Length);
            var rows = len / columns;
            if (len % columns > 0)
            {
                rows += 1;
            }

            var aBin = new string[rows, columns];
            var aText = new string[rows, columns];
            var bBin = new string[rows, columns];
            var bText = new string[rows, columns];
            var format = new RecordFormat[rows, columns];

            var terminatorIndex = -1;

            var buf = new byte[1];
            var column = 0;
            var row = 0;
            for (var i = 0; i < len; ++i)
            {
                if (column >= columns)
                {
                    column = 0;
                    ++row;
                }

                if (noA || i >= aData.Length)
                {
                    aBin[row, column] = blankValue;
                    aText[row, column] = blankValue;
                    bBin[row, column] = string.Format(datumFormat, buf[0] = bData[i]);
                    bText[row, column] = string.Format(textFormat, textEncoding.GetString(buf));
                    format[row, column] = RecordFormat.MissingInA;
                }
                else if (noB || i >= bData.Length)
                {
                    aBin[row, column] = string.Format(datumFormat, buf[0] = aData[i]);
                    aText[row, column] = string.Format(textFormat, textEncoding.GetString(buf));
                    bBin[row, column] = blankValue;
                    bText[row, column] = blankValue;
                    format[row, column] = RecordFormat.MissingInB;
                }
                else
                {
                    var aVal = aData[i];
                    var bVal = bData[i];

                    aBin[row, column] = string.Format(datumFormat, buf[0] = aVal);
                    aText[row, column] = string.Format(textFormat, textEncoding.GetString(buf));
                    bBin[row, column] = string.Format(datumFormat, buf[0] = bVal);
                    bText[row, column] = string.Format(textFormat, textEncoding.GetString(buf));

                    if (nullTerminatedIndex != -1 && i >= nullTerminatedIndex)
                    {
                        if (terminatorIndex == -1 && (aVal == 0 || bVal == 0))
                        {
                            terminatorIndex = i;
                            format[row, column] = aVal == bVal ? RecordFormat.Normal : RecordFormat.Mismatch;
                        }
                        else if (terminatorIndex != -1)
                        {
                            format[row, column] = aVal == bVal ? RecordFormat.Normal : RecordFormat.Ok;
                        }
                        else
                        {
                            format[row, column] = aVal == bVal ? RecordFormat.Normal : RecordFormat.Mismatch;
                        }
                    }
                    else
                    {
                        format[row, column] = aVal == bVal ? RecordFormat.Normal : RecordFormat.Mismatch;
                    }

                    
                    
                }

                ++column;
            }

            var lastRow = rows - 1;
            for (var i = 0; i < columns; ++i)
            {
                if (aBin[lastRow, i] == null)
                {
                    aBin[lastRow, i] = noneValue;
                    aText[lastRow, i] = noneValue;
                }
                if (bBin[lastRow, i] == null)
                {
                    bBin[lastRow, i] = noneValue;
                    bText[lastRow, i] = noneValue;
                }
            }

            comparison.ABin = aBin;
            comparison.AText = aText;
            comparison.ALength = noA ? 0 : aData.Length;
            comparison.BBin = bBin;
            comparison.BText = bText;
            comparison.BLength = noB ? 0 : bData.Length;
            comparison.Format = format;
        }

        static IList<RawRecord> ReadRaw(Stream stream, ComparisonRestriction restriction)
        {
            var buf = new byte[8];


            var result = new List<RawRecord>();
            var recordIndex = 0;

            RawRecord prevRecord = null;
            int bytes;
            while ((bytes = stream.Read(buf, 0, 8)) != 0)
            {
                if (bytes != 8)
                {
                    throw new InvalidOperationException(bytes.ToString());
                }
                var recordName = textEncoding.GetString(buf, 0, 4);
                

                var recordSize = BitConverter.ToInt32(buf, 4);
                if (!RecordNames.Contains(recordName))
                {
                    string nameVal = null;
                    if (prevRecord != null)
                    {
                        var names = (from sr in prevRecord.subRecords where sr.name == "NAME" select textEncoding.GetString(sr.data)).ToList();
                        if (names.Count > 0)
                        {
                            nameVal = names[0];
                        }
                    }
                    Console.WriteLine($"R: ({prevRecord.name} {nameVal})  {recordName} {recordSize}");
                    throw new InvalidOperationException();
                }

                var recordData = new byte[16];
                Array.Copy(buf, 0, recordData, 0, 8);
                stream.Read(recordData, 8, 8);


                var subRecords = new List<RawSubRecord>();
                if (restriction == null || restriction.ShouldInclude(recordName))
                {
                    result.Add(prevRecord = new RawRecord(recordName, recordData, subRecords, recordIndex));
                }
                ++recordIndex;

                
                var bytesRead = 0;
                var prev = "";
                while (bytesRead < recordSize)
                {
                    bytesRead += stream.Read(buf, 0, 8);

                    var subRecordName = textEncoding.GetString(buf, 0, 4);
                    var subRecordSize = BitConverter.ToInt32(buf, 4);
                    if (!SubRecordNames.Contains(subRecordName))
                    {

                        Console.WriteLine($"({prev})  {recordName}.{subRecordName} {subRecordSize}");
                        throw new InvalidOperationException();
                    }

                    var subRecordData = new byte[subRecordSize + 8];
                    Array.Copy(buf, 0, subRecordData, 0, 8);
                    bytesRead += stream.Read(subRecordData, 8, subRecordSize);


                    
                    if (restriction == null || restriction.ShouldInclude(new SubRecordRestriction(recordName, subRecordName)))
                    {
                        subRecords.Add(new RawSubRecord(subRecordName, subRecordData));
                        prev = subRecordName;
                    }
                }

                
            }

            return result;
        }

      



        class RawRecord
        {
            internal readonly string name;
            internal readonly byte[] data;
            internal readonly IList<RawSubRecord> subRecords;
            internal readonly int index;

            internal RawRecord(string name, byte[] data, IList<RawSubRecord> subRecords, int index)
            {
                this.name = name;
                this.data = data;
                this.subRecords = subRecords;
                this.index = index;
            }
        }

        class RawSubRecord
        {
            internal readonly string name;
            internal readonly byte[] data;

            internal RawSubRecord(string name, byte[] data)
            {
                this.name = name;
                this.data = data;
            }
        }

        class SubRecordName
        {
            const int HASH_CODE_PRIME = 31;

            string recordName;
            string subRecordName;

            internal SubRecordName(string recordName, string subRecordName)
            {
                this.recordName = recordName;
                this.subRecordName = subRecordName;
            }

            public override int GetHashCode()
            {
                var result = 1;

                result = result * HASH_CODE_PRIME + recordName.GetHashCode();
                result = result * HASH_CODE_PRIME * subRecordName.GetHashCode();

                return result;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(this, obj))
                {
                    return true;
                }

                if (obj == null || GetType() != obj.GetType())
                {
                    return false;
                }

                var other = (SubRecordName) obj;
                return recordName == other.recordName && subRecordName == other.subRecordName;
            }
        }

    }
}