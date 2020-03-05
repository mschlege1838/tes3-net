namespace TES3.GameItem.Part
{
    public class WeaponDamageRange : DamageRange<byte>
    {

        public WeaponDamageRange(byte min, byte max)
        {
            SetRange(min, max);
        }

        protected override bool IsNegative(byte value)
        {
            return value < 0;
        }
    }


}
