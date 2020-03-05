namespace TES3.Util
{
    public static class PrintUtils
    {
        public static void PrintSquare<T>(T[,] arr, IndentWriter target)
        {
            var dim0 = arr.GetLength(0);
            var dim1 = arr.GetLength(1);

            var table = Table.Of(dim1);
            for (var i = 0; i < dim0; ++i)
            {
                var row = new object[dim1];
                for (var j = 0; j < dim1; ++j)
                {
                    row[j] = arr[i, j];
                }
                table.AddRow(row);
            }

            table.Print(target);
        }

    }
}
