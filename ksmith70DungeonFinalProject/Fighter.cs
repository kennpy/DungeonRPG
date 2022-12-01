using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ksmith70DungeonFinalProject
{
    public class Fighter : Hero
    {
        public Fighter() {
            Defense = 5;
            HitPoints = 5;
            Intelligence = 5;
            MagicDefense = 5;
            Name = "Juan";
            Speed = 5;
            SpriteName = "Punch";
            Strength = 5;
            IsDefending = false;
        }


    }
}