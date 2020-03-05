using System.Collections.Generic;

namespace TES3.Util
{
    public static class CollectionUtils
    {

        public static void Copy<T>(IEnumerable<T> source, ICollection<T> target)
        {
            target.Clear();
            foreach (var item in source)
            {
                target.Add(item is ICopyable<T> copyable ? copyable.Copy() : item);
            }
        }

        public static void CopySquare<T>(T[,] source, T[,] target, int sideLength)
        {
            for (var i = 0; i < sideLength; ++i)
            {
                for (var j = 0; i < sideLength; ++j)
                {
                    var val = source[i, j];
                    target[i, j] = val is ICopyable<T> copyable ? copyable.Copy() : val;
                }
            }
        }

    }
}
