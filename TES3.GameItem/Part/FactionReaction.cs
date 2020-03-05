using TES3.Util;

namespace TES3.GameItem.Part
{
    public class FactionReaction : ICopyable<FactionReaction>
    {

        public FactionReaction(string name, int reaction)
        {
            Name = name;
            Reaction = reaction;
        }

        public string Name
        {
            get;
            set;
        }

        public int Reaction
        {
            get;
            set;
        }

        public FactionReaction Copy()
        {
            return new FactionReaction(Name, Reaction);
        }
    }

}
