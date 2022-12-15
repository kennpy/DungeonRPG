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
    /// <summary>
    /// Author : Kenji Smith
    /// Handles game logic for dungeon game. 
    /// Tracks turns, encounters, and turn orders,
    /// calculating and generating attacks based on whos turn it is.
    /// Tracks high score and passes game data to GameScreen.cs to update GUI
    /// </summary>
    public class GameLogic
    {
        private int currentTurn;
        private int encounters = 0;
        private bool gameIsOver = false;

        private const string HIGH_SCORE_FILE_NAME = "high_score.txt";

        // Track turns and who is alive
        private List<Actor> turnOrder = new List<Actor>();
        private List<Hero> playerParty = new List<Hero>();
        private List<Enemy> enemyParty = new List<Enemy>();

        // Our logic events
        public event EventHandler<UpdateEventArgs> Update;
        public event EventHandler<NewEncounterEventArgs> NewEncounter;
        public event EventHandler<CurrentAttackerEventArgs> CurrentAttacker;
        public event EventHandler<EventArgs> BeatEncounter;
        public event EventHandler<EventArgs> LostGame;

        private Random random = new Random();


        /// <summary>
        /// Generate attack on random hero and event data to gui
        /// </summary>
        public void EnemyTurn()
        {
            // select enemy and perform attack
            Enemy currentEnemy = (Enemy)turnOrder[currentTurn];
            string action = ChooseAction();
            Hero target = SelectTargetHero();
            PerformEnemyAction(currentEnemy, action, target);

            // tell gui who was attacked and their health is
            UpdateEventArgs args = new UpdateEventArgs();
            if (action == "Defend")
            {
                SetEnemyDefenseArgs(currentEnemy, args);
            }
            else
            {
                SetEnemyAttackArgs(currentEnemy, target, args);
            }

            OnUpdate(this, args); 

            // check if all playerParty has been killed
            CheckGameOver();
        }

        /// <summary>
        /// Set the args for an enemy attack to be passed to GUI
        /// </summary>
        /// <param name="currentEnemy">The enemy who is attacking</param>
        /// <param name="target">The target of the attack</param>
        /// <param name="args">The args to pass to GUI</param>
        private void SetEnemyAttackArgs(Enemy currentEnemy, Hero target, UpdateEventArgs args)
        {
            args.TargetIsHero = true;
            args.TargetName = target.Name;
            args.Health = target.HitPoints;
            args.TurnTag = target.TagNumber;
            args.AttackerName = currentEnemy.Name;
            args.DefendWasChosen = false;
        }

        /// <summary>
        /// Set the args for an enemy defense to be passed to the GUI
        /// </summary>
        /// <param name="currentEnemy">The enemy who is defending</param>
        /// <param name="args">The args to be passed</param>
        private void SetEnemyDefenseArgs(Enemy currentEnemy, UpdateEventArgs args)
        {
            args.TargetIsHero = false;
            args.TargetName = currentEnemy.Name;
            args.Health = currentEnemy.HitPoints;
            args.TurnTag = currentEnemy.TagNumber;
            args.AttackerName = currentEnemy.Name;
            args.DefendWasChosen = true;
        }

        /// <summary>
        /// perform an action on the specified target / enemy
        /// </summary>
        /// <param name="currentEnemy">The current enemy taking the action</param>
        /// <param name="action">The action they selected</param>
        /// <param name="target">Who their target is</param>
        private void PerformEnemyAction(Enemy currentEnemy, string action, Hero target)
        {
            switch (action)
            {
                case "Attack":
                    // Perform attack and reset their defense status
                    currentEnemy.Attack(target);
                    target.IsDefending = false;
                    
                    // if hero dies remove from party 
                    if(target.HitPoints <= 0)
                    {
                        playerParty.Remove(target); 
                    }
                    break;
                case "Defend":
                    currentEnemy.Defend();
                    break;
            }
        }

        /// <summary>
        /// Select a target hero who is alive
        /// </summary>
        /// <returns>Hero that has been selected</returns>
        private Hero SelectTargetHero()
        {
            // select random hero to attack
            int randTargetId = random.Next(0, playerParty.Count);
            Hero target = (Hero)playerParty[randTargetId];
            bool targetIsAlive = false;

            // Make sure the target is alive and a hero
            while (!(target is Hero) && !targetIsAlive)
            {
                targetIsAlive = false;
                randTargetId = random.Next(0, playerParty.Count); 
                target = (Hero)playerParty[randTargetId];
                if (target.HitPoints > 0)
                {
                    targetIsAlive = true;
                }
            }

            return target;
        }

        /// <summary>
        /// Chooses random action and returns it
        /// </summary>
        /// <returns>The action that  has been selected</returns>
        private string ChooseAction()
        {
            int rand = random.Next(0, 11); 
            string action;
            if (rand <= 1)
            {
                action = "Defend";
            }
            else
            {
                action = "Attack";
            }

            return action;
        }

        /// <summary>
        /// Generate a new encounter updating turnOrder and enemy party as needed
        /// </summary>
        public void GenerateEncounter()
        {
            // get rid of any old enemies that may be present
            turnOrder.RemoveAll(IsEnemy); 
            enemyParty.Clear();

            // make new enemies
            ConstructEnemies();
            encounters++;
        }

        /// <summary>
        /// Constructs random number of enemies and adds them 
        /// to turnOrder / enemy party
        /// </summary>
        private void ConstructEnemies()
        {
            // select random number of enemies
            int numEnemies = random.Next(1, 4);
            // create the specified number of enemies
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

        /// <summary>
        /// Makes an ogre and adds it to turnOrder / enemyParty
        /// </summary>
        /// <param name="i">The tag number to add</param>
        private void MakeOgre(int i)
        {
            Ogre ogre = new Ogre();
            ogre.TagNumber = i + 1;
            turnOrder.Add(ogre);
            enemyParty.Add(ogre);
        }

        /// <summary>
        /// Makes a dragon and adds it to turnOrder / enemyParty
        /// </summary>
        /// <param name="i">The tag number to add</param>
        private void MakeDragon(int i)
        {
            Dragon dragon = new Dragon();
            dragon.TagNumber = i + 1;
            turnOrder.Add(dragon);
            enemyParty.Add(dragon);
        }

        /// <summary>
        /// Makes a bandit and adds it to turnOrder / enemyParty
        /// </summary>
        /// <param name="i">The tag number to add</param>
        private void MakeBandit(int i)
        {
            Bandit bandit = new Bandit();
            bandit.TagNumber = i + 1;
            turnOrder.Add(bandit);
            enemyParty.Add(bandit);
        }

        /// <summary>
        /// Checks if actor is an enemy
        /// </summary>
        /// <param name="actor"></param>
        /// <returns>True if actor is enemy, false if otherwise</returns>
        private bool IsEnemy(Actor actor)
        {
            return actor is Enemy;
        }

        /// <summary>
        /// Performs player action and tells GUI what happened
        /// Checks if game has ended and performs enemy attacks
        /// until it is user's turn again
        /// </summary>
        /// <param name="action">The action to perform</param>
        /// <param name="target">The target to act upon</param>
        public void PlayerTurn(string action, Actor target)
        {
            // select hero and perform the action user has chosen
            Hero currentHero = (Hero)turnOrder[currentTurn];
            UpdateEventArgs args = new UpdateEventArgs();
            PerformAction(action, target, currentHero, args);
            
            // if we are attacking an enemy update with enemy values
            if (!args.DefendWasChosen)
            {
                MakeHeroAttackArgs(target, currentHero, args);
            }
            // else update with hero values so we can say the hero defended
            else
            {
                MakeHeroDefenseArgs(currentHero, args);
            }
            // let GameScreen know what happened
            currentTurn++; 
            OnUpdate(this, args);
            
            // Attack player until it's their turn again and check if game has ended
            AttackPlayerConsecutively();
            if (!gameIsOver)
            {
                ContinuePlaying();
            }
        }

        /// <summary>
        /// Continues performing enemy attacks until it's users turn again.
        /// Checks if encounter has been won
        /// </summary>
        private void ContinuePlaying()
        {
            // reset so we are not out of bounds
            if (currentTurn == turnOrder.Count)
            {
                currentTurn = 0;
            }
            // increment until current attacker is alive and attack
            IncrementTurnCounter();
            AttackPlayerConsecutively();
            // tell the GUI who's turn it is
            RaiseCurrentAttacker();

            // check if we need to generate new level or update the high score
            if (EncounterWon())
            {
                EventArgs beatEncounterArgs = new EventArgs();
                OnBeatEncounter(this, beatEncounterArgs);
                UpdateGameStats();
                StartNewLevel();
            }
        }

        /// <summary>
        /// Increments currentTurn while the current actor is dead 
        /// We do this so current attacker is someone alive (valid)
        /// </summary>
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

        /// <summary>
        /// Performs the specified action and updates target values
        /// </summary>
        /// <param name="action">The action being taken</param>
        /// <param name="target">The actor targeted</param>
        /// <param name="currentHero">The hero who is attacking</param>
        /// <param name="args">The details of the attack</param>
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
        }

        /// <summary>
        /// Set defense args of hero 
        /// We do this because a defense does not attack an enemy
        /// so the gui must handle this differently
        /// </summary>
        /// <param name="currentHero">The current hero defending</param>
        /// <param name="args">The args we are populating</param>
        private void MakeHeroDefenseArgs(Hero currentHero, UpdateEventArgs args)
        {
            args.TargetIsHero = true;
            args.Health = currentHero.HitPoints;
            args.TurnTag = currentHero.TagNumber;
            args.TargetName = currentHero.Name;
            args.AttackerName = currentHero.Name;
        }

        /// <summary>
        /// Creates the attack args specifying the attack details
        /// </summary>
        /// <param name="target">The actor being targeted</param>
        /// <param name="currentHero">The hero taking the action</param>
        /// <param name="args">The args to populate</param>
        private void MakeHeroAttackArgs(Actor target, Hero currentHero, UpdateEventArgs args)
        {
            args.TargetIsHero = false;
            args.Health = target.HitPoints;
            args.TurnTag = target.TagNumber;
            args.TargetName = target.Name;
            args.AttackerName = currentHero.Name;
        }

        /// <summary>
        /// Updates high score game stats
        /// </summary>
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

        /// <summary>
        /// Invokes current attaacker telling gui who's turn it is
        /// </summary>
        private void RaiseCurrentAttacker()
        {
            if (currentTurn < turnOrder.Count)
            {
                // create the args
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

        /// <summary>
        /// Update the high score if the current score is higher
        /// Reads and write to high score file
        /// </summary>
        private void UpdateHighScore()
        {
            // get the current high score
            StreamReader reader = new StreamReader(HIGH_SCORE_FILE_NAME);
            string score = reader.ReadLine();
            int highest_score = Int32.Parse(score);
            reader.Close();
            // if encounters greater than high score update high score
            if(encounters > highest_score)
            {
                StreamWriter writer = new StreamWriter(HIGH_SCORE_FILE_NAME);
                writer.WriteLine(encounters);
                writer.Close();
            }
        }
       
        /// <summary>
        /// Sets the initial high score to zero if no score is present
        /// </summary>
        private void InitializeHighScoreFile()
        {
            StreamWriter writer = File.CreateText(HIGH_SCORE_FILE_NAME);
            writer.WriteLine("0");
            writer.Close();
        }

        /// <summary>
        /// Starts new level of game, generating parties and encounters
        /// Tells gui the details of this encounter and performs enemy attacks
        /// while its the enemies turn
        /// </summary>
        public void StartNewLevel()
        {
            // if its the first encounter set currentTurn and generate heroes
            if (encounters == 0)
            {
                currentTurn = 0;
                GeneratePlayerParty(); 
            }
            // generate the new encounter / turnOrder and let GUI know the  
            // details of this encounter
            GenerateEncounter(); 
            SortTurnOrder();
            NewEncounterEventArgs args = new NewEncounterEventArgs();
            CreateNewEncounterArgs(args);
            OnNewEncounter(this, args);
            
            // perform enemy attacks if they are first in the turn order
            AttackPlayerConsecutively();
        }

        /// <summary>
        /// Attacks the player while it is an enemies turn
        /// </summary>
        private void AttackPlayerConsecutively()
        {
            while (currentTurn < turnOrder.Count && turnOrder[currentTurn] is Enemy) // THIS IS SKETCHY (range reset - idk if this workes when someone dies)
            {
                // if enemy is alive and heroes aren't dead then perform an attack
                if (turnOrder[currentTurn].HitPoints > 0 && playerParty.Count > 0)
                {
                    EnemyTurn();
                }
                currentTurn++;

                // check if next turn is player so we can enabled buttons
                // and allow the correct box to be highlighted
                if (currentTurn == turnOrder.Count)
                {
                    currentTurn = 0;
                }
                RaiseCurrentAttacker();
            }
            // if we are out of bounds reset to beginning of turn order
            if (currentTurn == turnOrder.Count)
            {
                currentTurn = 0;
            }
        }

        /// <summary>
        /// Populate new encounter args with enemy / hero data
        /// including their sprite image and current hit points
        /// </summary>
        /// <param name="args">The args to populate</param>
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