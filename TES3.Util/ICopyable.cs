using System;
using System.Collections.Generic;
using System.Text;

namespace TES3.Util
{
    public interface ICopyable<T>
    {
        T Copy();
    }
}
