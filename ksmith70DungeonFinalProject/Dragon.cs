using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ksmith70DungeonFinalProject
{
    public class Dragon : Enemy
    {

        public Dragon()
        {
            Defense = 8;
            HitPoints = 10;
            Intelligence = 3;
            MagicDefense = 2;
            Name = "Boss Baby Dragon";
            Speed = 5;
            SpriteName = "Dragon";
            Strength = 5;
            IsDefending = false;
        }

    }
}