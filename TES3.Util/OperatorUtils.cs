
namespace TES3.Util
{
    public static class OperatorUtils
    {
        public static bool Equals<T>(T x, T y) where T : class
        {
            return ReferenceEquals(x, y) || !(x is null) && x.Equals(y);
        }

        public static bool NotEquals<T>(T x, T y) where T : class
        {
            return ReferenceEquals(x, y) ? false : x is null || !x.Equals(y);
        }
    }
}
