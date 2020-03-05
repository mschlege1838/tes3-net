namespace TES3.Util.Compare
{
    public class RecordComparison : Comparison
    {

        internal RecordComparison(string aName, string bName)
        {
            AName = aName;
            BName = bName;
        }

        public string AName
        {
            get;
        }

        public string BName
        {
            get;
        }
    }
}