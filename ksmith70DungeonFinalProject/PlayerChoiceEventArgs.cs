using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ksmith70DungeonFinalProject
{
    public class PlayerChoiceEventArgs
    {
        private int playerTag;
        private bool attackerIsEnemy;

        public int PlayerTag { get => playerTag; set => playerTag = value; }
        public bool AttackerIsEnemy { get => attackerIsEnemy; set => attackerIsEnemy = value; }
    }
}
