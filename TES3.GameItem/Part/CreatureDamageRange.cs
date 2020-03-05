namespace TES3.GameItem.Part
{
    public class CreatureDamageRange : DamageRange<int>
    {

        protected override bool IsNegative(int value)
        {
            return value < 0;
        }
    }


}
