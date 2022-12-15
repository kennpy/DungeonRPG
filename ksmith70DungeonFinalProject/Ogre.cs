namespace ksmith70DungeonFinalProject
{
    /// <summary>
    /// Author : Kenji Smith
    /// Generic ogre class where we set ogre stats
    /// </summary>
    public class Ogre : Enemy
    {
        public Ogre()
        {
            this.Image = Properties.Resources.Ogre;
            Defense = 8;
            HitPoints = 10;
            Intelligence = 3;
            MagicDefense = 2;
            Name = "Da-Baby";
            Speed = 7;
            SpriteName = "Ogre";
            Strength = 2;
            IsDefending = false;
        }
    }
}