using System;

namespace ksmith70DungeonFinalProject
{
    /// <summary>
    /// Author : Kenji Smith
    /// Args which store the tag of the actor who is going and whether
    /// the actor is hero or not
    /// </summary>
    public class CurrentAttackerEventArgs : EventArgs
    {
        private int playerTag;
        private bool attackerIsHero;

        public int PlayerTag { get => playerTag; set => playerTag = value; }
        public bool AttackerIsHero { get => attackerIsHero; set => attackerIsHero = value; }
    }
}
