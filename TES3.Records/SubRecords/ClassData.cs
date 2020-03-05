namespace TES3.Records.SubRecords
{
    public class ClassData : SubRecord
    {
        public int AttributeId1 { get; set; }
        public int AttributeId2 { get; set; }
        public int Specialization { get; set; }
        public int MinorId1 { get; set; }
        public int MajorId1 { get; set; }
        public int MinorId2 { get; set; }
        public int MajorId2 { get; set; }
        public int MinorId3 { get; set; }
        public int MajorId3 { get; set; }
        public int MinorId4 { get; set; }
        public int MajorId4 { get; set; }
        public int MinorId5 { get; set; }
        public int MajorId5 { get; set; }
        public int Flags { get; set; }
        public int AutoCalcFlags { get; set; }

        public ClassData(string name, int attributeId1, int attributeId2, int specialization, int minorId1,
                int majorId1, int minorId2, int majorId2, int minorId3, int majorId3, int minorId4, int majorId4,
                int minorId5, int majorId5, int flags, int autoCalcFlags) : base(name)
        {
            AttributeId1 = attributeId1;
            AttributeId2 = attributeId2;
            Specialization = specialization;
            MinorId1 = minorId1;
            MajorId1 = majorId1;
            MinorId2 = minorId2;
            MajorId2 = majorId2;
            MinorId3 = minorId3;
            MajorId3 = majorId3;
            MinorId4 = minorId4;
            MajorId4 = majorId4;
            MinorId5 = minorId5;
            MajorId5 = majorId5;
            Flags = flags;
            AutoCalcFlags = autoCalcFlags;
        }

        public override string ToString()
        {
            return $"{Name} (spec)({Specialization}) (attributes)({AttributeId1}, {AttributeId2}) (major)({MajorId1}, {MajorId2}, {MajorId3}, {MajorId4}, {MajorId5}) " +
                $"(minor)({MinorId1}, {MinorId2}, {MinorId3}, {MinorId4}, {MinorId5}) (flag, auto)({Flags:X8}, {AutoCalcFlags:X8})";
        }

    }


}