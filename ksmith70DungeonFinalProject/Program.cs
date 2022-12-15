namespace ksmith70DungeonFinalProject
{
    /// <summary>
    /// Author : Kenji Smith
    /// Generic Program class which runs the game
    /// Creates instance of simple dungeon game and runs it
    /// </summary>
    internal static class Program
    {
        static void Main()
        {
            DungeonGame game = new DungeonGame();
            game.Run();
        }
    }
}
