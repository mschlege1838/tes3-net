using System;
using System.Collections;
using System.Collections.Generic;

namespace TES3.Util
{
    public class ListDictionary<K, V> : IDictionary<K, IList<V>>
    {

        readonly IDictionary<K, IList<V>> backing = new Dictionary<K, IList<V>>();

        public IList<V> this[K key] 
        { 
            get
            {
                if (backing.ContainsKey(key))
                {
                    return backing[key];
                }
                else
                {
                    IList<V> result = new List<V>();
                    backing.Add(key, result);
                    return result;
                }
            }
            set => throw new NotImplementedException(); 
        }

        public ICollection<K> Keys => backing.Keys;

        public ICollection<IList<V>> Values => backing.Values;

        public int Count => backing.Count;

        public bool IsReadOnly => backing.IsReadOnly;

        public void Add(K key, IList<V> value)
        {
            throw new NotImplementedException();
        }

        public void Add(KeyValuePair<K, IList<V>> item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            backing.Clear();
        }

        public bool Contains(KeyValuePair<K, IList<V>> item)
        {
            return backing.Contains(item);
        }

        public bool ContainsKey(K key)
        {
            return backing.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<K, IList<V>>[] array, int arrayIndex)
        {
            backing.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<K, IList<V>>> GetEnumerator()
        {
            return backing.GetEnumerator();
        }

        public bool Remove(K key)
        {
            return backing.Remove(key);
        }

        public bool Remove(KeyValuePair<K, IList<V>> item)
        {
            return backing.Remove(item);
        }

        public bool TryGetValue(K key, out IList<V> value)
        {
            return backing.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
