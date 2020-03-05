using TES3.GameItem.TypeConstant;
using TES3.Util;

namespace TES3.GameItem.Part
{
    public class ApparelBipedPart : ICopyable<ApparelBipedPart>
    {

        public ApparelBipedPart(ApparelBipedPartType type, string maleBodyPart, string femaleBodyPart)
        {
            Type = type;
            MaleBodyPart = maleBodyPart;
            FemaleBodyPart = femaleBodyPart;
        }

        public ApparelBipedPartType Type
        {
            get;
            set;
        }

        public string MaleBodyPart
        {
            get;
            set;
        }

        public string FemaleBodyPart
        {
            get;
            set;
        }

        public ApparelBipedPart Copy()
        {
            return new ApparelBipedPart(Type, MaleBodyPart, FemaleBodyPart);
        }
    }
}
