using TES3.Core;

namespace TES3.Records.SubRecords
{
    public class InteriorLightSubRecord : SubRecord
    {
        public ColorRef AmbientColor { get; set; }
        public ColorRef SunlightColor { get; set; }
        public ColorRef FogColor { get; set; }
        public float FogDensity { get; set; }

        public InteriorLightSubRecord(string name, ColorRef ambientColor, ColorRef sunlightColor, ColorRef fogColor,
            float fogDensity) : base(name)
        {
            AmbientColor = ambientColor;
            SunlightColor = sunlightColor;
            FogColor = fogColor;
            FogDensity = fogDensity;
        }

        public override string ToString()
        {
            return $"{Name} fog_dsnty({FogDensity}) (amb, sun, fog)({AmbientColor}, {SunlightColor}, {FogColor})";
        }
    }


}