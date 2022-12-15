namespace ksmith70DungeonFinalProject
{
    /// <summary>
    /// Author : Kenji Smith
    /// Generic bandit class where we set bandit stats
    /// </summary>
    public class Bandit : Enemy
    {
        public Bandit()
        {
            this.Image = Properties.Resources.Bandit;
            Defense = 8;
            HitPoints = 10;
            Intelligence = 3;
            MagicDefense = 2;
            Name = "Jerry";
            Speed = 5;
            SpriteName = "Bandit";
            Strength = 2;
            IsDefending = false;
        }
    }
}