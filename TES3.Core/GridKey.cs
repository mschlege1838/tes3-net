
using TES3.Util;

namespace TES3.Core
{
    public class GridKey
    {

        const int HASH_CODE_PRIME = 31;

        public GridKey(int gridX, int gridY)
        {
            GridX = gridX;
            GridY = gridY;
        }

        public int GridX
        {
            get;
        }

        public int GridY
        {
            get;
        }


        public override int GetHashCode()
        {
            int result = 1;
            result = HASH_CODE_PRIME * result + GridX;
            result = HASH_CODE_PRIME * result + GridY;
            return result;
        }

        public override bool Equals(object obj)
        {
            if (GetType() != obj.GetType())
            {
                return false;
            }

            var other = (GridKey)obj;
            return GridX == other.GridX && GridY == other.GridY;
        }

        public override string ToString()
        {
            return $"({GridX}, {GridY})";
        }

        public static bool operator ==(GridKey x, GridKey y)
        {
            return OperatorUtils.Equals(x, y);
        }

        public static bool operator !=(GridKey x, GridKey y)
        {
            return OperatorUtils.NotEquals(x, y);
        }

    }

    public class CellGridKey : GridKey
    {
        public CellGridKey(int gridX, int gridY) : base(gridX, gridY)
        {
        }
    }

    public class LandscapeGridKey : GridKey
    {
        public LandscapeGridKey(int gridX, int gridY) : base(gridX, gridY)
        {
        }
    }


}
