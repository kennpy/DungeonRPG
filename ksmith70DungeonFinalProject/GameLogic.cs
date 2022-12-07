using System;
using System.Collections.Generic;
using System.Drawing;
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
        private int encounters = 0;

        private List<Actor> turnOrder = new List<Actor>();
        private List<Hero> playerParty = new List<Hero>();
        private List<Enemy> enemyParty = new List<Enemy>();

        private const int NUM_MAX_ENEMIES = 3;

        public event EventHandler<UpdateEventArgs> Update;
        public event EventHandler<NewEncounterEventArgs> NewEncounter;
        public event EventHandler<PlayerChoiceEventArgs> PlayerChoice;
        public event EventHandler<EventArgs> BeatEncounter;
        public event EventHandler<EventArgs> LostGame;

        private Random random = new Random();

        /*
         * CAN ADD MORE EVENTS ITS OK !!
         */

        // seperate event to load the sprites so we don't have overloaded eventargs
        // newRound or newEnctouner event vs update with damage
        // dont need to send sprites over again

        public void EnemyTurn()
        {
            // generate an attack on a specific hero

            Enemy currentEnemy = (Enemy)turnOrder[currentTurn];

            // Ban defense since it is causing errors (cancel for debug)
            int rand = random.Next(0, 11); // gives big value = chage to 0 or 1 (range)
            string action;
            if (rand <= 2)
            {
                action = "Defend";
            }
            else
            {
                action = "Attack";
            }
            // string action = "Attack";

            // *** TODO : don't need to do this if we are picking defense (update later)***

            // select random hero to attack
            Random randGenerator = new Random();
            int randTargetId = randGenerator.Next(0, playerParty.Count); // MAY NOT WORK MAY NOT WORK (range0

            Hero target = (Hero)playerParty[randTargetId];
            bool targetIsAlive = false;

            while (!(target is Hero) && !targetIsAlive)
            {
                randTargetId = randGenerator.Next(0, playerParty.Count); // gives big value = chage to 0 or 1 (range)
                target = (Hero)playerParty[randTargetId];
                if (target.HitPoints > 0)
                {
                    targetIsAlive = true;
                }
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

            if(action == "Defend")
            {
                args.TargetIsHero = false;
                args.TargetName = currentEnemy.Name;
                args.Health = currentEnemy.HitPoints;
                args.TurnTag = currentEnemy.TagNumber;
                args.AttackerName = currentEnemy.Name;
                args.DefendWasChosen = true;
            }
            else
            {
                args.TargetIsHero = true;
                args.TargetName = target.Name;
                args.Health = target.HitPoints;
                args.TurnTag = target.TagNumber;
                args.AttackerName = currentEnemy.Name;
                args.DefendWasChosen = false;
            }

            // currentTurn++;

            OnUpdate(this, args); // update gui

            // check if next turn is player so we can enabled buttons
          /*  if (turnOrder[currentTurn] is Hero)
            {
                PlayerChoiceEventArgs playerChoiceArgs = new PlayerChoiceEventArgs();
                playerChoiceArgs.PlayerTag = turnOrder[currentTurn].TagNumber;
                OnPlayerChoice(this, playerChoiceArgs);
            }*/

            // check if all playerParty has been killed
            CheckGameOver();
        }

        // generate new encounter based on heroes / enemies in turn order
        // pass to gui so it knows where / how to update everything
        // take what is in our heroParty array and generate a new encounter with them
        public void GenerateEncounter()
        {
            
            Random random = new Random();

            // get rid of any old enemies that may be present

            // clear turn party of enemies
            for (int i = 0; i < turnOrder.Count; i++)
            {
                if(turnOrder[i] is Enemy)
                {
                    turnOrder.Remove(turnOrder[i]);
                }
            }
            // clear enemyParty of enemies
            enemyParty.Clear();

            int numEnemies = random.Next(1, 4);

            for (int i = 0; i < numEnemies; i++)
            {
                int spawnChoice = random.Next(3);
                switch (spawnChoice)
                {

                    case 0:
                        Bandit bandit = new Bandit();
                        bandit.TagNumber = i + 1;
                        turnOrder.Add(bandit);
                        enemyParty.Add(bandit);
                        break;
                    case 1:
                        Dragon dragon = new Dragon();
                        dragon.TagNumber = i + 1;
                        turnOrder.Add(dragon);
                        enemyParty.Add(dragon);

                        break;
                    case 2:
                        Ogre ogre = new Ogre();
                        ogre.TagNumber = i + 1;
                        turnOrder.Add(ogre);
                        enemyParty.Add(ogre);

                        break;
                }
            }

            // throw new encounter event so the gui clears the board and
            // updates with the correct actors
            // hero party is already decided at the outset so we only
            // need to update the enemies
            // populate our turn list (but keep all the heroes)

            // populate our event args based on the turn order
            // on the gui side, each picture box will have a tag based on the 
            // position of the actor in the turnOrder
            // so when we generate attacks it is based on the turnOrder. (works hopefully)

            // basically passing our turn order to gui and updating num encounters

            encounters = encounters + 1;
            // if its the first turn then populate the turnOrder
        }


        public void PlayerTurn(string action, Actor target) {

            // current hero decided by current turn state

            Hero currentHero = (Hero)turnOrder[currentTurn];


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
            // if we are attacking an enemy update with enemy values
            if (!args.DefendWasChosen)
            {
                args.TargetIsHero = false;
                args.Health = target.HitPoints;
                args.TurnTag = target.TagNumber;
                args.TargetName = target.Name;
                args.AttackerName = currentHero.Name;
            }
            else
            {
                args.TargetIsHero = true;
                args.Health = currentHero.HitPoints;
                args.TurnTag = currentHero.TagNumber;
                args.TargetName = currentHero.Name;
                args.AttackerName = currentHero.Name;
            }
            


            // else let gui know defend was chosen so the target is the hero which selected defense


            currentTurn++;

            OnUpdate(this, args);

            // run enemy turns until it is user turn again
            while (currentTurn < turnOrder.Count && turnOrder[currentTurn] is Enemy ) // THIS IS SKETCHY (range reset - idk if this workes when someone dies)
            {
                // if enemy is alive, then perform an attack
                if(turnOrder[currentTurn].HitPoints > 0)
                {
                    EnemyTurn();
                    currentTurn++;
                }
            }
            if(currentTurn == turnOrder.Count)
            {
                currentTurn = 0; // reset current turn so we repeat the loop
            }

            // check if we need to generate new level
            if (EncounterWon())
            {
                StartNewLevel();
            }

        }
        

        public void StartNewLevel()
        {
            // generate all our Actors so we can track their stats (to update gui)
            // generate a new encounter based on that actor
            currentTurn = 0;
            
            // if its the first encounter generate a new party
            if(encounters == 0)
            {
                GeneratePlayerParty(); // this is run ONCE since we have ONE party
            }
                                   // GenerateInitialTurnOrder(); // create the turn order built on top of player party

         
                GenerateEncounter(); // make enemies and add to turnOrder
                                     // generate event args and populate bbitmaps 
                NewEncounterEventArgs args = new NewEncounterEventArgs();
                for (int i = 0; i < turnOrder.Count; i++)
                {
                    Actor actor = turnOrder[i];
                    if (actor is Hero)
                    {
                        args.HeroSprites.Add((Bitmap)(actor.Image)); // add images
                        args.HeroHealth.Add((actor.HitPoints)); // add health
                    }
                    else
                    {
                        args.EnemySprites.Add((Bitmap)(actor.Image));
                        args.EnemyHealth.Add((actor.HitPoints));
                    }
                }

                OnNewEncounter(this, args);

                // while there are enemies who must play their turn play their turn
                // else we wait for user to select their turn then repeat the proces
                while (turnOrder[currentTurn] is Enemy)
                {
                    if(turnOrder[currentTurn].HitPoints > 0)
                    {
                        EnemyTurn();
                        currentTurn++;
                    }
                }
            
            // spaw heroes

            //generate encounter

            // Actor firstActor = turnOrder[0];
            //OUTDATED
            /*while (GameWon() == false)
            {
                GenerateEncounter();
                // SortTurnOrder(); 

                // while encounter is not over keep taking turns
                while (EncounterWon() == false)
                {
                    TakeTurn();

                }
            }*/
        }


        // private void UpdateTurnOrder
        // Generates initial turn order inlcuding heroes and enemies
        // we  need this seperate from generate encounter since generate encounter
        // relies on the hero part to already be present before creating enemies (so it can sort them)
        // we generate initial encounter after generate initial turn order is called
        
        private void GeneratePlayerParty()
        {
            // populate turn order with newly made actor party

            Fighter fighter = new Fighter();
            Mage mage = new Mage();
            Cleric cleric = new Cleric();

            fighter.TagNumber = 1;
            mage.TagNumber = 2;
            cleric.TagNumber = 3;

            turnOrder.Add(fighter);
            turnOrder.Add(mage);
            turnOrder.Add(cleric);

            playerParty.Add(fighter);
            playerParty.Add(mage);
            playerParty.Add(cleric);

        }

        private void GenerateInitialTurnOrder()
        {
            // take our hero party and 

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
            int numEnemies = 0;
            int numDeadEnemies = 0;

            // get num enemies and heroes
            foreach (var actor in enemyParty)
            {
                if (actor.HitPoints <= 0)
                {
                    numDeadEnemies++;
                }
                numEnemies++;
               
            }

            if(numDeadEnemies == numEnemies)
            {
                wonEncounter = true;
            }
            return wonEncounter;
        }

        private void CheckGameOver()
        {
            // if all heroes have died end game. dungeon never ends !!!
            int numDead = 0;
            foreach(var hero in playerParty)
            {
                if(hero.HitPoints <= 0)
                {
                    numDead++;
                }
            }
            if(numDead == playerParty.Count)
            {
                EventArgs args = new EventArgs();
                OnLostGame(this, args);
            }
        }

        public void UpdateGUI()
        {
            // pass whether its a hero or enemy turn,
            // their tag number so we kno which one to update
            // their health status so we can update the health bar accordingly
            // if the actor is dead or alive (health less than or equal to 0)


            StartNewLevel();

            // dont need this since start will handle everything inlcuding generate encounter

            UpdateEventArgs args = new UpdateEventArgs();
            args.TurnTag = -1;
            args.TargetIsHero = true; // ?? ?? 

            OnUpdate(this, args);
        }


        protected virtual void OnNewEncounter(object sender, NewEncounterEventArgs e)
        {
            NewEncounter.Invoke(this, e);
        }
        protected virtual void OnUpdate(object sender, UpdateEventArgs e)
        {
            Update.Invoke(this, e);
        }
        protected virtual void OnPlayerChoice(object sender, PlayerChoiceEventArgs e)
        {
            PlayerChoice.Invoke(this, e);
        }
        protected virtual void OnBeatEncounter(object sender, EventArgs e)
        {
            BeatEncounter.Invoke(this, e);
        }
        protected virtual void OnLostGame(object sender, EventArgs e)
        {
            LostGame.Invoke(this, e);
        }
        public void OnTurnReady_Handler(object sender, TurnReadyEventArgs e)
        {
            // generate player turn
            // dont need attacker since we know that based on currentTurn
            string action = e.Attack;
            int enemyId = e.Enemy - 1; // prevent out of range error 
            Actor enemy;
            if(action == "Defend")
            {
                enemy = playerParty[enemyId];

            }
            else
            {
                enemy = enemyParty[enemyId];
            }

            PlayerTurn(action, enemy);

        }


    }
}