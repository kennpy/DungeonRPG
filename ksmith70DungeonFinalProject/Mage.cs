namespace ksmith70DungeonFinalProject
{
    /// <summary>
    /// Author : Kenji Smith
    /// Generic mage class where we set mage stats
    /// </summary>
    public class Mage : Hero
    {
        public Mage()
        {
            this.Image = Properties.Resources.Mage;
            Defense = 8;
            HitPoints = 10;
            Intelligence = 3;
            MagicDefense = 2;
            Name = "Feet Enthusiast";
            Speed = 8;
            SpriteName = "Mage";
            Strength = 2;
            IsDefending = false;
        }
    }
}