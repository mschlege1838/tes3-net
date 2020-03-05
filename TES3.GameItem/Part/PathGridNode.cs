using System;
using System.Collections.Generic;

using TES3.Core;
using TES3.Util;

namespace TES3.GameItem.Part
{
    public class PathGridNode : ICopyable<PathGridNode>
    {
        PathGridPoint point;
        IList<PathGridPoint> connectedPoints;

        public PathGridNode(PathGridPoint point, IList<PathGridPoint> connectedPoints)
        {
            Point = point;
            ConnectedPoints = connectedPoints;
        }

        public PathGridPoint Point
        {
            get => point;
            set => point = value ?? throw new ArgumentNullException("value", "Point cannot be null.");
        }

        public IList<PathGridPoint> ConnectedPoints
        {
            get => connectedPoints;
            set => connectedPoints = value ?? throw new ArgumentNullException("value", "Connected Points cannot be null.");
        }

        public PathGridNode Copy()
        {
            var connectedPoints = new List<PathGridPoint>(ConnectedPoints.Count);
            CollectionUtils.Copy(ConnectedPoints, connectedPoints);
            return new PathGridNode(point.Copy(), connectedPoints);
        }
    }
}
