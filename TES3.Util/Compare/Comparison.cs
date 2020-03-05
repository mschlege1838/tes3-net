namespace TES3.Util.Compare
{
    public abstract class Comparison
    {

        public string[,] ABin
        {
            get;
            internal set;
        }

        public string[,] AText
        {
            get;
            internal set;
        }

        public int ALength
        {
            get;
            internal set;
        }

        public string[,] BBin
        {
            get;
            internal set;
        }

        public string[,] BText
        {
            get;
            internal set;
        }

        public int BLength
        {
            get;
            internal set;
        }

        public RecordFormat[,] Format
        {
            get;
            internal set;
        }

        public int Rows
        {
            get => ABin.GetLength(0);
        }

        public int Columns
        {
            get => ABin.GetLength(1);
        }

        internal bool HasAnomoly
        {
            get
            {
                foreach (var f in Format)
                {
                    if (f != RecordFormat.Normal && f != RecordFormat.Ok)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
    }
}