using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ksmith70DungeonFinalProject
{
    public class TurnReadyEventArgs : EventArgs
    {
        private string attack;
        private int enemy;

        public string Attack { get => attack; set => attack = value; }
        public int Enemy { get => enemy; set => enemy = value; }
    }
}
