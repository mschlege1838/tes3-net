
using System;
using System.IO;
using System.Text;


namespace TES3.Core
{
    public static class GlobalConfig
    {

        static Encoding textEncoding;

        public static bool TrimNullChars
        {
            get;
            set;
        } = true;

        public static Encoding TextEncoding
        {
            get
            {
                if (textEncoding == null)
                {
                    textEncoding = Encoding.GetEncoding("Windows-1252");
                }
                return textEncoding;
            }
        }

        public static TextWriter InfoWriter
        {
            get;
            set;
        } = Console.Out;

        public static TextWriter WarnWriter
        {
            get;
            set;
        } = Console.Error;

        public static TextWriter ErrorWriter
        {
            get;
            set;
        } = Console.Error;
        
    }


}
