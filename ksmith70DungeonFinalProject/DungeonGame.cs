using System.Windows.Forms;

namespace ksmith70DungeonFinalProject
{
    /// <summary>
    /// Author : Kenji Smith
    /// Dungeon RPG Game containing game screen and logic
    /// Players perform attacks and select enemies
    /// High score is saved based on number of encounters beat !!
    /// </summary>
    public class DungeonGame
    {
        private GameScreen screen;
        private GameLogic logic = new GameLogic();

        /// <summary>
        /// Main game loop. Creates screen, subscribes events, and starts game
        /// </summary>
        public void Run()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            screen = new GameScreen();
            
            SubscribeAllHandlers();
            logic.BeginGame();
            
            Application.Run(screen);
        }

        /// <summary>
        /// Subscribes all events present in GameLogic and GameScreen
        /// </summary>
        private void SubscribeAllHandlers()
        {
            logic.Update += screen.OnUpdate_Handler;
            screen.TurnReady += logic.OnTurnReady_Handler;
            logic.NewEncounter += screen.OnNewEncounter_Handler;
            logic.CurrentAttacker += screen.OnPlayerChoice_Handler;
            logic.LostGame += screen.OnLostGame_Handler;
            logic.BeatEncounter += screen.OnBeatEncounter_Handler;
        }
    }
}