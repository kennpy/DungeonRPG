using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
        private bool gameIsOver = false;

        private const string HIGH_SCORE_FILE_NAME = "high_score.txt";

        private List<Actor> turnOrder = new List<Actor>();
        private List<Hero> playerParty = new List<Hero>();
        private List<Enemy> enemyParty = new List<Enemy>();

        private const int NUM_MAX_ENEMIES = 3;

        public event EventHandler<UpdateEventArgs> Update;
        public event EventHandler<NewEncounterEventArgs> NewEncounter;
        public event EventHandler<CurrentAttackerEventArgs> CurrentAttacker;
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
            string action = ChooseAction();
            // string action = "Attack";

            // *** TODO : don't need to do this if we are picking defense (update later)***

            Hero target = SelectTarget();
            PerformAttack(currentEnemy, action, target);

            UpdateEventArgs args = new UpdateEventArgs();

            if (action == "Defend")
            {
                SetEnemyDefenseArgs(currentEnemy, args);
            }
            else
            {
                SetEnemyAttackArgs(currentEnemy, target, args);
            }

            // currentTurn++;

            OnUpdate(this, args); // update gui

            // check if all playerParty has been killed
            CheckGameOver();
        }

        private static void SetEnemyAttackArgs(Enemy currentEnemy, Hero target, UpdateEventArgs args)
        {
            args.TargetIsHero = true;
            args.TargetName = target.Name;
            args.Health = target.HitPoints;
            args.TurnTag = target.TagNumber;
            args.AttackerName = currentEnemy.Name;
            args.DefendWasChosen = false;
        }

        private static void SetEnemyDefenseArgs(Enemy currentEnemy, UpdateEventArgs args)
        {
            args.TargetIsHero = false;
            args.TargetName = currentEnemy.Name;
            args.Health = currentEnemy.HitPoints;
            args.TurnTag = currentEnemy.TagNumber;
            args.AttackerName = currentEnemy.Name;
            args.DefendWasChosen = true;
        }

        private void PerformAttack(Enemy currentEnemy, string action, Hero target)
        {
            switch (action)
            {
                case "Attack":

                    currentEnemy.Attack(target);
                    target.IsDefending = false;
                    // if hero dies remove from party and turn order
                    if(target.HitPoints <= 0)
                    {
                        playerParty.Remove(target);
                        // turnOrder.Remove(target);
                        // decrement so we are not one ahead in turn order
                        //currentTurn = currentTurn - 1; 
                    }
                    break;
                case "Defend":
                    currentEnemy.Defend();
                    break;
            }
        }

        private Hero SelectTarget()
        {
            // select random hero to attack
            int randTargetId = random.Next(0, playerParty.Count); // MAY NOT WORK MAY NOT WORK (range0
            Hero target = (Hero)playerParty[randTargetId];
            bool targetIsAlive = false;

            while (!(target is Hero) && !targetIsAlive)
            {
                targetIsAlive = false;
                randTargetId = random.Next(0, playerParty.Count); // gives big value = chage to 0 or 1 (range)
                target = (Hero)playerParty[randTargetId];
                if (target.HitPoints > 0)
                {
                    targetIsAlive = true;
                }
            }

            return target;
        }

        private string ChooseAction()
        {
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

            return action;
        }

        // generate new encounter based on heroes / enemies in turn order
        // pass to gui so it knows where / how to update everything
        // take what is in our heroParty array and generate a new encounter with them
        public void GenerateEncounter()
        {
            // get rid of any old enemies that may be present
            turnOrder.RemoveAll(IsEnemy); 
            enemyParty.Clear();

            ConstructEnemies();
            encounters++;
        }

        private void ConstructEnemies()
        {
            int numEnemies = random.Next(1, 4);
            for (int i = 0; i < numEnemies; i++)
            {
                int spawnChoice = random.Next(3);
                switch (spawnChoice)
                {

                    case 0:
                        MakeBandit(i);
                        break;
                    case 1:
                        MakeDragon(i);
                        break;
                    case 2:
                        MakeOgre(i);
                        break;
                }
            }
        }

        private void MakeOgre(int i)
        {
            Ogre ogre = new Ogre();
            ogre.TagNumber = i + 1;
            turnOrder.Add(ogre);
            enemyParty.Add(ogre);
        }

        private void MakeDragon(int i)
        {
            Dragon dragon = new Dragon();
            dragon.TagNumber = i + 1;
            turnOrder.Add(dragon);
            enemyParty.Add(dragon);
        }

        private void MakeBandit(int i)
        {
            Bandit bandit = new Bandit();
            bandit.TagNumber = i + 1;
            turnOrder.Add(bandit);
            enemyParty.Add(bandit);
        }

        private bool IsEnemy(Actor actor)
        {
            return actor is Enemy;
        }

        public void PlayerTurn(string action, Actor target)
        {
            // if the current hero is alive then attack
            // current hero decided by current turn state

            Hero currentHero = (Hero)turnOrder[currentTurn];
            
            // ADD HERE

            UpdateEventArgs args = new UpdateEventArgs();
            PerformAction(action, target, currentHero, args);
            // if we are attacking an enemy update with enemy values
            if (!args.DefendWasChosen)
            {
                MakeHeroAttackArgs(target, currentHero, args);
            }
            else
            {
                MakeHeroDefenseArgs(currentHero, args);
            }

            currentTurn++; // so they don't attack twice
            OnUpdate(this, args);
            // Attack player until it's their turn again
            AttackPlayerConsecutively();

            if (!gameIsOver)
            {
                if (currentTurn == turnOrder.Count)
                {
                    currentTurn = 0;
                }
                IncrementTurnCounter();
                AttackPlayerConsecutively();
                RaiseCurrentAttacker();

                // check if we need to generate new level
                if (EncounterWon())
                {
                    EventArgs beatEncounterArgs = new EventArgs();
                    OnBeatEncounter(this, beatEncounterArgs);
                    UpdateGameStats();
                    StartNewLevel();
                }

            }

        }

        private void IncrementTurnCounter()
        {
            // while the next actor in the list is dead increment
            while (turnOrder[currentTurn].HitPoints <= 0)
            {
                currentTurn++;
                if (currentTurn == turnOrder.Count)
                {
                    currentTurn = 0;
                }
            }
        }

        private void PerformAction(string action, Actor target, Hero currentHero, UpdateEventArgs args)
        {
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
            if(target.HitPoints <= 0 && target is Enemy)
            {
                // currentTurn = currentTurn - 1;
                //enemyParty.Remove((Enemy)target);
                // turnOrder.Remove(target);
            }
        }

        private static void MakeHeroDefenseArgs(Hero currentHero, UpdateEventArgs args)
        {
            args.TargetIsHero = true;
            args.Health = currentHero.HitPoints;
            args.TurnTag = currentHero.TagNumber;
            args.TargetName = currentHero.Name;
            args.AttackerName = currentHero.Name;
        }

        private static void MakeHeroAttackArgs(Actor target, Hero currentHero, UpdateEventArgs args)
        {
            args.TargetIsHero = false;
            args.Health = target.HitPoints;
            args.TurnTag = target.TagNumber;
            args.TargetName = target.Name;
            args.AttackerName = currentHero.Name;
        }



        private void UpdateGameStats()
        {
            try
            {
                UpdateHighScore();
            }
            catch (FileNotFoundException e) // if its our first time playing make new file
            {
                InitializeHighScoreFile();
            }
        }

        private void RaiseCurrentAttacker()
        {
            if (currentTurn < turnOrder.Count)
            {
                CurrentAttackerEventArgs currentAttackerArgs = new CurrentAttackerEventArgs();
                currentAttackerArgs.PlayerTag = turnOrder[currentTurn].TagNumber;

                if (turnOrder[currentTurn] is Hero)
                {
                    currentAttackerArgs.AttackerIsHero = true;
                }
                else
                {
                    currentAttackerArgs.AttackerIsHero = false;
                }

                OnCurrentAttacker(this, currentAttackerArgs);
            }
        }

        private void UpdateHighScore()
        {
            // get the current high score
            StreamReader reader = new StreamReader(HIGH_SCORE_FILE_NAME);
            string score = reader.ReadLine();
            int highest_score = Int32.Parse(score);
            reader.Close();
            // if numEcounters greater than high score update high score
            if(encounters > highest_score)
            {
                StreamWriter writer = new StreamWriter(HIGH_SCORE_FILE_NAME);
                writer.WriteLine(encounters);
                writer.Close();
            }
        }
       
        private void InitializeHighScoreFile()
        {
            StreamWriter writer = File.CreateText(HIGH_SCORE_FILE_NAME);
            writer.WriteLine("0");
            writer.Close();
        }

        public void StartNewLevel()
        {
            // generate all our Actors so we can track their stats (to update gui)
            // generate a new encounter based on that actor
            // if its the first encounter generate a new party
            if (encounters == 0)
            {
                currentTurn = 0;
                GeneratePlayerParty(); // this is run ONCE since we have ONE party
            }
            GenerateEncounter(); // make enemies and add to turnOrder
                                 // generate event args and populate bbitmaps 
            
            SortTurnOrder();
            NewEncounterEventArgs args = new NewEncounterEventArgs();
            CreateNewEncounterArgs(args);
            OnNewEncounter(this, args);

            AttackPlayerConsecutively();
        }

        private void AttackPlayerConsecutively()
        {
            while (currentTurn < turnOrder.Count && turnOrder[currentTurn] is Enemy) // THIS IS SKETCHY (range reset - idk if this workes when someone dies)
            {
                // if enemy is alive and heroes aren't dead then perform an attack
                if (turnOrder[currentTurn].HitPoints > 0 && playerParty.Count > 0)
                {
                    EnemyTurn();
                }

                // ADD ONE HERE (potentially)

                currentTurn++;

                // check if next turn is player so we can enabled buttons
                // and allow the correct box to be highlighted
                if (currentTurn == turnOrder.Count)
                {
                    currentTurn = 0;
                }
                RaiseCurrentAttacker();
            }
            if (currentTurn == turnOrder.Count)
            {
                currentTurn = 0;
            }
        }

        private void CreateNewEncounterArgs(NewEncounterEventArgs args)
        {
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

        private void SortTurnOrder()
        {
            // foreach actor in turnOrder order by speed using bubble sort
            int[] sortedArr = new int[turnOrder.Count];
            for(int i = 0; i < turnOrder.Count; i++)
            {
                sortedArr[i] = turnOrder[i].Speed;
            }
            //BubbleSort(sortedArr, sortedArr.Length);
            turnOrder = turnOrder.OrderBy(actor => actor.Speed).ToList();
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
            foreach (var hero in playerParty)
            {
                if (hero.HitPoints <= 0)
                {
                    numDead++;
                }
            }
            if (numDead == playerParty.Count)
            {
                gameIsOver = true;
                EventArgs args = new EventArgs();
                OnLostGame(this, args);
            }

            // NEW CODE

            /*if(playerParty.Count == 0)
            {
                gameIsOver = true;
                EventArgs args = new EventArgs();
                OnLostGame(this, args);
            }*/

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
        protected virtual void OnCurrentAttacker(object sender, CurrentAttackerEventArgs e)
        {
            CurrentAttacker.Invoke(this, e);
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
                enemy = (Actor)playerParty[enemyId];
            }
            else
            {
                enemy = (Actor)enemyParty[enemyId];
            }
            if(currentTurn >= turnOrder.Count)
            {
                currentTurn = 1; // USED TO BE 0
            }

            PlayerTurn(action, enemy);

        }


    }
}