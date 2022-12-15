using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ksmith70DungeonFinalProject
{
    public class Fighter : Hero
    {
        public Fighter() {
            Defense = 5;
            HitPoints = 10;
            Intelligence = 5;
            MagicDefense = 5;
            Name = "Holy Christian";
            Speed = 3;
            SpriteName = "Punch";
            Strength = 4;
            IsDefending = false;

            this.Image = Properties.Resources.Fighter;
        }


    }
}