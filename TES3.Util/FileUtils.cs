
using System.IO;

namespace TES3.Util
{
    public static class FileUtils
    {
        static readonly char[] WildCardChars = new char[] { '*', '?' };

        public static string[] ResolveWildcard(string path)
        {
            var wildcardIndex = path.IndexOfAny(WildCardChars);
            if (wildcardIndex == -1)
            {
                return new string[] { path };
            }

            return Directory.GetFiles(Path.GetDirectoryName(path), Path.GetFileName(path));
            /*
            var separatorIndex = path.LastIndexOf('\\', wildcardIndex);
            if (separatorIndex == -1)
            {
                var result = Directory.GetFiles(".", path);
                for (var i = 0; i < result.Length; ++i)
                {
                    var fname = result[i];
                    result[i] = fname.Substring(2);
                }
                return result;
            }

            var pos = separatorIndex + 1;
            var dirPath = path.Substring(0, pos);
            var searchString = path.Substring(pos);
            return Directory.GetFiles(dirPath, searchString);
            */
        }
    }
}
