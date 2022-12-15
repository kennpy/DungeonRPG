namespace ksmith70DungeonFinalProject
{
    /// <summary>
    /// Author : Kenji Smith
    /// Generic hero class where specify a special attack
    /// </summary>
    public class Hero : Actor
    {
        public Hero() {}

        /// <summary>
        /// Perform special attack on the specified target
        /// </summary>
        /// <param name="target">The target to attack</param>
        public void Special(Actor target)
        {
            // calculate damage and deduct proper attribute
            int damage = (int)(this.Strength);
            target.HitPoints = target.HitPoints - damage;
        }
    }
}