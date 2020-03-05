
using System.Collections;
using System.Collections.Generic;

namespace TES3.Util
{
    public abstract class AbstractEnumerator<T> : IEnumerator<T>
    {
        public T Current
        {
            get;
            protected set;
        }

        object IEnumerator.Current
        {
            get => Current;
        }

        public virtual void Dispose()
        {

        }

        public abstract bool MoveNext();

        public abstract void Reset();
    }

    public abstract class AbstractEnumeratorEnumerable<T> : AbstractEnumerator<T>, IEnumerable<T>
    {
        public IEnumerator<T> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public abstract class AbstractEnumerable<T> : IEnumerable<T>
    {
        public abstract IEnumerator<T> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
