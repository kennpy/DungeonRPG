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
            Enemy currentEnemy = (Enemy)turnOrder[1];
            Hero target = (Hero)turnOrder[0];

            Random randGenerator = new Random();
            int rand = randGenerator.Next(0,11); // gives big value = chage to 0 or 1 (range)
            string action;
            if(rand <= 2)
            {
                action = "Defend";
            }
            else
            {
                action = "Attack";
            }

            switch (action)
            {
                case "Attack":
                    currentEnemy.Attack(target);
                    // if they are defending reset their defense status
                    target.IsDefending = false;
                    break;
                case "Defend":
                    currentEnemy.Defend();
                    break;
            }
            UpdateEventArgs args = new UpdateEventArgs();
            args.TargetIsHero = true;
            args.Health = target.HitPoints;
            args.TurnTag = 1; // HARD CODED HARD CODED HARD CODED
            args.TargetName = target.Name;

            OnUpdate(this, args);
        }

        public void GenerateEncounter()
        {
            // throw new encounter event so the gui clears the board and
            // updates with the correct actors
            // hero party is already decided at the outset so we only
            // need to update the enemies
            // populate our turn list (but keep all the heroes)

            encounters = encounters + 1;
            // if its the first turn then populate the turnOrder
        }

        public void PlayerTurn(string action, Actor target) {

            // current hero decided by current turn state

            Hero currentHero = (Hero)turnOrder[0];

            UpdateEventArgs args = new UpdateEventArgs();
            // TODO : if its defend then target should be a hero
            // so we could do target.Defend instead of currentHero if that is easier

            switch (action)
            {
                case "Attack":
                    currentHero.Attack(target);
                    args.DefendWasChosen = false;
                    // if they are defending reset their defense status
                    target.IsDefending = false;
                    break;
                case "Defend":
                    args.DefendWasChosen = true;
                    currentHero.Defend();
                    break;
                case "Special":
                    args.DefendWasChosen = false;
                    currentHero.Special(target);
                    // if they are defending reset their defense status
                    target.IsDefending = false;
                    break;
            }

            args.TargetIsHero = false;
            args.Health = target.HitPoints;
            args.TurnTag = 1; // HARD CODED HARD CODED HARD CODED
            args.TargetName = target.Name;
            

            OnUpdate(this, args);
        }
        

        public void Start()
        {
            // generate all our Actors so we can track their stats (to update gui)
            // generate a new encounter based on that actor
            currentTurn = 0;
            GenerateInitialTurnOrder();
            // Actor firstActor = turnOrder[0];

            /*while (GameWon() == false)
            {
                GenerateEncounter();
                // SortTurnOrder(); 
                
                // while encounter is not over keep taking turns
                while (EncounterWon() == false)
                {
                    TakeTurn();

                }
            } */           
        }

        public void TakeTurn(string action, int targetId)
        {
            // check who is actually taking a turn
            // well figure this out later. for now this is what we're doing
            // THIS WILL BREAK WHEN turnIndex IS DECIMAL PROBABLY

            Actor target = turnOrder[targetId];
            // TODO : if its defend then target should be a hero (link 1)

            // TODO : enemy and player can kill eachother at the same time
            // make it so this isnt the case (or ask if we can keep it since its more fun)

            PlayerTurn(action, target);
            EnemyTurn();
            
            /*int turnIndex = currentTurn / turnOrder.Count;
            

            

            Actor currentActor = turnOrder[turnIndex];
            if(currentActor is Hero)
            {
                // send back hero info


            }
            else
            {

            }*/

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
            Dragon dragon = new Dragon();

            turnOrder.Add(fighter);
            turnOrder.Add(dragon); // TODO : randomize this
        }

        private void SortTurnOrder()
        {
            // foreach actor in turnOrder order by speed using bubble sort
        }

        private bool EncounterWon()
        {
            bool wonEncounter = false;
            int numHeroes = 0;
            int numDeadHeroes = 0;
            int numEnemies = 0;
            int numDeadEnemies = 0;

            // get num enemies and heroes
            foreach (var actor in turnOrder)
            {
                if (actor is Enemy)
                {
                    if (actor.HitPoints <= 0)
                    {
                        numDeadEnemies++;
                    }
                    numEnemies++;
                }
                else
                {
                    if (actor.HitPoints <= 0)
                    {
                        numDeadHeroes++;
                    }
                    numHeroes++;
                }
            }

            if(numDeadEnemies == numEnemies || numDeadHeroes == numHeroes)
            {
                wonEncounter = true;
            }
            return wonEncounter;
        }

        private bool GameWon()
        {
            // idk what goes in here 
            // for now game never ends
            return false;

        }

        public void UpdateGUI()
        {
            // pass whether its a hero or enemy turn,
            // their tag number so we kno which one to update
            // their health status so we can update the health bar accordingly
            // if the actor is dead or alive (health less than or equal to 0)

            UpdateEventArgs args = new UpdateEventArgs();
            args.TurnTag = -1;
            args.TargetIsHero = true; // ?? ?? 
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
            TakeTurn(action, enemy);
        }


    }
}