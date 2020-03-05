
using System;

namespace TES3.Records.SubRecords
{
    public class ScriptHeader : SubRecord
    {

        public ScriptHeader(string name, string scriptName, int numShorts, int numLongs, int numFloats,
                int dataSize, int localVarSize) : base(name)
        {
            ScriptName = scriptName ?? throw new ArgumentNullException("scriptName", "Script Name cannot be null.");
            NumShorts = numShorts;
            NumLongs = numLongs;
            NumFloats = numFloats;
            DataSize = dataSize;
            LocalVarSize = localVarSize;
        }

        public string ScriptName
        {
            get;
            set;
        }

        public int NumShorts
        {
            get;
            set;
        }

        public int NumLongs
        {
            get;
            set;
        }

        public int NumFloats
        {
            get;
            set;
        }

        public int DataSize
        {
            get;
            set;
        }

        public int LocalVarSize
        {
            get;
            set;
        }

        public override string ToString()
        {
            return $"{Name} {ScriptName} ({ScriptName.Length} chars)";
        }
    }



}