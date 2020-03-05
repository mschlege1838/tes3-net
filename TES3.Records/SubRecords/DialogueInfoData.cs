namespace TES3.Records.SubRecords
{

    public class DialogueInfoData : SubRecord
    {
        public int Unknown { get; set; }
        public int Disposition { get; set; }
        public byte Rank { get; set; }
        public byte Gender { get; set; }
        public byte PCRank { get; set; }
        public byte Unknown1 { get; set; }

        public DialogueInfoData(string name, int unknown, int disposition, byte rank, byte gender,
            byte pcRank, byte unknown1) : base(name)
        {
            Unknown = unknown;
            Disposition = disposition;
            Rank = rank;
            Gender = gender;
            PCRank = pcRank;
            Unknown1 = unknown1;
        }

        public override string ToString()
        {
            return $"{Name} (disp, rnk, gnd, pcrnk)({Disposition}, {Rank}, {Gender}, {PCRank}) / {Unknown}, {Unknown1}";
        }
    }


}