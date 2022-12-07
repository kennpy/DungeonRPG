using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ksmith70DungeonFinalProject
{
    public class Ogre : Enemy
    {
        public Ogre()
        {
            this.Image = Properties.Resources.Ogre;
            Defense = 8;
            HitPoints = 10;
            Intelligence = 3;
            MagicDefense = 2;
            Name = "Bad Ogre";
            Speed = 5;
            SpriteName = "Ogre";
            Strength = 2;
            IsDefending = false;
        }
        /*Image image = Properties.Resources.Ogre;

        public Image Image { get => image; set => image = value; }*/
    }
}