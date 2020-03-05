using System;

namespace TES3.Util
{
    public static class Validation
    {

        public static T[,] ValidateSquare<T>(T[,] arr, int sideLength, string paramName, string displayName = null, bool optional = false)
        {
            if (displayName == null)
            {
                displayName = paramName;
            }

            if (arr == null)
            {
                if (optional)
                {
                    return arr;
                }
                throw new ArgumentNullException(paramName, $"{displayName} cannot be null.");
            }
            if (arr.GetLength(0) != sideLength)
            {
                throw new ArgumentOutOfRangeException(paramName, arr.GetLength(0), $"{displayName} must have exactly {sideLength} rows.");
            }
            if (arr.GetLength(1) != sideLength)
            {
                throw new ArgumentOutOfRangeException(paramName, arr.GetLength(1), $"{displayName} must have exactly {sideLength} columns.");
            }

            return arr;
        }

        public static T Range<T>(T value, T low, T high, string parameterName, string displayName) where T : IComparable<T>
        {
            if (displayName == null)
            {
                displayName = parameterName;
            }
            if (value.CompareTo(low) < 0)
            {
                throw new ArgumentOutOfRangeException(parameterName, value, $"{displayName} cannot be less than {low}.");
            }
            if (value.CompareTo(high) > 0)
            {
                throw new ArgumentOutOfRangeException(parameterName, value, $"{displayName} cannot be greather than {high}.");
            }

            return value;
        }
        public static T NotNull<T>(T value, string parameterName, string displayName = null)
        {
            if (displayName == null)
            {
                displayName = parameterName;
            }
            if (value == null)
            {
                throw new ArgumentNullException(parameterName, $"{displayName} cannot be null.");
            }

            return value;
        }

        public static string Length(string value, int max, string parameterName, string displayName = null, bool checkNull = false)
        {
            if (checkNull)
            {
                NotNull(value, parameterName, displayName);
            }
            else if (value == null)
            {
                return null;
            }

            if (displayName == null)
            {
                displayName = parameterName;
            }

            if (value.Length > max)
            {
                throw new ArgumentOutOfRangeException(parameterName, value.Length, $"{displayName} cannot have greater than {max} characters: {value} ({value.Length})");
            }

            return value;
        }

        public static T Gte<T>(T value, T target, string parameterName, string displayName = null) where T : IComparable<T>
        {
            if (displayName == null)
            {
                displayName = parameterName;
            }
            if (value.CompareTo(target) < 0)
            {
                throw new ArgumentOutOfRangeException(parameterName, value, $"{displayName} must be greater than or equal to {target}: {value}");
            }
            return value;
        }
    }
}
