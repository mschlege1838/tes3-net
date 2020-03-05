
using System;

namespace TES3.GameItem.TypeConstant
{
    public enum ArmorType
    {
        Helmet,
        Cuirass,
        LeftPauldron,
        RightPauldron,
        Greaves,
        Boots,
        LeftGauntlet,
        RightGauntlet,
        Shield,
        LeftBracer,
        RightBracer
    }


    public static class ArmorTypeExtensions
    {
        public static float GetLightCutoff(this ArmorType armorType)
        {
            switch (armorType)
            {
                case ArmorType.Helmet:
                    return 3f;
                case ArmorType.Cuirass:
                    return 18f;
                case ArmorType.LeftPauldron:
                case ArmorType.RightPauldron:
                    return 6f;
                case ArmorType.Greaves:
                    return 9f;
                case ArmorType.Boots:
                    return 12f;
                case ArmorType.Shield:
                    return 9f;
                case ArmorType.LeftGauntlet:
                case ArmorType.RightGauntlet:
                case ArmorType.LeftBracer:
                case ArmorType.RightBracer:
                    return 3f;
                default:
                    throw new InvalidOperationException($"Unimplemented Armor Type: {armorType}");
            }
        }

        public static float GetMediumCutoff(this ArmorType armorType)
        {
            switch (armorType)
            {
                case ArmorType.Helmet:
                    return 4.5f;
                case ArmorType.Cuirass:
                    return 27f;
                case ArmorType.LeftPauldron:
                case ArmorType.RightPauldron:
                    return 9f;
                case ArmorType.Greaves:
                    return 13.5f;
                case ArmorType.Boots:
                    return 18f;
                case ArmorType.Shield:
                    return 13.5f;
                case ArmorType.LeftGauntlet:
                case ArmorType.RightGauntlet:
                case ArmorType.LeftBracer:
                case ArmorType.RightBracer:
                    return 4.5f;
                default:
                    throw new InvalidOperationException($"Unimplemented Armor Type: {armorType}");
            }
        }
    }
}
