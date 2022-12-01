using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ksmith70DungeonFinalProject
{
    public class GameLogic
    {
        private int currentTurn;
        private int encounters;
        private List<Actor> turnOrder;
        
        public event EventHandler<UpdateEventArgs> Update;

        public void EnemyTurn()
        {
            // generate an attack on a specific hero
            // 
            throw new System.NotImplementedException();
        }

        public void GenerateEncounter()
        {
            // throw new encounter event so the gui clears the board and
            // updates with the correct actors
            // hero party is already decided at the outset so we only
            // need to update the enemies
            // populate our turn list (but keep all the heroes)
            
            // if its the first turn then populate the turnOrder
            if(currentTurn == 0)
            {
                GenerateInitialTurnOrder();
            }
            


            throw new System.NotImplementedException();
        }

        public void PlayerTurn()
        {
            throw new System.NotImplementedException();
        }

        public void Start()
        {
            // generate all our Actors so we can track their stats (to update gui)
            // generate a new encounter based on that actor
            currentTurn = 0;
            GenerateEncounter();
            // SortTurnOrder(); 
            TakeTurn();

            
            throw new System.NotImplementedException();
        }

        public void TakeTurn()
        {
            // check who is actually taking a turn

            Actor currentActor = turnOrder[currentTurn];
            if(currentActor is Hero)
            {
                // send back hero info


            }
            else
            {

            }

            throw new System.NotImplementedException();
        }

        // private void UpdateTurnOrder
        private void GenerateInitialTurnOrder()
        {
            Random randGenerator = new Random();
            int randInt = randGenerator.Next();
            turnOrder = new List<Actor>();
            // FOR NOW ONLY ONE ACTOR BUT UPDATE TO 3 LATER
            // Heroes are default so we dont need to randomly select them

            Fighter fighter = new Fighter();

            turnOrder.Add(new Fighter());
            turnOrder.Add(new Dragon()); // TODO : randomize this
        }

        private void SortTurnOrder()
        {
            // foreach actor in turnOrder order by speed using bubble sort
        }

        public void UpdateGUI()
        {
            // pass whether its a hero or enemy turn,
            // their tag number so we kno which one to update
            // their health status so we can update the health bar accordingly
            // if the actor is dead or alive (health less than or equal to 0)

            UpdateEventArgs args = new UpdateEventArgs();
            args.HeroTurnTag = 1;
            OnUpdate(this, args);
        }

        protected virtual void OnUpdate(object sender, UpdateEventArgs e)
        {
            Update.Invoke(this, e);
        }
        public void OnTurnReady_Handler(object sender, TurnReadyEventArgs e)
        {
            // what is the action
            string action = e.Attack;
            int enemy = e.Enemy; 
        }


    }
}