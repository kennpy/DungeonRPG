using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ksmith70DungeonFinalProject
{
    public class UpdateEventArgs : EventArgs
    {
        private int heroTurnTag;
        private int health;
        private bool isHero;

        public int HeroTurnTag { get => heroTurnTag; set => heroTurnTag = value; }
        public int Health { get => health; set => health = value; }
        public bool IsHero { get => isHero; set => isHero = value; }
    }
}