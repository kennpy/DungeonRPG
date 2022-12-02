using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ksmith70DungeonFinalProject
{
    public class Hero : Actor
    {
        private string specialName;
        private int skillPoints;

        public Hero() 
        {
            
        }

        public void Special(Actor target)
        {
            // calculate damage and deduct proper attribute
            int damage = (int)(this.Strength);
            target.HitPoints = target.HitPoints - damage;
        }
    }
}