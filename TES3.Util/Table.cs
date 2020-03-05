using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace TES3.Util
{
    public enum TableSortType
    {
        String,
        Int,
        Float,
        Length
    }

    public class Table
    {
        public static readonly object RowIndex = new object();

        const int COLUMN_SPACING = 2;
        const double NUMERIC_COMPARE_DIFF = 0.000001;

        public static Table Of(int columnCount)
        {
            var columnNames = new string[columnCount];
            for (var i = 0; i < columnCount; ++i)
            {
                columnNames[i] = "";
            }
            return new Table(columnNames);
        }

        public static Table Reflect<T>(IEnumerable<T> q)
        {
            Table table = null;
            List<PropertyInfo> props = null;
            foreach (var o in q)
            {
                if (table == null)
                {
                    props = (from member in o.GetType().GetMembers()
                             where member.MemberType == MemberTypes.Property
                             select (PropertyInfo) member).ToList()
                             ;

                    var columnNames = new string[props.Count];
                    var i = 0;
                    foreach (var prop in props)
                    {
                        columnNames[i++] = prop.Name;
                    }
                    table = new Table(columnNames);
                }

                {
                    var row = new object[props.Count];
                    var i = 0;
                    foreach (var prop in props)
                    {
                        row[i++] = prop.GetValue(o);
                    }
                    table.AddRow(row);
                }
            }

            return table ?? Of(0);
        }

        readonly string[] columnNames;
        readonly List<string[]> data = new List<string[]>();
        readonly int[] columnLengths;

        public Table(params string[] columnNames)
        {
            this.columnNames = columnNames;
            columnLengths = new int[columnNames.Length];

            for (var i = 0; i < columnNames.Length; ++i)
            {
                columnLengths[i] = columnNames[i].Length + COLUMN_SPACING;
            }
        }

        public int Count
        {
            get => data.Count;
        }

        public void AddRow<T>(T[] row) where T : struct
        {
            if (row.Length != columnNames.Length)
            {
                throw new ArgumentException($"{columnNames.Length} columns in this table: {row.Length}");
            }

            var strRow = new string[row.Length];
            for (var i = 0; i < row.Length; ++i)
            {
                var str = strRow[i] = row[i].ToString();

                var testLength = str.Length + COLUMN_SPACING;

                if (testLength > columnLengths[i])
                {
                    columnLengths[i] = testLength;
                }
            }

            data.Add(strRow);
        }


        public void AddRow(params object[] row)
        {
            if (row.Length != columnNames.Length)
            {
                throw new ArgumentException($"{columnNames.Length} columns in this table: {row.Length}");
            }

            var strRow = new string[row.Length];
            for (var i = 0; i < row.Length; ++i)
            {
                var value = row[i];
                if (value == RowIndex)
                {
                    value = data.Count;
                }

                var str = strRow[i] = value == null ? "<NULL>" : value.ToString();

                var testLength = str.Length + COLUMN_SPACING;

                if (testLength > columnLengths[i])
                {
                    columnLengths[i] = testLength;
                }
            }

            data.Add(strRow);
        }

        public void Print(IndentWriter writer)
        {
            Print(writer.BackingWriter, writer.Indent);
        }

        public void Print(TextWriter writer, int indentSize = 0)
        {
            for (int i = 0; i < indentSize; ++i)
            {
                writer.Write('\t');
            }

            for (var i = 0; i < columnNames.Length; ++i)
            {
                var name = columnNames[i];
                writer.Write(name);
                int spaces = columnLengths[i] - name.Length;
                for (var j = 0; j < spaces; ++j)
                {
                    writer.Write(' ');
                }
            }
            writer.WriteLine();


            foreach (var row in data)
            {
                for (int i = 0; i < indentSize; ++i)
                {
                    writer.Write('\t');
                }
                for (var i = 0; i < row.Length; ++i)
                {
                    var value = row[i];

                    writer.Write(value);
                    int spaces = columnLengths[i] - value.Length;
                    for (var j = 0; j < spaces; ++j)
                    {
                        writer.Write(' ');
                    }
                }
                writer.WriteLine();
            }
        }

        public void Sort(params SortParameter[] parameters)
        {
            for (var i = 0; i < parameters.Length; ++i)
            {
                var parameter = parameters[i];

                if (parameter.ColumnIndex < 0 || parameter.ColumnIndex >= columnNames.Length)
                {
                    throw new ArgumentOutOfRangeException("parameters", parameter.ColumnIndex, $"Invalid Column Index at parameter {i}: {parameter.ColumnIndex}");
                }
            }

            data.Sort(new SortComparer(parameters));
        }

        public SortParameter SortParam(string columnName, TableSortType sortType = TableSortType.String, bool ascendingOrder = true)
        {
            if (columnName == null)
            {
                throw new ArgumentNullException("columnName", "Column Name cannot be null.");
            }

            var columnIndex = Array.IndexOf(columnNames, columnName);
            if (columnIndex == -1)
            {
                throw new ArgumentException($"Column Name not found: {columnName}");
            }

            return SortParam(columnIndex, sortType, ascendingOrder);
        }

        public SortParameter SortParam(int columnIndex, TableSortType sortType = TableSortType.String, bool ascendingOrder = true)
        {
            return new SortParameter(columnIndex, sortType, ascendingOrder);
        }


        public class SortParameter
        {
            public SortParameter(int columnIndex, TableSortType sortType, bool ascendingOrder)
            {
                ColumnIndex = columnIndex;
                SortType = sortType;
                AscendingOrder = ascendingOrder;
            }

            public int ColumnIndex
            {
                get;
                set;
            } 

            public TableSortType SortType
            {
                get;
                set;
            }

            public bool AscendingOrder
            {
                get;
                set;
            }
        }

        class SortComparer : IComparer<string[]>
        {
            readonly SortParameter[] parameters;

            internal SortComparer(SortParameter[] parameters)
            {
                this.parameters = parameters;
            }

            public int Compare(string[] rowA, string[] rowB)
            {
                foreach (var parameter in parameters)
                {
                    var columnIndex = parameter.ColumnIndex;

                    int diff;
                    switch (parameter.SortType)
                    {
                        case TableSortType.String:
                            diff = rowA[columnIndex].CompareTo(rowB[columnIndex]);
                            break;
                        case TableSortType.Int:
                            diff = int.Parse(rowA[columnIndex]) - int.Parse(rowB[columnIndex]);
                            break;
                        case TableSortType.Float:
                            var fltDiff = double.Parse(rowA[columnIndex]) - double.Parse(rowB[columnIndex]);
                            if (Math.Abs(fltDiff) <= NUMERIC_COMPARE_DIFF)
                            {
                                diff = 0;
                            }
                            else
                            {
                                diff = fltDiff < 0 ? -1 : 1;
                            }
                            break;
                        case TableSortType.Length:
                            var valA = rowA[columnIndex];
                            var valB = rowB[columnIndex];
                            var lengthDiff = valA.Length - valB.Length;
                            diff = lengthDiff != 0 ? lengthDiff : valA.CompareTo(valB);
                            break;
                        default:
                            throw new InvalidOperationException($"Unsupported sort type: parameter.SortType");
                    }

                    if (diff != 0)
                    {
                        return parameter.AscendingOrder ? diff : -1 * diff;
                    }

                }

                return 0;
            }
        }
    }
}
