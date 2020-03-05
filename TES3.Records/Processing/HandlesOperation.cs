using System;
using System.Collections.Generic;
using System.Text;

namespace TES3.Records.Processing
{
    [AttributeUsage(AttributeTargets.Method)]
    class HandlesOperation : Attribute
    {

        internal HandlesOperation(SubRecordOperationType operation)
        {
            SubRecordOperation = operation;
        }

        internal HandlesOperation(RecordOperationType operation)
        {
            RecordOperation = operation;
        }

        internal SubRecordOperationType SubRecordOperation
        {
            get;
        }

        internal RecordOperationType RecordOperation
        {
            get;
        }
    }
}
