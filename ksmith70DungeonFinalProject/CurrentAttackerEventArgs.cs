using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ksmith70DungeonFinalProject
{
    public class CurrentAttackerEventArgs
    {
        private int playerTag;
        private bool attackerIsHero;

        public int PlayerTag { get => playerTag; set => playerTag = value; }
        public bool AttackerIsHero { get => attackerIsHero; set => attackerIsHero = value; }
    }
}
