
using System;
using System.Collections.Generic;
using TES3.Util;


namespace TES3.Records
{

    public class NameEnumerable<TSub, TParent> : AbstractEnumeratorEnumerable<TSub> where TSub : INamed where TParent : IRecord<TSub>
    {
        readonly TParent parent;
        readonly IEnumerator<TSub> backing;
        readonly string[] names;

        internal NameEnumerable(TParent parent, string[] names)
        {
            this.parent = parent;
            backing = parent.GetEnumerator();
            this.names = names;
        }

        public int Index { get; private set; } = -1;

        public override bool MoveNext()
        {
            while (backing.MoveNext())
            {
                ++Index;
                var current = backing.Current;
                if (Array.IndexOf(names, current.Name) == -1)
                {
                    continue;
                }

                Current = current;
                return true;
            }
            return false;
        }

        public override void Reset()
        {
            backing.Reset();
            Index = -1;
        }

        public bool HasNext()
        {
            return Index + 1 < parent.Count;
        }

        public TSub PeekNext()
        {
            var next = Index + 1;
            if (next >= parent.Count)
            {
                throw new InvalidOperationException("No next element.");
            }
            return parent[next];
        }

    }

}
