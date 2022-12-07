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
        }
        /*Image image = Properties.Resources.Ogre;

        public Image Image { get => image; set => image = value; }*/
    }
}