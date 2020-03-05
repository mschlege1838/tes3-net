using System;

namespace TES3.GameItem.Part
{
    public abstract class DamageRange<T> where T : struct, IComparable<T>
    {

        protected abstract bool IsNegative(T value);

        public T Min
        {
            get;
            private set;
        }

        public T Max
        {
            get;
            private set;
        }

        public void SetRange(T min, T max)
        {
            if (IsNegative(min))
            {
                throw new ArgumentOutOfRangeException("value", min, $"Min Damage cannot be negative: {min}");
            }
            if (IsNegative(max))
            {
                throw new ArgumentOutOfRangeException("value", max, $"Max Damage cannot be negative: {max}");
            }
            if (min.CompareTo(max) > 0)
            {
                throw new ArgumentOutOfRangeException("value", min, $"Min Damage cannot be greater than max ({max}): {min}");
            }
            if (max.CompareTo(min) < 0)
            {
                throw new ArgumentOutOfRangeException("value", max, $"Max Damage cannot be less than min ({min}): {max}");
            }

            Min = min;
            Max = max;
        }

        public void SetRange(DamageRange<T> other)
        {
            SetRange(other.Min, other.Max);
        }
    }


}
