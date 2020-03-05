using System;

namespace TES3.Util.Compare
{
    public class SubRecordRestriction
    {
        const int HASH_CODE_PRIME = 31;

        public SubRecordRestriction(string recordName, string subRecordName)
        {
            RecordName = recordName ?? throw new ArgumentNullException("recordName");
            SubRecordName = subRecordName ?? throw new ArgumentNullException("subRecordName");
        }

        public string RecordName
        {
            get;
        }

        public string SubRecordName
        {
            get;
        }

        public override int GetHashCode()
        {
            int result = 1;

            result = result * HASH_CODE_PRIME + RecordName.GetHashCode();
            result = result * HASH_CODE_PRIME + SubRecordName.GetHashCode();

            return result;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (SubRecordRestriction) obj;
            return RecordName == other.RecordName && SubRecordName == other.SubRecordName;
        }

        public override string ToString()
        {
            return $"RecordName: {RecordName}; SubRecordName: {SubRecordName}";
        }
    }
}
