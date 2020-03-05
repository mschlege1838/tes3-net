using System;
using TES3.Util;

namespace TES3.Core
{
    public class PositionRef : ICopyable<PositionRef>
    {
        const int UNITS_PER_GRID = 8192;
        const float DEGREE_CONVERSION = (float) (180 / Math.PI);

        public PositionRef(float xPos, float yPos, float zPos, float xRot, float yRot, float zRot)
        {
            XPos = xPos;
            YPos = yPos;
            ZPos = zPos;
            XRot = xRot;
            YRot = yRot;
            ZRot = zRot;
        }

        public float XPos
        {
            get;
            set;
        }

        public float YPos
        {
            get;
            set;
        }

        public float ZPos
        {
            get;
            set;
        }

        public float XRot
        {
            get;
            set;
        }

        public float XRotDegrees
        {
            get => XRot * DEGREE_CONVERSION;
            set => XRot = value / DEGREE_CONVERSION;
        }

        public float YRot
        {
            get;
            set;
        }

        public float YRotDegrees
        {
            get => YRot * DEGREE_CONVERSION;
            set => YRot = value / DEGREE_CONVERSION;
        }

        public float ZRot
        {
            get;
            set;
        }

        public float ZRotDegrees
        {
            get => ZRot * DEGREE_CONVERSION;
            set => ZRot = value / DEGREE_CONVERSION;
        }

        public int GridX
        {
            get => (int) Math.Floor(XPos / UNITS_PER_GRID);
        }

        public int GridY
        {
            get => (int) Math.Floor(YPos / UNITS_PER_GRID);
        }

        public PositionRef Copy()
        {
            return new PositionRef(XPos, YPos, ZPos, XRot, YRot, ZRot);
        }

        public override string ToString()
        {
            return $"({XPos}/{XRotDegrees} deg, {YPos}/{YRotDegrees} deg, {ZPos}/{ZRotDegrees} deg)";
        }
    }

}