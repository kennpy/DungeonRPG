using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ksmith70DungeonFinalProject
{
    public class UpdateEventArgs : EventArgs
    {
        private int turnTag;
        private int health;
        private bool targetIsHero;
        private string targetName;
        private bool defendWasChosen;

        public int TurnTag { get => turnTag; set => turnTag = value; }
        public int Health { get => health; set => health = value; }
        public bool TargetIsHero { get => targetIsHero; set => targetIsHero = value; }
        public string TargetName { get => targetName; set => targetName = value; }
        public bool DefendWasChosen { get => defendWasChosen; set => defendWasChosen = value; }
    }
}