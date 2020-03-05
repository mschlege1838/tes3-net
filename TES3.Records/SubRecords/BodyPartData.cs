namespace TES3.Records.SubRecords
{
    public class BodyPartData : SubRecord
    {
        public byte Part { get; set; }
        public byte Vampire { get; set; }
        public byte Flags { get; set; }
        public byte PartType { get; set; }

        public BodyPartData(string name, byte part, byte vampire, byte flags, byte partType) : base(name)
        {
            Part = part;
            Vampire = vampire;
            Flags = flags;
            PartType = partType;
        }

        public override string ToString()
        {
            return $"{Name} (prt, vmpr, flg, typ)({Part}, {Vampire}, {Flags:X2}, {PartType})";
        }
    }


}