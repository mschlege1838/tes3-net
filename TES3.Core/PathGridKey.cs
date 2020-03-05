using System;
using TES3.Util;

namespace TES3.Core
{
    public class PathGridKey
    {

        const int HASH_CODE_PRIME = 31;

        public PathGridKey(string name, GridKey grid)
        {
            Name = name ?? throw new ArgumentNullException("name", "Name cannot be null.");
            Grid = grid ?? throw new ArgumentNullException("grid", "Grid cannot be null.");
        }

        public string Name
        {
            get;
        }

        public GridKey Grid
        {
            get;
        }

        public override int GetHashCode()
        {
            int result = 1;
            result = result * HASH_CODE_PRIME + Name.GetHashCode();
            result = result * HASH_CODE_PRIME + Grid.GetHashCode();
            return result;
        }

        public override bool Equals(object obj)
        {
            if (GetType() != obj.GetType())
            {
                return false;
            }

            var other = (PathGridKey) obj;
            return Name.Equals(other.Name) && Grid.Equals(other.Grid);
        }

        public override string ToString()
        {
            return $"{Name} {Grid}";
        }

        public static bool operator ==(PathGridKey x, PathGridKey y)
        {
            return OperatorUtils.Equals(x, y);
        }

        public static bool operator !=(PathGridKey x, PathGridKey y)
        {
            return OperatorUtils.NotEquals(x, y);
        }
    }

}
