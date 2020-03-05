
using System;
using System.Collections;
using System.Collections.Generic;


namespace TES3.Util
{
    public class ReadOnlyList<T> : IList<T>
    {
        readonly IList<T> source;

        public ReadOnlyList(IList<T> source)
        {
            this.source = source ?? throw new ArgumentNullException("source", "Source cannot be null.");
        }

        public T this[int index]
        {
            get => source[index];
            set => throw new NotImplementedException();
        }

        public int Count
        {
            get => source.Count;
        }

        public bool IsReadOnly
        {
            get => true;
        }

        public void Add(T item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item)
        {
            return source.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            source.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return source.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return source.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return source.GetEnumerator();
        }
    }


}
