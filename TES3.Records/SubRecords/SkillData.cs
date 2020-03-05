
using System;

namespace TES3.Records.SubRecords
{
    public class SkillData : SubRecord
    {
        public int Attribute { get; set; }
        public int Specialization { get; set; }
        float[] useValue;

        public SkillData(string name, int attribute, int specialization, float[] useValue) : base(name)
        {
            Attribute = attribute;
            Specialization = specialization;
            UseValue = useValue;
        }

        public float[] UseValue
        {
            get => useValue;
            set => useValue = value ?? throw new ArgumentNullException("useValue", "Use Value cannot be null.");
        }

        public override string ToString()
        {
            return $"{Name} attr,spec({Attribute}, {Specialization})";
        }
    }


}