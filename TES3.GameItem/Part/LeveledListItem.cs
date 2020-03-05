using TES3.Util;

namespace TES3.GameItem.Part
{
    public class LeveledListItem : ICopyable<LeveledListItem>
    {

        public LeveledListItem(string name, short pcLevel)
        {
            Name = name;
            PCLevel = pcLevel;
        }

        public string Name
        {
            get;
            set;
        }

        public short PCLevel
        {
            get;
            set;
        }

        public LeveledListItem Copy()
        {
            return new LeveledListItem(Name, PCLevel);
        }
    }
}
