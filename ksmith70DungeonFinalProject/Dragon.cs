namespace ksmith70DungeonFinalProject
{
    /// <summary>
    /// Author : Kenji Smith
    /// Generic dragon class where we set dragon stats
    /// </summary>
    public class Dragon : Enemy
    {
        public Dragon()
        {
            Defense = 8;
            HitPoints = 10;
            Intelligence = 3;
            MagicDefense = 2;
            Name = "Sick Sinner";
            Speed = 9;
            SpriteName = "Dragon";
            Strength = 2;
            IsDefending = false;
            this.Image = Properties.Resources.Dragon;
        }

    }
}