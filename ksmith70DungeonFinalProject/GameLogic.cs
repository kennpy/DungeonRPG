using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ksmith70DungeonFinalProject
{
    public class GameLogic
    {
        private int currentTurn;
        private int encounters;
        private List<Actor> turnOrder;

        public event EventHandler Update;

        public void EnemyTurn()
        {
            throw new System.NotImplementedException();
        }

        public void GenerateEncounter()
        {
            throw new System.NotImplementedException();
        }

        public void PlayerTurn()
        {
            throw new System.NotImplementedException();
        }

        public void Start()
        {
            // generate all our Actors so we can track their stats (to update gui)
            throw new System.NotImplementedException();
        }

        public void TakeTurn()
        {
            throw new System.NotImplementedException();
        }

        public void OnUpdate()
        {
            throw new System.NotImplementedException();
        }


    }
}