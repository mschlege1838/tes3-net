
using System.Collections.Generic;

namespace TES3.Records
{
    public interface INamed
    {
        string Name { get; }
        
    }

    public interface IRecord<T> : IEnumerable<T> where T : INamed
    {
        T this[int index] { get; }

        int Count { get; }

        int GetLastIndex(string name);
    }
}
