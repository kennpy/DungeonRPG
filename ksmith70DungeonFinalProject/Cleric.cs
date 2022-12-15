namespace ksmith70DungeonFinalProject
{
    /// <summary>
    /// Author : Kenji Smith
    /// Generic cleric class where we set cleric stats
    /// </summary>
    public class Cleric : Hero
    {
        public Cleric()
        {
            this.Image = Properties.Resources.Cleric;
            Defense = 8;
            HitPoints = 10;
            Intelligence = 3;
            MagicDefense = 2;
            Name = "Sad Nike Promotion";
            Speed = 5;
            SpriteName = "Cleric";
            Strength = 2;
            IsDefending = false;
        }
    }
}