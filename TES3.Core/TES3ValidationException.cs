using System;
using System.Collections.Generic;

namespace TES3.Core
{
    public class TES3ValidationException : Exception
    {

        public TES3ValidationException(IList<string> errors = null)
        {
            Errors = errors ?? new List<string>(0);
        }

        public TES3ValidationException(string error) : this(new List<string> { error })
        {
            
        }

        public IList<string> Errors
        {
            get;
        }


    }
}
