using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ksmith70DungeonFinalProject
{
    public class Fighter : Hero
    {

        /*Image image = Properties.Resources.Fighter;

        public Image Image { get => image; set => image = value; }*/
        public Fighter() {
            Defense = 5;
            HitPoints = 10; // everyone starts with the same hit points ?? 
            Intelligence = 5;
            MagicDefense = 5;
            Name = "Fighter Juan";
            Speed = 3;
            SpriteName = "Punch";
            Strength = 4;
            IsDefending = false;

            this.Image = Properties.Resources.Fighter;

        }


    }
}