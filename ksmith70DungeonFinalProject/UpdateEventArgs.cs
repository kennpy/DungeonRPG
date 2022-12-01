using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ksmith70DungeonFinalProject
{
    public class UpdateEventArgs : EventArgs
    {
        private int heroTurnTag;
        public int HeroTurnTag { get => heroTurnTag; set => heroTurnTag = value; }

        // adding this

    }
}