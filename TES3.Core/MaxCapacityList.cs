using System;
using System.Collections;
using System.Collections.Generic;

using TES3.Util;

namespace TES3.Core
{
    public class MaxCapacityList<T> : IList<T>
    {

        readonly IList<T> backing;
        readonly int max;
        readonly string fieldName;
        readonly bool strict;


        public MaxCapacityList(int max, string fieldName, bool strict = true)
        {
            this.fieldName = Validation.NotNull(fieldName, "fieldName");
            if (max < 0)
            {
                throw new ArgumentOutOfRangeException("max", max, $"Max capacity cannot be less than 0: {max}");
            }

            backing = new List<T>(max);
            this.max = max;
            this.strict = strict;
        }

        public MaxCapacityList(MaxCapacityList<T> other)
        {
            fieldName = other.fieldName;
            max = other.max;
            strict = other.strict;
            backing = new List<T>(other.backing);
        }

        public T this[int index]
        {
            get => backing[index];
            set => backing[index] = value;
        }

        public int Count
        {
            get => backing.Count;
        }

        public bool IsReadOnly
        {
            get => backing.IsReadOnly;
        }

        public void Add(T item)
        {
            CheckCap();
            backing.Add(item);
        }

        public void Clear()
        {
            backing.Clear();
        }

        public bool Contains(T item)
        {
            return backing.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            backing.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return backing.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            CheckCap();
            backing.Insert(index, item);
        }

        public bool Remove(T item)
        {
            return backing.Remove(item);
        }

        public void RemoveAt(int index)
        {
            RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        void CheckCap()
        {
            if (backing.Count >= max)
            {
                if (strict)
                {
                    throw new InvalidOperationException($"Unable to add elements to list beyond max capacity: {max}");
                }
                else
                {
                    TES3Logger.Log(TES3LogLevel.Warn, "The field {0} is limited to {2} items. This and subsequent items will likely be ignored by the game.", fieldName, max);
                }
            }
        }
    }
}
