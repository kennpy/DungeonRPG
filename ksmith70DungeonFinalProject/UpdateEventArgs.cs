using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ksmith70DungeonFinalProject
{
    /// <summary>
    /// Author : Kenji Smith
    /// Event args storing the details of an attack.
    /// This includes whos turn it is, health, target,
    /// name, whether action is defense and name of attacker
    /// </summary>
    public class UpdateEventArgs : EventArgs
    {
        private int turnTag;
        private int health;
        private bool targetIsHero;
        private string targetName;
        private bool defendWasChosen;
        private string attackerName;

        public int TurnTag { get => turnTag; set => turnTag = value; }
        public int Health { get => health; set => health = value; }
        public bool TargetIsHero { get => targetIsHero; set => targetIsHero = value; }
        public string TargetName { get => targetName; set => targetName = value; }
        public bool DefendWasChosen { get => defendWasChosen; set => defendWasChosen = value; }
        public string AttackerName { get => attackerName; set => attackerName = value; }
    }
}