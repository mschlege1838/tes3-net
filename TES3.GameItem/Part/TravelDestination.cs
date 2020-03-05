using TES3.Core;
using TES3.Util;

namespace TES3.GameItem.Part
{
    public class TravelDestination : ICopyable<TravelDestination>
    {

        public TravelDestination(PositionRef position, string cellName)
        {
            Position = position;
            CellName = cellName;
        }

        public PositionRef Position
        {
            get;
            set;
        }

        public string CellName
        {
            get;
            set;
        }

        public TravelDestination Copy()
        {
            return new TravelDestination(Position.Copy(), CellName);
        }
        
    }
}
