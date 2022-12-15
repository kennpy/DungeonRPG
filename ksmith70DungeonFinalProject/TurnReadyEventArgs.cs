using System;

namespace ksmith70DungeonFinalProject
{
    /// <summary>
    /// Author : Kenji Smith
    /// Event args which store the tag of the actor who is going and whether
    /// the actor is hero or not (attack and enemy respectively)
    /// </summary>
    public class TurnReadyEventArgs : EventArgs
    {
        private string attack;
        private int enemy;

        public string Attack { get => attack; set => attack = value; }
        public int Enemy { get => enemy; set => enemy = value; }
    }
}
