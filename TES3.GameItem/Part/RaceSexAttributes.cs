namespace TES3.GameItem.Part
{
    public class RaceSexAttributes
    {

        public int Strength
        {
            get;
            set;
        } = 50;

        public int Intelligence
        {
            get;
            set;
        } = 50;

        public int Willpower
        {
            get;
            set;
        } = 50;

        public int Agility
        {
            get;
            set;
        } = 50;

        public int Speed
        {
            get;
            set;
        } = 50;

        public int Endurance
        {
            get;
            set;
        } = 50;

        public int Personality
        {
            get;
            set;
        } = 50;

        public int Luck
        {
            get;
            set;
        } = 50;

        public float Height
        {
            get;
            set;
        } = 1.0f;

        public float Weight
        {
            get;
            set;
        } = 1.0f;

        public void SetProperties(RaceSexAttributes other)
        {
            other.Strength = Strength;
            other.Intelligence = Intelligence;
            other.Willpower = Willpower;
            other.Agility = Agility;
            other.Speed = Speed;
            other.Endurance = Endurance;
            other.Personality = Personality;
            other.Luck = Luck;
            other.Height = Height;
            other.Weight = Weight;
        }
    }

}
