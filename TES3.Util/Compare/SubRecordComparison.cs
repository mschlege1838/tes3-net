namespace TES3.Util.Compare
{
    public class SubRecordComparison : Comparison
    {

        internal SubRecordComparison(string aRecordName, string aSubRecordName, string bRecordName, string bSubRecordName)
        {
            ARecordName = aRecordName;
            ASubRecordName = aSubRecordName;
            BRecordName = bRecordName;
            BSubRecordName = bSubRecordName;
        }

        public string ARecordName
        {
            get;
        }

        public string ASubRecordName
        {
            get;
        }

        public string BRecordName
        {
            get;
        }

        public string BSubRecordName
        {
            get;
        }

    }
}