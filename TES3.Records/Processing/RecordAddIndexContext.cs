using System;
using System.Collections.Generic;
using System.Text;

namespace TES3.Records.Processing
{
    class RecordAddIndexContext : IRecordOperationContext
    {

        internal readonly ModFile modFile;

        internal RecordAddIndexContext(ModFile modFile, string recordName)
        {
            this.modFile = modFile;
            RecordName = recordName;
        }

        public string RecordName
        {
            get;
        }
    }
}
